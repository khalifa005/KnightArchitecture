namespace KH.Services.Contracts;

public interface ITokenService
{
  string CreateToken(User request);
  string CreateToken(Customer customer);
}
