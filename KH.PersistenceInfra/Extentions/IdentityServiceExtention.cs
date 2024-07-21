
using Microsoft.AspNetCore.Authorization;

namespace KH.PersistenceInfra.Extentions
{
	public static class IdentityServiceExtention
	{
		public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenSettings:Key"])),
						ValidIssuer = configuration["TokenSettings:Issuer"],
						ValidateIssuer = true,
						ValidateAudience = false,
					};
				});

			// Overrides the DefaultAuthorizationPolicyProvider with our own
			//services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

			return services;
		}
	}

}
