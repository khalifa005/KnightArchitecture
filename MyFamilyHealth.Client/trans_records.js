const fs = require('fs');

const en = {
  "title": "Medical Records",
  "subtitle": "Secure access to your clinical history, laboratory results, and diagnostic imaging in one editorial sanctuary.",
  "search_placeholder": "Search records, doctors, or tests...",
  "date_range": "Date Range",
  "type_all": "Type: All",
  "provider": "Provider",
  "cmp": "Comprehensive Metabolic Panel",
  "cmp_date": "October 24, 2023 • LabCorp Diagnostics",
  "normal": "Normal",
  "dr_helena": "Dr. Helena Vance • General Practitioner",
  "view_report": "View Report",
  "ai_interpretation": "AI Interpretation",
  "lumbar_mri": "Lumbar Spine MRI",
  "mri_date": "September 12, 2023 • City Imaging Center",
  "pending_review": "Pending Review",
  "dr_marcus": "Dr. Marcus Thorne • Orthopedic Surgeon",
  "open_dicom": "Open DICOM Viewer",
  "ecg": "Resting Electrocardiogram",
  "ecg_date": "August 05, 2023 • Cardiology Associates",
  "dr_sarah": "Dr. Sarah Chen • Cardiologist",
  "pdf_report": "PDF Report",
  "vit_d": "Vitamin D, 25-Hydroxy",
  "vit_d_date": "July 18, 2023 • LabCorp Diagnostics",
  "low_range": "Low Range",
  "view_details": "View Details",
  "showing_records": "Showing 4 of 28 medical records",
  "load_archive": "Load Archive History",
  "ai_upsell_title": "Understand your health like never before",
  "ai_upsell_desc": "Our Clinical AI breaks down complex medical jargon into easy-to-understand summaries. Tap 'AI Interpretation' on any record to begin.",
  "upgrade_tier": "Upgrade Insight Tier"
};

const ar = {
  "title": "السجلات الطبية",
  "subtitle": "الوصول الآمن إلى تاريخك السريري ونتائج المختبر والتصوير التشخيصي في مكان واحد.",
  "search_placeholder": "ابحث عن السجلات أو الأطباء أو التحاليل...",
  "date_range": "نطاق التاريخ",
  "type_all": "النوع: الكل",
  "provider": "مقدم الرعاية",
  "cmp": "فحص الأيض الشامل",
  "cmp_date": "24 أكتوبر 2023 • مختبرات لابكورب",
  "normal": "طبيعي",
  "dr_helena": "د. هيلينا فانس • طبيب عام",
  "view_report": "عرض التقرير",
  "ai_interpretation": "تفسير الذكاء الاصطناعي",
  "lumbar_mri": "الرنين المغناطيسي للعمود الفقري",
  "mri_date": "12 سبتمبر 2023 • مركز تصوير المدينة",
  "pending_review": "قيد المراجعة",
  "dr_marcus": "د. ماركوس ثورن • جراح عظام",
  "open_dicom": "فتح عارض DICOM",
  "ecg": "تخطيط القلب الكهربائي أثناء الراحة",
  "ecg_date": "05 أغسطس 2023 • عيادات أمراض القلب",
  "dr_sarah": "د. سارة تشين • طبيبة قلب",
  "pdf_report": "تقرير PDF",
  "vit_d": "فيتامين د، 25-هيدروكسي",
  "vit_d_date": "18 يوليو 2023 • مختبرات لابكورب",
  "low_range": "نطاق منخفض",
  "view_details": "عرض التفاصيل",
  "showing_records": "عرض 4 من 28 سجلاً طبياً",
  "load_archive": "تنزيل سجلات الأرشيف",
  "ai_upsell_title": "افهم صحتك كن متألقاً كما لم تفعل من قبل",
  "ai_upsell_desc": "يقوم الذكاء الاصطناعي الطبي الخاص بنا بتبسيط المصطلحات الطبية المعقدة إلى ملخصات سهلة الفهم. انقر فوق 'تفسير الذكاء الاصطناعي' على أي ملف للبدء.",
  "upgrade_tier": "ترقية باقة الرؤى"
};

fs.writeFileSync('public/assets/i18n/medical-records/en-US.json', JSON.stringify(en, null, 2));
fs.writeFileSync('public/assets/i18n/medical-records/ar-SA.json', JSON.stringify(ar, null, 2));

let html = fs.readFileSync('src/app/features/medical-records/medical-records.html', 'utf8');

const replacements = {
  "Medical Records": "title",
  "Secure access to your clinical history, laboratory results, and diagnostic imaging in one editorial sanctuary.": "subtitle",
  "Date Range": "date_range",
  "Type: All": "type_all",
  "Provider": "provider",
  "Comprehensive Metabolic Panel": "cmp",
  "October 24, 2023 • LabCorp Diagnostics": "cmp_date",
  "Normal": "normal",
  "Dr. Helena Vance • General Practitioner": "dr_helena",
  "View Report": "view_report",
  "AI Interpretation": "ai_interpretation",
  "Lumbar Spine MRI": "lumbar_mri",
  "September 12, 2023 • City Imaging Center": "mri_date",
  "Pending Review": "pending_review",
  "Dr. Marcus Thorne • Orthopedic Surgeon": "dr_marcus",
  "Open DICOM Viewer": "open_dicom",
  "Resting Electrocardiogram": "ecg",
  "August 05, 2023 • Cardiology Associates": "ecg_date",
  "Dr. Sarah Chen • Cardiologist": "dr_sarah",
  "PDF Report": "pdf_report",
  "Vitamin D, 25-Hydroxy": "vit_d",
  "July 18, 2023 • LabCorp Diagnostics": "vit_d_date",
  "Low Range": "low_range",
  "View Details": "view_details",
  "Showing 4 of 28 medical records": "showing_records",
  "Load Archive History": "load_archive",
  "Understand your health like never before": "ai_upsell_title",
  "Our Clinical AI breaks down complex medical jargon into easy-to-understand summaries. Tap 'AI Interpretation' on any record to begin.": "ai_upsell_desc",
  "Upgrade Insight Tier": "upgrade_tier"
};

for (const [text, key] of Object.entries(replacements)) {
  const regex = new RegExp(`>\\s*${text}\\s*<`, 'g');
  if (html.match(regex)) {
    html = html.replace(regex, `>{{ 'medical-records.${key}' | transloco }}<`);
  } else {
    // Escape specific chars for plain replace
    const escapedText = text.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
    html = html.replace(new RegExp(escapedText, 'g'), `{{ 'medical-records.${key}' | transloco }}`);
  }
}

// Special case for placeholder
html = html.replace(/placeholder="Search records, doctors, or tests..."/g, `[placeholder]="'medical-records.search_placeholder' | transloco"`);

fs.writeFileSync('src/app/features/medical-records/medical-records.html', html);

let ts = fs.readFileSync('src/app/features/medical-records/medical-records.ts', 'utf8');
if (!ts.includes('TranslocoModule')) {
    ts = ts.replace(/imports: \[/, 'imports: [TranslocoModule, ');
    ts = `import { TranslocoModule } from '@jsverse/transloco';\n${ts}`;
    fs.writeFileSync('src/app/features/medical-records/medical-records.ts', ts);
}

console.log('Medical Records translation applied');
