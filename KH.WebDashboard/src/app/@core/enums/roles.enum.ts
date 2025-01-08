export enum UserRoleEnum {
  SuperAdmin = 1,
  Support = 5,
}

function isValidRoleId(roleId: number): boolean {
  return Object.values(UserRoleEnum).includes(roleId);
}