import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { MenuModule } from 'primeng/menu';

@Component({
  selector: 'app-family-profiles',
  imports: [CommonModule, TranslocoModule, ButtonModule, MenuModule],
  templateUrl: './family-profiles.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    class: 'block w-full'
  }
})
export class FamilyProfilesComponent {
  primaryUser = {
    name: 'Sarah Johnson',
    roleKey: 'family-profiles.roles.parent',
    age: 38,
    image: 'https://lh3.googleusercontent.com/aida-public/AB6AXuAWkodz9uviUuEY8l7yWfv6wmDy7uX3yCZbYXbEM_4jwIkrdmpcus5DMGI5P65SIy12er37ExV18ZiASbDXGcMnMCVpaTreQ11JTvhya7xjNAbs5PmevjT5L9XB0eBxKJ_Ly3MJOpk1-JbLdeKgfLTnzMs-J7T5d9tFPHtgswPZ86ZUFWfdih2zNXVYM6ieS7U6k-u9-XCxbwNI-pBIy-zlfHZ6mNfVs5W6qjbC5z_qGEYPhQWHkov9Qg9B4XrQ8BnRbjqDGKhrbjGq'
  };

  familyMembers = [
    {
      name: 'Leo Johnson',
      relationKey: 'family-profiles.roles.son',
      age: 8,
      bloodType: 'O+',
      accessKey: 'family-profiles.access.full_access',
      accessIcon: 'shield_with_heart',
      accessClass: 'bg-secondary-fixed text-on-secondary-fixed-variant',
      image: 'https://lh3.googleusercontent.com/aida-public/AB6AXuCXbqjVetsmIBNw5ugAqi1_GFqa9X4WVjX-yNRTtaAZLL9fHnIycsiOHW4ftDjcxKlk1TLE9f4zp1KbCr8JmW5QHH4JMavR9p_Ze6QW0x6D0S_0hQZDmaF1F7pApQuosuEhJt5CPmtX9Z1RtHn8Rs0QEQi6iEjavbQjCD-izbgx97IhsHkjdlMeCpQQp33b626NXlmz7h_hU4MNffYoJ6m3cHrrx1lQVhby5chZZOpYxknkyoCv5FuhaNq4YvHkqh1RgVeDS7lxLYIr',
      actions: [
        { icon: 'folder_shared', title: 'Health Records' },
        { icon: 'vaccines', title: 'Immunization' }
      ]
    },
    {
      name: 'Maya Johnson',
      relationKey: 'family-profiles.roles.daughter',
      age: 14,
      bloodType: 'A-',
      accessKey: 'family-profiles.access.view_only',
      accessIcon: 'visibility',
      accessClass: 'bg-surface-container-highest text-on-surface-variant',
      image: 'https://lh3.googleusercontent.com/aida-public/AB6AXuC9sM00rZdk9hL7tfl3YMmNuy_drO8hrCd9mW3sHVbo_u4ui2uOcH0p9Vn9ReoYc1J9iieKjo48qFJOmdhm1_Wy813AWXGQ0EvlwdBpGHO65J2heASmsaR8i05viYuuRk68ndOS2QtABtNHK8gVqyHPGhoHvXzsMabvBsdF3zk4d9B1PdAHr5wwSCte3ssNkvDv1ugrZDOf5ceFx57GT4-xvaenlSN3VJNrL6w9PsCtVo_vbVnvNceceHnLrQPmHwyTkF-1241WZBJa',
      actions: [
        { icon: 'folder_shared', title: 'Health Records' }
      ]
    },
    {
      name: 'Henry Smith',
      relationKey: 'family-profiles.roles.father',
      age: 72,
      bloodType: 'O+',
      accessKey: 'family-profiles.access.full_access',
      accessIcon: 'shield_with_heart',
      accessClass: 'bg-secondary-fixed text-on-secondary-fixed-variant',
      image: 'https://lh3.googleusercontent.com/aida-public/AB6AXuAYy3zPanTv5k3UsHdsMOM2ji8XVYlAXtJGU6yhFWfw0i_k_-Rb48N4ySFHoJgzZplwdCydOrOEHLtaBzPmBU039XdrupybJ298MZL-znOZhQXPnps4NVgbrEl6ijUavNu9gUQtvZGl8LmtN3Rooesg_qfXEJP4_xsNgeK8iO_8rUhoUmNE82GYYp0t9hK_dBe3nXth2AvOZHaBPea9iuPPXYt71ubpdo13uJ_fJiFxwSHAp0bS2Q7PJ1mL4D1BVVjSZHONNuFmzyoP',
      actions: [
        { icon: 'folder_shared', title: 'Health Records' },
        { icon: 'medication', title: 'Medications' }
      ]
    }
  ];
}
