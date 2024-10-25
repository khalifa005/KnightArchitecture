# Authentication, Authorization, and Permission Management in KnightHedgeArchitecture

This document outlines the setup and implementation of authentication, authorization, and permission management in the `KnightHedgeArchitecture` repository. The project supports dual authentication schemes (JWT and Basic) and uses role-based access control (RBAC) for permissions.

## Table of Contents
1. [Define Permissions Models](#define-permissions-models)
2. [Create Authentication Services](#create-authentication-services)
3. [Define Authorization Requirements and Policies](#define-authorization-requirements-and-policies)
4. [Implement Permissions Middleware](#implement-permissions-middleware)
5. [Develop Permission Service](#develop-permission-service)
6. [Configure Startup for Middleware and Authentication](#configure-startup-for-middleware-and-authentication)
7. [Protect API Endpoints Using Permissions](#protect-api-endpoints-using-permissions)

---

### 1. Define Permissions Models

Start by defining the permissions and roles required for managing user access.

- **Permission.cs**
  ```csharp
  namespace KH.Domain.Entities;
  public class Permission : LookupEntity
  {
      public string Key { get; set; }
      public int SortKey { get; set; }
      public long? ParentId { get; set; }
      public virtual Permission Parent { get; set; }
      public long? DependOnId { get; set; }
      public ICollection<Permission> Children { get; set; } = new HashSet<Permission>();
      public ICollection<RolePermissions> RolePermissions { get; set; } = new HashSet<RolePermissions>();
  }
  ```


```csharp

namespace KH.Domain.Entities;
public class Role : LookupEntity
{
    public long? ReportToRoleId { get; set; }
    public Role? ReportToRole { get; set; }
    public ICollection<Role> SubRoles { get; set; } = new HashSet<Role>();
    public ICollection<RolePermissions> RolePermissions { get; set; } = new HashSet<RolePermissions>();
}
```

RolePermissions.cs

```csharp

namespace KH.Domain.Entities;
public class RolePermissions : TrackerEntity
{
    public long PermissionId { get; set; }
    public Permission Permission { get; set; }
    public long RoleId { get; set; }
    public Role Role { get; set; }
}
```
2. Create Authentication Services
Create services that handle login, token generation, and session management.

IAuthenticationService.cs

```csharp

namespace KH.Services.Users.Contracts;
public interface IAuthenticationService
{
    Task<ApiResponse<AuthenticationResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<AuthenticationResponse>> RefreshUserTokenAsync(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken);
    Task<List<Claim>> GetUserClaimsAsync(LoginRequest request);
}
```

AuthenticationService.cs

```csharp

namespace KH.Services.Users.Implementation;
public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly ICurrentUserService _currentUserService;

    public AuthenticationService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _tokenService = tokenService;
    }

    public async Task<ApiResponse<AuthenticationResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        // Login logic
    }
}
```

3. Define Authorization Requirements and Policies
Define classes that manage permission-based authorization.

PermissionAuthorizeAttribute.cs

```csharp

namespace KH.BuildingBlocks.Auth.Attributes;
public class PermissionAuthorizeAttribute : AuthorizeAttribute
{
    public PermissionAuthorizeAttribute(string permission)
    {
        Policy = $"PERMISSION_{permission}";
    }
}
```

PermissionRequirement.cs

```csharp

namespace KH.BuildingBlocks.Auth;
public class PermissionRequirement : AuthorizationHandler<PermissionRequirement>, IAuthorizationRequirement
{
    public PermissionRequirement(string[] permissions)
    {
        Permissions = permissions;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.HasClaim(claim => requirement.Permissions.Contains(claim.Value)))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
        return Task.CompletedTask;
    }
}
```

4. Implement Permissions Middleware
The middleware enforces permission requirements on each request.

PermissionsMiddleware.cs
```csharp

namespace KH.BuildingBlocks.Auth.Midilleware;
public class PermissionsMiddleware
{
    private readonly RequestDelegate _next;

    public PermissionsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserPermissionService permissionService)
    {
        if (!context.User.Identity.IsAuthenticated)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized access");
            return;
        }

        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            var userPermissions = await permissionService.GetUserPermissionsIdentity(long.Parse(userId));
            context.User.AddIdentity(userPermissions);
        }

        await _next(context);
    }
}
```

5. Develop Permission Service
Define a service to manage and cache user permissions.

IUserPermissionService.cs

```csharp

namespace KH.BuildingBlocks.Auth.Contracts;
public interface IUserPermissionService
{
    Task<ClaimsIdentity?> GetUserPermissionsIdentity(long userId);
}
UserPermissionService.cs
```

```csharp

namespace KH.Services.Users.Implementation;
public class UserPermissionService : IUserPermissionService
{
    public async Task<ClaimsIdentity?> GetUserPermissionsIdentity(long userId)
    {
        // Logic to fetch user permissions based on roles
    }
}
```

6. Configure Startup for Middleware and Authentication
Configure Startup.cs to register the middleware and services.

```csharp

public void ConfigureServices(IServiceCollection services)
{
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
    services.AddAuthorization(options =>
    {
        options.AddPolicy("PermissionPolicy", policy => policy.Requirements.Add(new PermissionRequirement()));
    });
    services.AddSingleton<IUserPermissionService, UserPermissionService>();
}

public void Configure(IApplicationBuilder app)
{
    app.UseAuthentication();
    app.UseMiddleware<PermissionsMiddleware>();
    app.UseAuthorization();
}
```

7. Protect API Endpoints Using Permissions
Use PermissionAuthorizeAttribute on controller endpoints to enforce permissions.

PermissionsController.cs
```csharp

namespace KH.WebApi.Controllers;
public class PermissionsController : BaseApiController
{
    [PermissionAuthorize("VIEW_PERMISSION")]
    public IActionResult GetPermissions()
    {
        return Ok("Permissions Access Granted");
    }
}
```
