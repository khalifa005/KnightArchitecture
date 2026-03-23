const fs = require('fs');

function fix(file, hyphen, camel) {
    let html = fs.readFileSync(file, 'utf8');
    html = html.replace(new RegExp(hyphen + '\\.', 'g'), camel + '.');
    fs.writeFileSync(file, html);
    console.log('Fixed ' + file);
}

fix('src/app/features/medical-records/medical-records.html', 'medical-records', 'medicalRecords');
fix('src/app/features/medication-manager/medication-manager.html', 'medication-manager', 'medicationManager');
fix('src/app/features/ai-assistant/ai-assistant.html', 'ai-assistant', 'aiAssistant');
fix('src/app/features/access-control/access-control.html', 'access-control', 'accessControl');
