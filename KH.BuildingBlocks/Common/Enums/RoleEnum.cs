namespace KH.BuildingBlocks.Apis.Enums;

public enum RoleEnum
{
  SuperAdmin = 1,
  CEO = 2,
  VicePresident = 3,
  DeptManager = 4,
  UnitHead = 5,
  Supervisor = 6,
  CPA = 7,
  CustomerServiceManager = 8,
  CustomerServiceSupervisor = 9,
  Agentuser = 10,
}


public static class RoleChecker
{
  public static bool HasSuperAdminRole(List<string> userRoles)
  {
    // Check if the list is null or empty
    if (userRoles == null || userRoles.Count == 0)
    {
      return false;
    }

    // Check if the list contains the SuperAdmin role ID
    return userRoles.Contains(((int)RoleEnum.SuperAdmin).ToString());
  }
}
