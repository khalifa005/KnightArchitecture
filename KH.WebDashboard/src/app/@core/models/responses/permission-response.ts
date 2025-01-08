


export class PermissionResponse {
  id: number;
  key: string;
  nameEn: string;
  nameAr: string;
  disabled: boolean;
  checked: boolean;
  childrens: PermissionResponse[];
  sortKey: number;
  parentId: number | null;
  roleId!: number | null; // Added property to hold roleId later on


  constructor(data: Partial<PermissionResponse>) {
    this.id = data.id || 0;
    this.key = data.key || '';
    this.nameEn = data.nameEn || '';
    this.nameAr = data.nameAr || '';
    this.disabled = data.disabled || false;
    this.checked = data.checked || false;
    this.childrens = (data.childrens || []).map(child => new PermissionResponse(child));
    this.sortKey = data.sortKey || 0;
    this.parentId = data.parentId || null;
  }
}
