namespace KH.BuildingBlocks.Auth.Contracts;

public interface ICurrentUserService
{
  string UserId { get; }
  string FirstName { get; }
  string LastName { get; }
  string Email { get; }
  string MobileNumber { get; }
  List<string> RolesIds { get; }
  List<KeyValuePair<string, string>> Claims { get; }
}
