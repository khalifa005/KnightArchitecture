const fs = require('fs');

const files = [
  'src/app/core/layout/layout.component.html',
  'src/app/features/test-page/test-page.component.html',
  'src/app/features/access-control/access-control.html',
  'src/app/features/ai-assistant/ai-assistant.html',
  'src/app/features/medication-manager/medication-manager.html',
  'src/app/features/medical-records/medical-records.html'
];

files.forEach(f => {
  let content = fs.readFileSync(f, 'utf8');
  content = content.replace(/<p-button styleClass="/g, '<button pButton class="');
  content = content.replace(/<\/p-button>/g, '</button>');
  fs.writeFileSync(f, content);
});

console.log('Fixed p-button wrappers');
