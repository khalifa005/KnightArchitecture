namespace KH.Dto.Models.AuthenticationDto;

public class RefreshTokenResponse
{
  public string Token { get; set; }
  public DateTime Expires { get; set; }
  public bool IsExpired => DateTime.UtcNow >= Expires;
  public DateTime Created { get; set; }
  public DateTime? Revoked { get; set; }
  public bool IsActive => Revoked == null && !IsExpired;
}

