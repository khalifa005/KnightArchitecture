const fs = require('fs');

const en = {
  "title": "AI Sanctuary Assistant",
  "subtitle": "Your secure, clinical AI companion for health inquiries.",
  "today_date": "Today, October 24",
  "ai_msg_1": "Hello. I am your Clinical Assistant. I have reviewed your recent blood work from yesterday's appointment with Dr. Miller. Would you like me to summarize the results or help you understand the recommended dosage for your new prescription?",
  "user_msg_1": "Can you explain my Vitamin D levels? They seemed highlighted in the report.",
  "timestamp_1": "10:42 AM • Read",
  "ai_msg_2_part1": "Of course. Your Vitamin D (25-Hydroxy) level was measured at ",
  "ai_msg_2_value": "18 ng/mL",
  "ai_msg_2_part2": ". ",
  "clinical_context": "Clinical Context",
  "educational_info": "Educational Info",
  "ai_msg_3_part1": "Optimal levels are typically between 30 and 100 ng/mL. A result of 18 ng/mL is classified as a ",
  "deficiency": "deficiency",
  "ai_msg_3_part2": ". This is very common and Dr. Miller has already added a supplement to your plan.",
  "note": "Note: This information is based on your lab results from LabCorp, dated Oct 23, 2023.",
  "vit_d3": "Vitamin D3 2000IU",
  "daily_supplement": "Daily oral supplement prescribed",
  "view_instructions": "View Instructions",
  "reschedule": "Reschedule Appointment",
  "analyze_test": "Analyze latest test",
  "message_dr": "Message Dr. Miller",
  "placeholder": "Ask about test results, dosages, or appointments...",
  "disclaimer": "AI-generated health insights. Please consult with your provider for clinical decisions."
};

const ar = {
  "title": "مساعد الذكاء الاصطناعي",
  "subtitle": "رفيقك الآمن والموثوق للاستفسارات الصحية.",
  "today_date": "اليوم، 24 أكتوبر",
  "ai_msg_1": "مرحباً. أنا مساعدك السريري. لقد راجعت تحاليل الدم الأخيرة من موعدك بالأمس مع د. ميلر. هل ترغب في أن ألخص لك النتائج أو أساعدك في فهم الجرعة الموصى بها لوصفتك الجديدة؟",
  "user_msg_1": "هل يمكنك شرح مستويات فيتامين د لدي؟ بدت مميزة في التقرير.",
  "timestamp_1": "10:42 صباحاً • مقروء",
  "ai_msg_2_part1": "بالتأكيد. تم قياس مستوى فيتامين د (25-هيدروكسي) لديك عند ",
  "ai_msg_2_value": "18 نانوغرام/مل",
  "ai_msg_2_part2": ". ",
  "clinical_context": "السياق السريري",
  "educational_info": "معلومات تثقيفية",
  "ai_msg_3_part1": "المستويات المثالية تتراوح عادة بين 30 و 100 نانوغرام/مل. يُصنف نتيجة 18 نانوغرام/مل على أنها ",
  "deficiency": "نقص",
  "ai_msg_3_part2": ". هذا أمر شائع جداً وقد أضاف د. ميلر بالفعل مكملاً غذائياً إلى خطتك.",
  "note": "ملاحظة: تستند هذه المعلومات إلى نتائج الفحص من مختبرات لابكورب، بتاريخ 23 أكتوبر 2023.",
  "vit_d3": "فيتامين د3 2000 وحدة دولية",
  "daily_supplement": "مكمل غذائي عن طريق الفم يوصف يومياً",
  "view_instructions": "عرض التعليمات",
  "reschedule": "إعادة جدولة الموعد",
  "analyze_test": "تحليل أحدث فحص",
  "message_dr": "مراسلة د. ميلر",
  "placeholder": "اسأل عن نتائج الفحوصات، الجرعات، أو المواعيد...",
  "disclaimer": "رؤى صحية مولدة بالذكاء الاصطناعي. يرجى استشارة طبيبك لاتخاذ القرارات السريرية."
};

fs.writeFileSync('public/assets/i18n/ai-assistant/en-US.json', JSON.stringify(en, null, 2));
fs.writeFileSync('public/assets/i18n/ai-assistant/ar-SA.json', JSON.stringify(ar, null, 2));

let html = fs.readFileSync('src/app/features/ai-assistant/ai-assistant.html', 'utf8');

const replacements = {
  "AI Sanctuary Assistant": "title",
  "Your secure, clinical AI companion for health inquiries.": "subtitle",
  "Today, October 24": "today_date",
  "Hello. I am your Clinical Assistant. I have reviewed your recent blood work from yesterday's appointment with Dr. Miller. Would you like me to summarize the results or help you understand the recommended dosage for your new prescription\\?": "ai_msg_1",
  "Can you explain my Vitamin D levels\\? They seemed highlighted in the report.": "user_msg_1",
  "10:42 AM • Read": "timestamp_1",
  "Of course. Your Vitamin D \\(25-Hydroxy\\) level was measured at ": "ai_msg_2_part1",
  "18 ng/mL": "ai_msg_2_value",
  "\\. ": "ai_msg_2_part2",
  "Clinical Context": "clinical_context",
  "Educational Info": "educational_info",
  "Optimal levels are typically between 30 and 100 ng/mL. A result of 18 ng/mL is classified as a ": "ai_msg_3_part1",
  "deficiency": "deficiency",
  "\\. This is very common and Dr. Miller has already added a supplement to your plan.": "ai_msg_3_part2",
  "Note: This information is based on your lab results from LabCorp, dated Oct 23, 2023.": "note",
  "Vitamin D3 2000IU": "vit_d3",
  "Daily oral supplement prescribed": "daily_supplement",
  "View Instructions": "view_instructions",
  "Reschedule Appointment": "reschedule",
  "Analyze latest test": "analyze_test",
  "Message Dr. Miller": "message_dr",
  "AI-generated health insights. Please consult with your provider for clinical decisions.": "disclaimer"
};

for (const [text, key] of Object.entries(replacements)) {
  const isRegexSafe = text.replace(/\\/g, ''); // Unescape for HTML testing check
  
  if (html.includes(isRegexSafe) || html.match(new RegExp(`>\\s*${text}\\s*<`, 'g'))) {
     let regex = new RegExp(`>\\s*${text}\\s*<`, 'g');
     if (html.match(regex)) {
        html = html.replace(regex, `>{{ 'ai-assistant.${key}' | transloco }}<`);
     } else {
        html = html.replace(new RegExp(text, 'g'), `{{ 'ai-assistant.${key}' | transloco }}`);
     }
  }
}

// Special cases that might over-replace or need direct targeting due to spacing in tags:
html = html.replace(/placeholder="Ask about test results, dosages, or appointments..."/g, `[placeholder]="'ai-assistant.placeholder' | transloco"`);

fs.writeFileSync('src/app/features/ai-assistant/ai-assistant.html', html);

let ts = fs.readFileSync('src/app/features/ai-assistant/ai-assistant.ts', 'utf8');
if (!ts.includes('TranslocoModule')) {
    ts = ts.replace(/imports: \[/, 'imports: [TranslocoModule, ');
    ts = `import { TranslocoModule } from '@jsverse/transloco';\n${ts}`;
    fs.writeFileSync('src/app/features/ai-assistant/ai-assistant.ts', ts);
}

console.log('AI Assistant translation applied');
