const fs = require('fs');
['medical-records/medical-records.ts', 'medication-manager/medication-manager.ts', 'ai-assistant/ai-assistant.ts', 'access-control/access-control.ts'].forEach(f => {
    let p = 'src/app/features/' + f;
    let t = fs.readFileSync(p, 'utf8');
    if (!t.includes('standalone: true')) {
        t = t.replace('@Component({', '@Component({\n  standalone: true,');
        fs.writeFileSync(p, t);
    }
});
console.log('Fixed standalone flags for all feature routes.');
