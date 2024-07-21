namespace KH.Helper.Contracts.Infrastructure
{
  public interface ITokenService
  {
    //string CreateToken(IdentityUser user);
    string CreateToken(object user);
    //string CreateToken(object customer);
    //string CreateToken(CustomerDto customer);
  }
}
