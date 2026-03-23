const fs = require('fs');
const path = require('path');

const scopes = [
  'dashboard',
  'medical-records',
  'medication-manager',
  'ai-assistant',
  'access-control'
];

scopes.forEach(scope => {
  const dirPath = path.join(__dirname, 'public', 'assets', 'i18n', scope);
  if (!fs.existsSync(dirPath)) {
    fs.mkdirSync(dirPath, { recursive: true });
  }
  
  const files = ['en-US.json', 'ar-SA.json'];
  files.forEach(file => {
    const filePath = path.join(dirPath, file);
    if (!fs.existsSync(filePath)) {
      fs.writeFileSync(filePath, '{}');
    }
  });
});

console.log('Scopes generated.');
