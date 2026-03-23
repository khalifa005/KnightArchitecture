const fs = require('fs');
const path = require('path');

const htmlFiles = [
  'src/app/core/layout/layout.component.html',
  'src/app/features/test-page/test-page.component.html',
  'src/app/features/access-control/access-control.html',
  'src/app/features/ai-assistant/ai-assistant.html',
  'src/app/features/medication-manager/medication-manager.html',
  'src/app/features/medical-records/medical-records.html'
];
const tsFiles = [
  'src/app/core/layout/layout.component.ts',
  'src/app/features/test-page/test-page.component.ts',
  'src/app/features/access-control/access-control.ts',
  'src/app/features/ai-assistant/ai-assistant.ts',
  'src/app/features/medication-manager/medication-manager.ts',
  'src/app/features/medical-records/medical-records.ts'
];

htmlFiles.forEach(f => {
  let content = fs.readFileSync(f, 'utf8');
  content = content.replace(/<button class="/g, '<p-button styleClass="');
  content = content.replace(/<\/button>/g, '</p-button>');
  content = content.replace(/<input class="/g, '<input pInputText class="');
  fs.writeFileSync(f, content);
});

tsFiles.forEach(f => {
  let content = fs.readFileSync(f, 'utf8');
  let importsToAdd = [];
  if (content.indexOf('ButtonModule') === -1) {
    importsToAdd.push("import { ButtonModule } from 'primeng/button';");
  }
  if (content.indexOf('InputTextModule') === -1) {
    importsToAdd.push("import { InputTextModule } from 'primeng/inputtext';");
  }
  if (importsToAdd.length > 0) {
    content = importsToAdd.join('\n') + '\n' + content;
  }
  // Inject into imports array
  content = content.replace(/imports:\s*\[([^\]]*)\]/, (match, p1) => {
    let currentImports = p1.trim();
    let additional = ['ButtonModule', 'InputTextModule'];
    let finalImports = currentImports;
    additional.forEach(item => {
      if (!currentImports.includes(item)) {
        finalImports += (finalImports.length > 0 && !finalImports.endsWith(',')) ? ', ' + item : item;
      }
    });
    return 'imports: [' + finalImports + ']';
  });
  
  fs.writeFileSync(f, content);
});

console.log('Conversion Complete!');
