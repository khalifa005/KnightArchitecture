namespace KH.BuildingBlocks.Apis.Helpers;

public static class CookieHelper
{
  public static void SetCookie(HttpResponse response, string cookieName, string cookieValue, bool httpOnly = true, int expirationDays = 10)
  {
    var cookieOptions = new CookieOptions
    {
      HttpOnly = httpOnly,
      Expires = DateTime.UtcNow.AddDays(expirationDays),
    };

    response.Cookies.Append(cookieName, cookieValue, cookieOptions);
  }
}
