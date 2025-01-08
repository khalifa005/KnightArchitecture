import { PermissionResponse } from "./permission-response";

export class RolesResponse {
  reportToRoleId!: number | null;
  reportToRole!: RolesResponse | null;
  subRoles: RolesResponse[] = [];
  permissions: PermissionResponse[] = [];
  nameAr!: string;
  nameEn!: string;
  description!: string | null;
  createdDate!: string;
  createdById!: number | null;
  updatedDate!: string | null;
  updatedById!: number | null;
  isDeleted!: boolean;
  deletedDate!: string | null;
  deletedById!: number | null;
  id!: number;
}