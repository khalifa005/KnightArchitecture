const fs = require('fs');

const en = {
  "title": "Medication Management",
  "subtitle": "Track your prescribed regimen, set intelligent alerts, and keep your care network informed in real-time.",
  "daily_schedule": "Daily Schedule",
  "today_date": "Today, Oct 24",
  "time_8am": "08:00 AM",
  "lisinopril_10mg": "Lisinopril • 10mg",
  "taken": "Taken",
  "time_12pm": "12:30 PM",
  "metformin_500mg": "Metformin • 500mg",
  "due_15m": "Due in 15 mins",
  "time_8pm": "08:00 PM",
  "atorvastatin_20mg": "Atorvastatin • 20mg",
  "scheduled": "Scheduled",
  "caregiver": "Caregiver Status",
  "realtime_sync": "Real-time sync with Primary Contact",
  "parent_notified": "Parent Notified",
  "last_sync": "Last sync: 2 mins ago",
  "current_rx": "Current Prescriptions",
  "add_new": "Add New",
  "metformin": "Metformin",
  "interaction": "Potential Interaction",
  "metformin_desc": "Oral tablet • 500mg • Twice daily",
  "mark_taken": "Mark as Taken",
  "alert_title": "Interaction Alert with Lisinopril",
  "alert_desc": "May increase the risk of lactic acidosis. Consult your healthcare provider if you experience muscle pain or weakness.",
  "lisinopril": "Lisinopril",
  "no_conflicts": "No Conflicts",
  "lisinopril_desc": "Oral tablet • 10mg • Once daily (Morning)",
  "taken_814": "Taken at 08:14 AM",
  "atorvastatin": "Atorvastatin",
  "days_left_3": "3 Days Left",
  "atorvastatin_desc": "Capsule • 20mg • Once daily (Night)",
  "request_refill": "Request Refill",
  "smart_reminders": "Smart Reminders",
  "reminders_desc": "Predictive alerts based on your activity and heart rate trends.",
  "active": "Active",
  "adherence": "Adherence Overview",
  "adherence_desc": "You have maintained a 98% adherence rate over the last 30 days. Your care team has been notified of your progress.",
  "day_streak": "Day Streak",
  "missed_doses": "Missed Doses"
};

const ar = {
  "title": "إدارة الأدوية",
  "subtitle": "تتبع خطتك العلاجية، واضبط تنبيهات ذكية، وابق على تواصل مع شبكة رعايتك الصحية في الوقت الفعلي.",
  "daily_schedule": "الجدول اليومي",
  "today_date": "اليوم، 24 أكتوبر",
  "time_8am": "08:00 صباحاً",
  "lisinopril_10mg": "ليزينوبريل • 10ملغ",
  "taken": "تم تناوله",
  "time_12pm": "12:30 مساءً",
  "metformin_500mg": "ميتفورمين • 500ملغ",
  "due_15m": "مستحق خلال 15 دقيقة",
  "time_8pm": "08:00 مساءً",
  "atorvastatin_20mg": "أتورفاستاتين • 20ملغ",
  "scheduled": "أدوية مجدولة",
  "caregiver": "حالة فريق الرعاية",
  "realtime_sync": "مزامنة فورية مع المرافق الأساسي",
  "parent_notified": "تم إشعار الوالدين",
  "last_sync": "آخر مزامنة: قبل دقيقتين",
  "current_rx": "الوصفات الطبية الحالية",
  "add_new": "إضافة جديد",
  "metformin": "ميتفورمين",
  "interaction": "تفاعل محتمل",
  "metformin_desc": "قرص فموي • 500ملغ • مرتين يومياً",
  "mark_taken": "تحديد كمتناول",
  "alert_title": "تنبيه تفاعل دوائي مع ليزينوبريل",
  "alert_desc": "قد يزيد من خطر الحماض اللاكتيكي. استشر مقدم الرعاية الصحية إذا شعرت بألم في العضلات أو ضعف.",
  "lisinopril": "ليزينوبريل",
  "no_conflicts": "لا يوجد تعارض",
  "lisinopril_desc": "قرص فموي • 10ملغ • مرة يومياً (صباحاً)",
  "taken_814": "تُم تناوله في 08:14 صباحاً",
  "atorvastatin": "أتورفاستاتين",
  "days_left_3": "متبقي 3 أيام",
  "atorvastatin_desc": "كبسولة • 20ملغ • مرة يومياً (مساءً)",
  "request_refill": "طلب إعادة تعبئة",
  "smart_reminders": "تنبيهات ذكية",
  "reminders_desc": "تنبيهات استباقية بناءً على نشاطك ومعدل ضربات قلبك.",
  "active": "نشط",
  "adherence": "ملخص الالتزام بالأدوية",
  "adherence_desc": "لقد حافظت على معدل التزام بنسبة 98% خلال آخر 30 يوماً. تم إبلاغ فريق الرعاية الصحية بتقدمك.",
  "day_streak": "أيام متتالية دون انقطاع",
  "missed_doses": "الجرعات الفائتة"
};

fs.writeFileSync('public/assets/i18n/medication-manager/en-US.json', JSON.stringify(en, null, 2));
fs.writeFileSync('public/assets/i18n/medication-manager/ar-SA.json', JSON.stringify(ar, null, 2));

let html = fs.readFileSync('src/app/features/medication-manager/medication-manager.html', 'utf8');

const replacements = {
  "Medication Management": "title",
  "Track your prescribed regimen, set intelligent alerts, and keep your care network informed in real-time.": "subtitle",
  "Daily Schedule": "daily_schedule",
  "Today, Oct 24": "today_date",
  "08:00 AM": "time_8am",
  "Lisinopril • 10mg": "lisinopril_10mg",
  "Taken": "taken",
  "12:30 PM": "time_12pm",
  "Metformin • 500mg": "metformin_500mg",
  "Due in 15 mins": "due_15m",
  "08:00 PM": "time_8pm",
  "Atorvastatin • 20mg": "atorvastatin_20mg",
  "Scheduled": "scheduled",
  "Caregiver Status": "caregiver",
  "Real-time sync with Primary Contact": "realtime_sync",
  "Parent Notified": "parent_notified",
  "Last sync: 2 mins ago": "last_sync",
  "Current Prescriptions": "current_rx",
  "Add New": "add_new",
  "Metformin": "metformin",
  "Potential Interaction": "interaction",
  "Oral tablet • 500mg • Twice daily": "metformin_desc",
  "Mark as Taken": "mark_taken",
  "Interaction Alert with Lisinopril": "alert_title",
  "May increase the risk of lactic acidosis. Consult your healthcare provider if you experience muscle pain or weakness.": "alert_desc",
  "Lisinopril": "lisinopril",
  "No Conflicts": "no_conflicts",
  "Oral tablet • 10mg • Once daily \\(Morning\\)": "lisinopril_desc",
  "Taken at 08:14 AM": "taken_814",
  "Atorvastatin": "atorvastatin",
  "3 Days Left": "days_left_3",
  "Capsule • 20mg • Once daily \\(Night\\)": "atorvastatin_desc",
  "Request Refill": "request_refill",
  "Smart Reminders": "smart_reminders",
  "Predictive alerts based on your activity and heart rate trends.": "reminders_desc",
  "Active": "active",
  "Adherence Overview": "adherence",
  "You have maintained a 98% adherence rate over the last 30 days. Your care team has been notified of your progress.": "adherence_desc",
  "Day Streak": "day_streak",
  "Missed Doses": "missed_doses"
};

for (const [text, key] of Object.entries(replacements)) {
  const regex = new RegExp(`>\\s*${text}\\s*<`, 'g');
  if (html.match(regex)) {
    html = html.replace(regex, `>{{ 'medication-manager.${key}' | transloco }}<`);
  } else {
    html = html.replace(new RegExp(text.replace(/\\/g, ''), 'g'), `{{ 'medication-manager.${key}' | transloco }}`);
  }
}

fs.writeFileSync('src/app/features/medication-manager/medication-manager.html', html);

let ts = fs.readFileSync('src/app/features/medication-manager/medication-manager.ts', 'utf8');
if (!ts.includes('TranslocoModule')) {
    ts = ts.replace(/imports: \[/, 'imports: [TranslocoModule, ');
    ts = `import { TranslocoModule } from '@jsverse/transloco';\n${ts}`;
    fs.writeFileSync('src/app/features/medication-manager/medication-manager.ts', ts);
}

console.log('Medications translation applied');
