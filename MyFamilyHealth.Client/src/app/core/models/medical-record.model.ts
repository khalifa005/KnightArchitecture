export interface MedicalRecord {
  id: string;
  titleKey: string;
  dateKey: string;
  type: 'lab' | 'imaging' | 'ecg';
  icon: string;
  iconBgClass: string;
  iconTextClass: string;
  statusKey: string;
  statusBgClass: string;
  statusTextClass: string;
  providerKey: string;
  primaryActionKey: string;
  primaryActionIcon: string;
  isUrgent: boolean;
}
