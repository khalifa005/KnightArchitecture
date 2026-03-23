const fs = require('fs');

const en = {
  "welcome": "Welcome back, Alex.",
  "status_prefix": "Your health sanctuary is looking stable today. You have",
  "status_highlight": "2 pending actions",
  "insight_tag": "AI Personalized Insight",
  "insight_title": "Focus on your sleep hygiene tonight.",
  "insight_desc": "Based on your glucose levels and heart rate variability from your wearables, we suggest moving your dinner 1 hour earlier. This could improve your recovery score by approximately 14%.",
  "view_analysis": "View Full Analysis",
  "health_score": "Health Score",
  "need_refill": "Need a Refill?",
  "refill_desc": "Your Lisinopril prescription is running low. We can contact Dr. Aris for you.",
  "refill_now": "Refill Now",
  "latest_results": "Latest Test Results",
  "updated_time": "Updated 14 hours ago",
  "view_all": "View All Records",
  "cbc": "Complete Blood Count",
  "cbc_lab": "Clinical Labs West",
  "normal": "Normal",
  "within_range": "Within Range",
  "ecg": "ECG Summary",
  "ecg_dept": "Cardiology Dept.",
  "pending": "Pending",
  "est_2_days": "Est. 2 days",
  "metabolic": "Metabolic Panel",
  "gen_hospital": "General Hospital",
  "attention": "Attention",
  "high_glucose": "High Glucose",
  "med_track": "Medication Track",
  "completed": "Completed",
  "upcoming": "Upcoming",
  "metformin": "Metformin",
  "metformin_desc": "500mg • After Breakfast",
  "vit_d3": "Vitamin D3",
  "vit_d3_desc": "2000 IU • With Meal",
  "log_dose": "Log Dose",
  "atorvastatin": "Atorvastatin",
  "atorvastatin_desc": "20mg • Before Bed",
  "last_weight": "Last Weight",
  "lbs": "lbs",
  "weight_trend": "-2.1 lbs this month",
  "body_temp": "Body Temp",
  "f": "°F",
  "hydration": "Hydration",
  "oz": "oz",
  "daily_goal": "Daily goal: 84 oz"
};

const ar = {
  "welcome": "مرحباً بعودتك، أليكس.",
  "status_prefix": "حالتك الصحية تبدو مستقرة اليوم. لديك",
  "status_highlight": "إجراءين معلقين",
  "insight_tag": "رؤية ذكية مخصصة",
  "insight_title": "ركّز على جودة فترات نومك الليلة.",
  "insight_desc": "بناءً على مستويات الجلوكوز ومعدل ضربات القلب من أجهزتك الذكية، نقترح تقديم العشاء ساعة واحدة. قد يحسن ذلك مستوى التعافي بنسبة 14٪.",
  "view_analysis": "عرض التحليل الكامل",
  "health_score": "نتيجة الصحة",
  "need_refill": "هل تحتاج لتكرار الوصفة؟",
  "refill_desc": "وصفة ليزينوبريل توشك على الانتهاء. يمكننا التواصل مع د. العريس نيابة عنك.",
  "refill_now": "اطلب الدواء الآن",
  "latest_results": "أحدث نتائج التحاليل",
  "updated_time": "تم التحديث قبل 14 ساعة",
  "view_all": "عرض كافة السجلات",
  "cbc": "صورة الدم الكاملة (CBC)",
  "cbc_lab": "مختبرات كلينيكال الغربية",
  "normal": "طبيعي",
  "within_range": "ضمن المعدل الطبيعي",
  "ecg": "ملخص تخطيط القلب",
  "ecg_dept": "قسم أمراض القلب",
  "pending": "قيد الانتظار",
  "est_2_days": "المتوقع: يومين",
  "metabolic": "فحص الأيض الشامل",
  "gen_hospital": "المستشفى العام",
  "attention": "تنبيه",
  "high_glucose": "ارتفاع الجلوكوز",
  "med_track": "تتبع الأدوية",
  "completed": "مكتمل",
  "upcoming": "القادمة",
  "metformin": "ميتفورمين",
  "metformin_desc": "500ملغ • بعد الإفطار",
  "vit_d3": "فيتامين د3",
  "vit_d3_desc": "2000 وحدة • مع الوجبة",
  "log_dose": "تسجيل الجرعة",
  "atorvastatin": "أتورفاستاتين",
  "atorvastatin_desc": "20ملغ • قبل النوم",
  "last_weight": "أخر وزن مجسّل",
  "lbs": "رطل",
  "weight_trend": "-2.1 رطل هذا الشهر",
  "body_temp": "درجة حرارة الجسم",
  "f": "°ف",
  "hydration": "الترطيب",
  "oz": "أونصة",
  "daily_goal": "الهدف اليومي: 84 أونصة"
};

// Write JSONs
fs.writeFileSync('public/assets/i18n/dashboard/en-US.json', JSON.stringify(en, null, 2));
fs.writeFileSync('public/assets/i18n/dashboard/ar-SA.json', JSON.stringify(ar, null, 2));

// Replace HTML
let html = fs.readFileSync('src/app/features/test-page/test-page.component.html', 'utf8');

const replacements = {
  "Welcome back, Alex.": "welcome",
  "Your health sanctuary is looking stable today. You have": "status_prefix",
  "2 pending actions": "status_highlight",
  "AI Personalized Insight": "insight_tag",
  "Focus on your sleep hygiene tonight.": "insight_title",
  "Based on your glucose levels and heart rate variability from your wearables, we suggest moving your dinner 1 hour earlier. This could improve your recovery score by approximately 14%.": "insight_desc",
  "View Full Analysis": "view_analysis",
  "Health Score": "health_score",
  "Need a Refill\\?": "need_refill",
  "Your Lisinopril prescription is running low. We can contact Dr. Aris for you.": "refill_desc",
  "Refill Now": "refill_now",
  "Latest Test Results": "latest_results",
  "Updated 14 hours ago": "updated_time",
  "View All Records": "view_all",
  "Complete Blood Count": "cbc",
  "Clinical Labs West": "cbc_lab",
  "Normal": "normal",
  "Within Range": "within_range",
  "ECG Summary": "ecg",
  "Cardiology Dept.": "ecg_dept",
  "Pending": "pending",
  "Est. 2 days": "est_2_days",
  "Metabolic Panel": "metabolic",
  "General Hospital": "gen_hospital",
  "Attention": "attention",
  "High Glucose": "high_glucose",
  "Medication Track": "med_track",
  "Completed": "completed",
  "Upcoming": "upcoming",
  "Metformin": "metformin",
  "500mg • After Breakfast": "metformin_desc",
  "Vitamin D3": "vit_d3",
  "2000 IU • With Meal": "vit_d3_desc",
  "Log Dose": "log_dose",
  "Atorvastatin": "atorvastatin",
  "20mg • Before Bed": "atorvastatin_desc",
  "Last Weight": "last_weight",
  "lbs": "lbs",
  "-2.1 lbs this month": "weight_trend",
  "Body Temp": "body_temp",
  "°F": "f",
  "Hydration": "hydration",
  "oz": "oz",
  "Daily goal: 84 oz": "daily_goal"
};

for (const [text, key] of Object.entries(replacements)) {
  const regex = new RegExp(`>\\s*${text}\\s*<`, 'g');
  html = html.replace(regex, `>{{ 'dashboard.${key}' | transloco }}<`);
  
  // also handle bare text replacement without surrounding tags just in case
  const bareRegex = new RegExp(`(?<!>)` + text + `(?!<)`, 'g');
  if (html.match(regex)) {
     // Wait, if we use just generic replacement we might break things
  } else {
    // If exact > text < fails, try to just replace the text generically
    html = html.replace(new RegExp(text, 'g'), `{{ 'dashboard.${key}' | transloco }}`);
  }
}

fs.writeFileSync('src/app/features/test-page/test-page.component.html', html);

let ts = fs.readFileSync('src/app/features/test-page/test-page.component.ts', 'utf8');
if (!ts.includes('TranslocoModule')) {
    ts = ts.replace(/imports: \[/, 'imports: [TranslocoModule, ');
    ts = `import { TranslocoModule } from '@jsverse/transloco';\n${ts}`;
    fs.writeFileSync('src/app/features/test-page/test-page.component.ts', ts);
}

console.log('Dashboard translation applied');
