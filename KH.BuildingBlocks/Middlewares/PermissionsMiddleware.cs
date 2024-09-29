//using Azure;
//using KH.Helper.Models;
//using Microsoft.Extensions.Caching.Memory;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.WebSockets;
//using System.Security.Claims;
//using System.Text;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;

//namespace KH.Helper.Middlewares
//{
//	/// <summary>
//	/// In CASE USER IsAuthenticated 
//	/// Collect his Permissions from (DB/Caching) and add to Identity Claims With Type [permissions]
//	/// </summary>
//	public class PermissionsMiddleware
//	{
//		private readonly RequestDelegate _request;
//		private readonly ILogger<PermissionsMiddleware> _logger;
//		private IResponseCacheService _cache;
//		public PermissionsMiddleware(RequestDelegate request, ILogger<PermissionsMiddleware> logger, IResponseCacheService cache)
//		{
//			_request = request;
//			_logger = logger;
//			_cache = cache;
//		}


//		public async Task InvokeAsync(HttpContext context, IUserPermissionService permissionService)
//		{
//			if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
//			{
//				await _request(context);
//				return;
//			}

//			var cancellationToken = context.RequestAborted;
//			var permissionsIdentity = new ClaimsIdentity(nameof(PermissionsMiddleware), "name", "role");

//			if (context.User.HasSuperAdminRole())
//			{
//				//--Add Default Permissions In case User System admin
//				permissionsIdentity.AddClaim(new Claim(PermissionRequirement.ClaimType, ApplicationConstant.SUPER_ADMIN_PERMISSION));
//			}
//			else
//			{
//				var userId = context.User.GetId();
//				if (string.IsNullOrEmpty(userId))
//				{
//					await context.Response.WriteAsync("User 'Identifier' claim is required",
//					cancellationToken);
//					return;
//				}

//				var systemType = context.User.GetSystemType();
//				var userRoles = context.User.GetUserRole();

//				permissionsIdentity = await permissionService.GetUserPermissionsIdentity(int.Parse(userId), systemType, cancellationToken);
//				if (permissionsIdentity == null)
//				{
//					_logger.LogWarning("User {Identifier} does not have permissions", userId);
//					await context.Response.WriteAsync("Access denied",
//					cancellationToken);
//					return;
//				}
//			}
//			// User has permissions, so we add the extra identity containing the "permissions" claims
//			context.User.AddIdentity(permissionsIdentity);

//			//-- TODO: Remove Comment To Allow Redis Caching
//			/*
//			var responseCached = await _cache.GetCachedResponseAsync($"Permissions_{userId}");
//			var permissionsIdentity = new ClaimsIdentity();
//			if (responseCached == null)
//			{
//				permissionsIdentity = await permissionService.GetUserPermissionsIdentity(int.Parse(userId), systemType, cancellationToken);
//				if (permissionsIdentity == null)
//				{
//					_logger.LogWarning("User {Identifier} does not have permissions", userId);
//					await context.Response.WriteAsync("Access denied",
//					cancellationToken);
//					return;
//				}
//				await _cache.CacheResponseAsync($"Permissions_{userId}", permissionsIdentity, TimeSpan.FromSeconds(500000));
//			}
//			else
//			{
//				permissionsIdentity = JsonConvert.DeserializeObject<ClaimsIdentity>(responseCached, new ClaimConverter());
//			}
//			if (permissionsIdentity != null)
//			{
//				// User has permissions, so we add the extra identity containing the "permissions" claims
//				context.User.AddIdentity(permissionsIdentity);
//			}
//			*/

//			await _request(context);
//		}
//	}
//}
