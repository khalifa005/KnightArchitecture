const fs = require('fs');

const files = [
  'src/app/core/layout/layout.component.html',
  'src/app/features/test-page/test-page.component.html',
  'src/app/features/access-control/access-control.html',
  'src/app/features/ai-assistant/ai-assistant.html',
  'src/app/features/medication-manager/medication-manager.html',
  'src/app/features/medical-records/medical-records.html'
];

function processContent(content) {
  content = content.replace(/\bml-/g, 'ms-');
  content = content.replace(/\bmr-/g, 'me-');
  content = content.replace(/\b-ml-/g, '-ms-');
  content = content.replace(/\b-mr-/g, '-me-');
  
  content = content.replace(/\bpr-/g, 'pe-');
  content = content.replace(/\bpl-/g, 'ps-');
  
  content = content.replace(/\bleft-/g, 'start-');
  content = content.replace(/\bright-/g, 'end-');
  
  content = content.replace(/\bborder-l-/g, 'border-s-');
  content = content.replace(/\bborder-r-/g, 'border-e-');
  content = content.replace(/\bborder-l\b/g, 'border-s');
  content = content.replace(/\bborder-r\b/g, 'border-e');

  content = content.replace(/\brounded-l-/g, 'rounded-s-');
  content = content.replace(/\brounded-r-/g, 'rounded-e-');
  content = content.replace(/\brounded-l\b/g, 'rounded-s');
  content = content.replace(/\brounded-r\b/g, 'rounded-e');

  content = content.replace(/\btext-left\b/g, 'text-start');
  content = content.replace(/\btext-right\b/g, 'text-end');
  
  return content;
}

files.forEach(f => {
  if (fs.existsSync(f)) {
    let content = fs.readFileSync(f, 'utf8');
    fs.writeFileSync(f, processContent(content));
  }
});
console.log('RTL conversion complete');
