import {
  Component,
  ElementRef,
  AfterViewInit,
  OnDestroy,
  NgZone,
  viewChild,
  ChangeDetectionStrategy,
  inject,
  input,
  OnInit
} from '@angular/core';
import { TranslocoModule } from '@jsverse/transloco';
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';
import gsap from 'gsap';

@Component({
  selector: 'app-singularity',
  standalone: true,
  imports: [TranslocoModule],
  templateUrl: './singularity.component.html',
  styleUrl: './singularity.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    class: 'block w-full h-full relative overflow-hidden'
  }
})
export class SingularityComponent implements OnInit, AfterViewInit, OnDestroy {
  private ngZone = inject(NgZone);
  private canvas = viewChild<ElementRef<HTMLCanvasElement>>('singularityCanvas');

  translationScope = input<string>('landing');
  
  // Three.js instances
  private scene!: THREE.Scene;
  private camera!: THREE.PerspectiveCamera;
  private renderer!: THREE.WebGLRenderer;
  private controls!: OrbitControls;
  private clock = new THREE.Clock();
  
  // Shaders & Meshes
  private auraMat!: THREE.ShaderMaterial;
  private diskMaterial!: THREE.ShaderMaterial;
  private instancedDisk!: THREE.InstancedMesh;
  
  // Animation State
  private stateIdx = 0;
  private camControl = { distance: 85 };
  private isVisible = true;
  private observer!: IntersectionObserver;
  
  // HUD Data (Signals or plain objects for now)
  public currentTitleKey = '';
  public currentStatusKey = '';
  public currentVel = '';
  public statusColor = '#00f3ff';
  public isNominal = true;

  private config = [
    { 
      titleKey: 'singularity_title_stable', statusKey: 'singularity_status_nominal', 
      morph: 0.1, compress: 1.0, intensity: 1.0, rotate: 0.4, camY: 25, camDist: 85, orbit: 1.0,
      color: '#00f3ff', vel: '0.45c'
    },
    { 
      titleKey: 'singularity_title_turbulence', statusKey: 'singularity_status_fluctuating', 
      morph: 4.5, compress: 1.15, intensity: 1.4, rotate: 1.5, camY: 45, camDist: 95, orbit: 1.8,
      color: '#ffaa00', vel: '0.78c'
    },
    { 
      titleKey: 'singularity_title_collapse', statusKey: 'singularity_status_critical', 
      morph: 0.8, compress: 0.38, intensity: 3.5, rotate: 5.0, camY: 12, camDist: 55, orbit: 4.5,
      color: '#ff0044', vel: '0.99c'
    }
  ];

  ngOnInit(): void {}

  ngAfterViewInit(): void {
    this.initThree();
    this.createSingularity();
    this.setupIntersectionObserver();
    this.animate();
    
    // Start transitions
    setInterval(() => {
      if (this.isVisible) this.transition();
    }, 10000);
  }

  private setupIntersectionObserver(): void {
    this.observer = new IntersectionObserver(([entry]) => {
      this.isVisible = entry.isIntersecting;
    }, { threshold: 0.1 });

    const canvasEl = this.canvas()?.nativeElement;
    if (canvasEl) this.observer.observe(this.el.nativeElement);
  }

  constructor(private el: ElementRef) {}

  private initThree(): void {
    const canvasEl = this.canvas()?.nativeElement;
    if (!canvasEl) return;

    this.scene = new THREE.Scene();
    this.camera = new THREE.PerspectiveCamera(40, canvasEl.clientWidth / canvasEl.clientHeight, 0.1, 1000);
    this.camera.position.set(60, 30, 60);

    this.renderer = new THREE.WebGLRenderer({ 
      canvas: canvasEl,
      antialias: true, 
      powerPreference: "high-performance",
      alpha: true 
    });
    this.renderer.setSize(canvasEl.clientWidth, canvasEl.clientHeight);
    this.renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
    this.renderer.toneMapping = THREE.ACESFilmicToneMapping;
    this.renderer.toneMappingExposure = 1.6;

    this.controls = new OrbitControls(this.camera, this.renderer.domElement);
    this.controls.enableDamping = true;
    this.controls.dampingFactor = 0.03;
    this.controls.autoRotate = true;
    this.controls.autoRotateSpeed = 0.4;
    this.controls.enableZoom = false; // Disable zoom to avoid breaking hero layout
  }

  private createSingularity(): void {
    const coreGroup = new THREE.Group();
    this.scene.add(coreGroup);

    // Black Hole Center
    const bhMat = new THREE.MeshBasicMaterial({ color: 0x000000 });
    const bhGeo = new THREE.SphereGeometry(4, 64, 64);
    coreGroup.add(new THREE.Mesh(bhGeo, bhMat));

    // Aura Glow
    this.auraMat = new THREE.ShaderMaterial({
      uniforms: { uTime: { value: 0 }, uIntensity: { value: 1.0 } },
      vertexShader: `
        varying vec3 vNormal;
        varying vec3 vView;
        void main() {
          vNormal = normalize(normalMatrix * normal);
          vView = normalize(-(modelViewMatrix * vec4(position, 1.0)).xyz);
          gl_Position = projectionMatrix * modelViewMatrix * vec4(position, 1.0);
        }
      `,
      fragmentShader: `
        uniform float uIntensity;
        varying vec3 vNormal;
        varying vec3 vView;
        void main() {
          float rim = pow(1.0 - max(dot(vNormal, vView), 0.0), 4.0);
          gl_FragColor = vec4(vec3(1.0, 0.45, 0.1) * rim * uIntensity * 5.0, 1.0);
        }
      `,
      side: THREE.BackSide, 
      transparent: true, 
      blending: THREE.AdditiveBlending
    });
    coreGroup.add(new THREE.Mesh(new THREE.SphereGeometry(4.25, 64, 64), this.auraMat));

    // Accretion Disk
    const instanceCount = 5000;
    const streakGeo = new THREE.CylinderGeometry(0.01, 0.12, 2.2, 3);
    streakGeo.rotateX(Math.PI / 2);

    const noiseChunk = `
      vec3 mod289(vec3 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
      vec4 mod289(vec4 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
      vec4 permute(vec4 x) { return mod289(((x*34.0)+1.0)*x); }
      vec4 taylorInvSqrt(vec4 r) { return 1.79284291400159 - 0.85373472095314 * r; }
      float snoise(vec3 v) {
        const vec2 C = vec2(1.0/6.0, 1.0/3.0);
        const vec4 D = vec4(0.0, 0.5, 1.0, 2.0);
        vec3 i = floor(v + dot(v, C.yyy));
        vec3 x0 = v - i + dot(i, C.xxx);
        vec3 g = step(x0.yzx, x0.xyz);
        vec3 l = 1.0 - g;
        vec3 i1 = min(g.xyz, l.zxy);
        vec3 i2 = max(g.xyz, l.zxy);
        vec3 x1 = x0 - i1 + C.xxx;
        vec3 x2 = x0 - i2 + C.yyy;
        vec3 x3 = x0 - D.yyy;
        i = mod289(i);
        vec4 p = permute(permute(permute(i.z + vec4(0.0, i1.z, i2.z, 1.0)) + i.y + vec4(0.0, i1.y, i2.y, 1.0)) + i.x + vec4(0.0, i1.x, i2.x, 1.0));
        float n_ = 0.142857142857;
        vec3 ns = n_ * D.wyz - D.xzx;
        vec4 j = p - 49.0 * floor(p * ns.z * ns.z);
        vec4 x_ = floor(j * ns.z);
        vec4 y_ = floor(j - 7.0 * x_);
        vec4 x = x_ *ns.x + ns.yyyy;
        vec4 y = y_ *ns.x + ns.yyyy;
        vec4 h = 1.0 - abs(x) - abs(y);
        vec4 b0 = vec4(x.xy, y.xy);
        vec4 b1 = vec4(x.zw, y.zw);
        vec4 s0 = floor(b0)*2.0 + 1.0;
        vec4 s1 = floor(b1)*2.0 + 1.0;
        vec4 sh = -step(h, vec4(0.0));
        vec4 a0 = b0.xzyw + s0.xzyw*sh.xxyy;
        vec4 a1 = b1.xzyw + s1.xzyw*sh.zzww;
        vec3 p0 = vec3(a0.xy,h.x);
        vec3 p1 = vec3(a0.zw,h.y);
        vec3 p2 = vec3(a1.xy,h.z);
        vec3 p3 = vec3(a1.zw,h.w);
        vec4 norm = taylorInvSqrt(vec4(dot(p0,p0), dot(p1,p1), dot(p2,p2), dot(p3,p3)));
        p0 *= norm.x; p1 *= norm.y; p2 *= norm.z; p3 *= norm.w;
        vec4 m = max(0.6 - vec4(dot(x0,x0), dot(x1,x1), dot(x2,x2), dot(x3,x3)), 0.0);
        m = m * m;
        return 42.0 * dot(m*m, vec4(dot(p0,x0), dot(p1,x1), dot(p2,x2), dot(p3,x3)));
      }
    `;

    this.diskMaterial = new THREE.ShaderMaterial({
      uniforms: {
        uTime: { value: 0 },
        uMorph: { value: 0.1 },
        uCompression: { value: 1.0 },
        uIntensity: { value: 1.0 },
        uOrbitScale: { value: 1.0 }
      },
      vertexShader: `
        ${noiseChunk}
        uniform float uTime;
        uniform float uMorph;
        uniform float uCompression;
        uniform float uIntensity;
        uniform float uOrbitScale;
        varying vec3 vColor;
        varying float vOpacity;
        void main() {
          vec4 instPos = instanceMatrix * vec4(0.0, 0.0, 0.0, 1.0);
          float rOriginal = length(instPos.xz);
          float r = rOriginal * uCompression;
          float initialAngle = atan(instPos.z, instPos.x);
          float orbitalVelocity = (1.5 / sqrt(rOriginal)) * uOrbitScale;
          float currentAngle = initialAngle + (uTime * orbitalVelocity);
          vec3 morphedWorldPos = vec3(cos(currentAngle) * r, instPos.y, sin(currentAngle) * r);
          float noise = snoise(vec3(morphedWorldPos.x * 0.08, morphedWorldPos.z * 0.08, uTime * 0.3));
          morphedWorldPos.y += noise * uMorph * 4.0;
          vec3 viewDir = normalize(cameraPosition - morphedWorldPos);
          vec3 orbitDir = normalize(vec3(-sin(currentAngle), 0.0, cos(currentAngle)));
          float doppler = dot(orbitDir, viewDir);
          vec3 hot = vec3(1.0, 0.95, 0.9);
          vec3 warm = vec3(1.0, 0.45, 0.1);
          vec3 cool = vec3(0.1, 0.35, 1.0);
          vec3 color = mix(cool, warm, smoothstep(45.0, 12.0, r));
          color = mix(color, hot, smoothstep(10.0, 4.0, r));
          vColor = color * (1.3 + doppler * 0.7) * uIntensity;
          vOpacity = (smoothstep(3.8, 5.5, r) * (1.0 - smoothstep(38.0, 48.0, r))) * 0.8;
          float deltaAngle = currentAngle - initialAngle;
          float c = cos(deltaAngle);
          float s = sin(deltaAngle);
          mat3 rotY = mat3(c, 0, s, 0, 1, 0, -s, 0, c);
          vec3 localPos = (instanceMatrix * vec4(position, 0.0)).xyz;
          vec3 rotatedLocalPos = rotY * localPos;
          gl_Position = projectionMatrix * viewMatrix * vec4(morphedWorldPos + rotatedLocalPos, 1.0);
        }
      `,
      fragmentShader: `
        varying vec3 vColor;
        varying float vOpacity;
        void main() {
          gl_FragColor = vec4(vColor, vOpacity);
        }
      `,
      transparent: true, 
      blending: THREE.AdditiveBlending, 
      depthWrite: false
    });

    this.instancedDisk = new THREE.InstancedMesh(streakGeo, this.diskMaterial, instanceCount);
    const dummy = new THREE.Object3D();

    for (let i = 0; i < instanceCount; i++) {
        const r = 5 + Math.pow(Math.random(), 1.3) * 40;
        const angle = Math.random() * Math.PI * 2;
        dummy.position.set(Math.cos(angle) * r, (Math.random() - 0.5) * (8 / r), Math.sin(angle) * r);
        dummy.lookAt(dummy.position.x + Math.sin(angle), dummy.position.y, dummy.position.z - Math.cos(angle));
        dummy.updateMatrix();
        this.instancedDisk.setMatrixAt(i, dummy.matrix);
    }
    this.scene.add(this.instancedDisk);
  }

  private transition(): void {
    this.stateIdx = (this.stateIdx + 1) % this.config.length;
    const s = this.config[this.stateIdx];
    
    gsap.to(this.diskMaterial.uniforms['uMorph'], { value: s.morph, duration: 4, ease: 'power2.inOut' });
    gsap.to(this.diskMaterial.uniforms['uCompression'], { value: s.compress, duration: 4, ease: 'power2.inOut' });
    gsap.to(this.diskMaterial.uniforms['uIntensity'], { value: s.intensity, duration: 4, ease: 'power2.inOut' });
    gsap.to(this.diskMaterial.uniforms['uOrbitScale'], { value: s.orbit, duration: 4, ease: 'power2.inOut' });
    gsap.to(this.auraMat.uniforms['uIntensity'], { value: s.intensity, duration: 4, ease: 'power2.inOut' });
    gsap.to(this.controls, { autoRotateSpeed: s.rotate, duration: 4, ease: 'power2.inOut' });
    gsap.to(this.camera.position, { y: s.camY, duration: 4, ease: 'power2.inOut' });
    gsap.to(this.camControl, { distance: s.camDist, duration: 4, ease: 'power2.inOut' });

    // Update HUD
    this.currentTitleKey = s.titleKey;
    this.currentStatusKey = s.statusKey;
    this.currentVel = s.vel;
    this.statusColor = s.color;
    this.isNominal = s.statusKey.includes('nominal');
  }

  private animate(): void {
    this.ngZone.runOutsideAngular(() => {
      const render = () => {
        if (!this.isVisible) {
          requestAnimationFrame(render);
          return;
        }
        const time = this.clock.getElapsedTime();
        if (this.diskMaterial) this.diskMaterial.uniforms['uTime'].value = time;
        if (this.auraMat) this.auraMat.uniforms['uTime'].value = time;
        if (this.instancedDisk) this.instancedDisk.rotation.y += 0.0005;

        const currentDir = new THREE.Vector3().subVectors(this.camera.position, this.controls.target).normalize();
        this.camera.position.x = this.controls.target.x + currentDir.x * this.camControl.distance;
        this.camera.position.z = this.controls.target.z + currentDir.z * this.camControl.distance;

        this.controls.update();
        this.renderer.render(this.scene, this.camera);
        requestAnimationFrame(render);
      };
      render();
    });
  }

  ngOnDestroy(): void {
    if (this.observer) this.observer.disconnect();
    if (this.renderer) {
      this.renderer.dispose();
    }
  }
}
