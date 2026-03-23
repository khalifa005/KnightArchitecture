const fs = require('fs');

const en = {
  "title": "Security & Governance",
  "subtitle": "Manage who has access to your clinical records. Granular controls ensure your medical data is shared only with people you trust.",
  "family_sharing": "Family Sharing",
  "add_member": "Add Member",
  "elena": "Elena Mitchell",
  "elena_role": "Spouse • Emergency Contact",
  "full_access": "Full Access",
  "verified_oct": "Verified Oct 2023",
  "james": "James Mitchell",
  "james_role": "Son • Dependent",
  "view_only": "View Only",
  "managed_account": "Managed Account",
  "sarah": "Sarah Reed",
  "sarah_role": "Primary Caregiver",
  "authorized": "Authorized",
  "expires_dec": "Expires Dec 2024",
  "provider_requests": "Provider Requests",
  "dr_aris": "Dr. Aris Thorne",
  "dr_aris_spec": "Cardiology Specialist",
  "st_jude": "Saint Jude Medical Center",
  "aris_req": "\"Requesting access to history and current medication list for upcoming consultation.\"",
  "deny": "Deny",
  "approve": "Approve",
  "active_auth": "Active Authorization",
  "dr_linda": "Dr. Linda Vance",
  "dr_linda_spec": "Internal Medicine",
  "active": "ACTIVE",
  "linda_date": "Full record access granted 02/12/2024",
  "revoke": "Revoke",
  "privacy_reminder": "Privacy Reminder",
  "privacy_desc": "Providers only see the records you explicitly authorize. You can revoke access at any time with immediate effect.",
  "recent_activity": "Recent Activity",
  "log_desc": "A log of all changes made to your access settings",
  "view_history": "View Full History",
  "access_approved": "Access Approved",
  "log_linda": "Dr. Linda Vance was granted view access to 'Medication History'.",
  "ago_2h": "2 hours ago",
  "member_added": "Member Added",
  "log_elena": "Elena Mitchell was added as an Emergency Contact.",
  "yest_430": "Yesterday at 4:30 PM",
  "access_revoked": "Access Revoked",
  "log_lab": "Temporary access for City General Lab has expired.",
  "ago_3d": "3 days ago"
};

const ar = {
  "title": "الأمان والحوكمة",
  "subtitle": "إدارة من لديه حق الوصول إلى سجلاتك السريرية. تضمن عناصر التحكم الدقيقة مشاركة بياناتك الطبية فقط مع الأشخاص الذين تثق بهم.",
  "family_sharing": "المشاركة العائلية",
  "add_member": "إضافة فرد",
  "elena": "إيلينا ميتشل",
  "elena_role": "الزوجة • جهة اتصال للطوارئ",
  "full_access": "وصول كامل",
  "verified_oct": "تم التحقق منه في أكتوبر 2023",
  "james": "جيمس ميتشل",
  "james_role": "الابن • مُعال",
  "view_only": "للعرض فقط",
  "managed_account": "حساب مُدار",
  "sarah": "سارة ريد",
  "sarah_role": "مقدم الرعاية الأساسي",
  "authorized": "مُصرّح له",
  "expires_dec": "ينتهي في ديسمبر 2024",
  "provider_requests": "طلبات مقدمي الخدمة",
  "dr_aris": "د. أريس ثورن",
  "dr_aris_spec": "أخصائي أمراض القلب",
  "st_jude": "مركز سانت جود الطبي",
  "aris_req": "\"طلب الوصول إلى التاريخ السريري وقائمة الأدوية الحالية للاستشارة القادمة.\"",
  "deny": "رفض",
  "approve": "موافقة",
  "active_auth": "تصريح نشط",
  "dr_linda": "د. ليندا فانس",
  "dr_linda_spec": "الطب الباطني",
  "active": "نشط",
  "linda_date": "تم منح حق الوصول الكامل للسجل في 02/12/2024",
  "revoke": "إلغاء",
  "privacy_reminder": "تذكير بالخصوصية",
  "privacy_desc": "يرى مقدمو الخدمة فقط السجلات التي تُصرّح بها صراحةً. يمكنك إلغاء الوصول في أي وقت وسيسري مفعوله فوراً.",
  "recent_activity": "النشاط الأخير",
  "log_desc": "سجل بجميع التغييرات التي أُجريت على إعدادات الوصول الخاصة بك",
  "view_history": "عرض السجل الكامل",
  "access_approved": "تمت الموافقة على الوصول",
  "log_linda": "تم منح د. ليندا فانس حق عرض 'تاريخ الأدوية'.",
  "ago_2h": "قبل ساعتين",
  "member_added": "تمت إضافة عضو",
  "log_elena": "تمت إضافة إيلينا ميتشل كجهة اتصال للطوارئ.",
  "yest_430": "أمس الساعة 4:30 مساءً",
  "access_revoked": "تم إلغاء الوصول",
  "log_lab": "انتهت صلاحية الوصول المؤقت لمختبر المدينة العام.",
  "ago_3d": "قبل 3 أيام"
};

fs.writeFileSync('public/assets/i18n/access-control/en-US.json', JSON.stringify(en, null, 2));
fs.writeFileSync('public/assets/i18n/access-control/ar-SA.json', JSON.stringify(ar, null, 2));

let html = fs.readFileSync('src/app/features/access-control/access-control.html', 'utf8');

const replacements = {
  "Security &amp; Governance": "title",
  "Manage who has access to your clinical records. Granular controls ensure your medical data is shared only with people you trust.": "subtitle",
  "Family Sharing": "family_sharing",
  "Add Member": "add_member",
  "Elena Mitchell was added as an Emergency Contact.": "log_elena",
  "Elena Mitchell": "elena",
  "Spouse • Emergency Contact": "elena_role",
  "Full Access": "full_access",
  "Verified Oct 2023": "verified_oct",
  "James Mitchell": "james",
  "Son • Dependent": "james_role",
  "View Only": "view_only",
  "Managed Account": "managed_account",
  "Sarah Reed": "sarah",
  "Primary Caregiver": "sarah_role",
  "Authorized": "authorized",
  "Expires Dec 2024": "expires_dec",
  "Provider Requests": "provider_requests",
  "Dr. Aris Thorne": "dr_aris",
  "Cardiology Specialist": "dr_aris_spec",
  "Saint Jude Medical Center": "st_jude",
  "\"Requesting access to history and current medication list for upcoming consultation.\"": "aris_req",
  "Deny": "deny",
  "Approve": "approve",
  "Active Authorization": "active_auth",
  "Dr. Linda Vance was granted view access to 'Medication History'.": "log_linda",
  "Dr. Linda Vance": "dr_linda",
  "Internal Medicine": "dr_linda_spec",
  ">ACTIVE<": ">{{ 'access-control.active' | transloco }}<",
  "Full record access granted 02/12/2024": "linda_date",
  "Revoke": "revoke",
  "Privacy Reminder": "privacy_reminder",
  "Providers only see the records you explicitly authorize. You can revoke access at any time with immediate effect.": "privacy_desc",
  "Recent Activity": "recent_activity",
  "A log of all changes made to your access settings": "log_desc",
  "View Full History": "view_history",
  "Access Approved": "access_approved",
  "2 hours ago": "ago_2h",
  "Member Added": "member_added",
  "Yesterday at 4:30 PM": "yest_430",
  "Access Revoked": "access_revoked",
  "Temporary access for City General Lab has expired.": "log_lab",
  "3 days ago": "ago_3d"
};

for (const [text, key] of Object.entries(replacements)) {
  if (text.startsWith('>')) {
     html = html.replace(new RegExp(text.replace(/\\/g, ''), 'g'), key);
     continue;
  }
  
  const escapedText = text.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
  const regex = new RegExp(`>\\s*` + escapedText + `\\s*<`, 'g');
  
  if (html.match(regex)) {
    html = html.replace(regex, `>{{ 'access-control.${key}' | transloco }}<`);
  } else {
    html = html.replace(new RegExp(escapedText, 'g'), `{{ 'access-control.${key}' | transloco }}`);
  }
}

fs.writeFileSync('src/app/features/access-control/access-control.html', html);

let ts = fs.readFileSync('src/app/features/access-control/access-control.ts', 'utf8');
if (!ts.includes('TranslocoModule')) {
    ts = ts.replace(/imports: \[/, 'imports: [TranslocoModule, ');
    ts = `import { TranslocoModule } from '@jsverse/transloco';\n${ts}`;
    fs.writeFileSync('src/app/features/access-control/access-control.ts', ts);
}

console.log('Access Control translation applied');
