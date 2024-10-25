# Dual Authentication - Dynamic permissions management

## Overview
The KnightArchitecture project uses a robust authentication and authorization framework. It leverages JWT (JSON Web Token)-based and Basic authentication schemes for identity verification, coupled with a middleware-driven permissions management system that operates through dynamic policies. This structure is designed to support flexible, role-based access and modular permissions for user access control.

## Middleware Configuration in Startup
The `Startup.cs` file configures authentication, permissions, and authorization middleware:
```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseAuthentication();
    app.UseMiddleware<PermissionsMiddleware>();
    app.UseAuthorization();
}
```

## Key Features

### 1. **Dynamic Permission-Based Authorization**
- **Custom Permission Policy:** The application uses dynamic policies to control access, allowing flexibility to extend or modify permissions without changing the core code.
- **Fine-Grained Control:** You can define permission checks using both `AND` and `OR` logic, ensuring users have precise access based on permissions, not just roles.

### 2. **Dual Authentication Mechanism**
- **Basic or JWT Authentication:** The app supports both JWT and Basic authentication schemes, selecting the appropriate one based on the `Authorization` header.
- **Single Entry Point:** The `BasicOrJwt` policy ensures seamless authentication for both scheme types.

### 3. **Middleware-Based Permission Management**
- **Scalable Design:** The `PermissionsMiddleware` dynamically retrieves and attaches permissions to the user’s claims and can be extended with caching for improved performance.
- **Pluggable Permission System:** The permissions middleware centralizes permission management, allowing easy adaptation to new requirements.

---

## Execution Flow: Step-by-Step Breakdown

Here’s how the authentication, authorization, and permissions management system works step-by-step.

### 1. **Incoming Request**
- The client sends a request to a protected API (e.g., `PUT /roles`).
- The request includes an `Authorization` header with either a JWT token or Basic authentication credentials.

### 2. **Authentication Scheme Selection**
- **JWT Token:** If the header starts with `Bearer`, JWT is selected.
- **Basic Authentication:** If the header starts with `Basic`, Basic authentication is selected.

### 3. **Authentication Handler Execution**
- **JWT Authentication:** Validates the token's signature and claims.
- **Basic Authentication:** Decodes and validates the credentials, then retrieves user claims.

### 4. **Permissions Middleware Execution**
- **Anonymous Endpoint Check:** Skips permission checks for endpoints marked with `[AllowAnonymous]`.
- **SuperAdmin Check:** If the user has the `SuperAdminRole`, they bypass permission checks.
- **User Permissions Retrieval:** Permissions are fetched from the `IUserPermissionService` and added to the user’s claims.

### 5. **Authorization Policy Provider Execution**
- The system dynamically generates an authorization policy based on the required permissions for the current endpoint.

### 6. **Authorization Handler Execution**
- The `PermissionRequirement` checks whether the user has the necessary permissions. If the user has all the required permissions, the request is allowed.

### 7. **Role Permission Check**
- For users without the `SuperAdminRole`, the permissions are checked against the required permissions for the current endpoint.

### 8. **API Response Execution**
- If the user passes authentication and authorization, the API method is executed, and the response is returned.

### 9. **Error Handling and Feedback**
- If authentication or authorization fails, appropriate 401 (Unauthorized) or 403 (Forbidden) responses are returned, with localized error messages.

---

## Advantages

- **Dynamic Policies:** Flexible and easy-to-extend authorization rules.
- **Hybrid Authentication:** Supports both JWT and Basic authentication, allowing different types of users.
- **SuperAdmin Override:** Simplified management for administrative users.
- **Middleware-Based:** Centralized permission management, making it modular and scalable.
- **Localized Errors:** Detailed error messages in both English and Arabic for better user experience.

## Key Components and Responsibilities

### Authentication Layer
Managed by JWT and Basic schemes for user validation, this layer includes:
- **`AuthenticationService.cs`**: Handles login, token generation, and session management.
- **`TokenService.cs`**: Creates JWT tokens with user claims and roles for future requests.
- **`AuthenticationController.cs`**: Exposes login and token refresh endpoints.

### Authorization Layer
Enforces role-based access using middleware and policies:
- **`PermissionAuthorizeAttribute.cs`**: Custom attribute enforcing strict control over API endpoints.
- **`PermissionAuthorizationPolicyProvider.cs`**: Generates policies based on required permissions.
- **`PermissionRequirement.cs`**: Defines and validates custom authorization requirements.

### Permissions Management Layer
Permissions are dynamically managed through middleware:
- **`PermissionsMiddleware.cs`**: Retrieves and verifies user permissions for each request.
- **`IUserPermissionService.cs`**: Interface defining the contract for permissions retrieval.
- **`PermissionService.cs`**: Manages CRUD operations for permissions.
- **`PermissionController.cs`**: API controller for permissions management.

```
var permissionsIdentity = await permissionService.GetUserPermissionsIdentity(long.Parse(userId), systemType, cancellationToken);
if (permissionsIdentity == null) {
    // User lacks necessary permissions, return 401 Unauthorized
    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    await context.Response.WriteAsync("Unauthorized access");
    return;
}
context.User.AddIdentity(permissionsIdentity); // Adds permissions as claims
```

## 4. Dynamic Policy Creation
**File(s):**
- `KH.BuildingBlocks/Auth/PermissionAuthorizationPolicyProvider.cs`
- `KH.BuildingBlocks/Auth/Attributes/PermissionAuthorizeAttribute.cs`

**Process:**
- **Policy Provider (PermissionAuthorizationPolicyProvider):**
  - For endpoints protected with `PermissionAuthorizeAttribute`, the `PermissionAuthorizationPolicyProvider` dynamically creates an authorization policy based on permissions (e.g., `VIEW_CUSTOMER`).
  - The provider interprets required permissions using the `PolicyPrefix` and generates policies to enforce fine-grained access control.

- **Policy Enforcement:**
  - The policy is checked against the user’s claims.
  - If the policy’s permissions match the user’s claims, the request proceeds to the next stage; otherwise, a `403 Forbidden` is returned.

**Example Code in PermissionAuthorizationPolicyProvider.cs:**

```csharp
var requirement = new PermissionRequirement(PermissionOperatorEnum.And, permissions);
return new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .AddRequirements(requirement) // Enforces permissions on this endpoint
    .Build();
```

## 5. Controller Endpoint Protection
**File(s):**
- `KH.WebApi/Controllers/PermissionsController.cs`
- `KH.BuildingBlocks/Auth/Constant/PermissionKeysConstant.cs`

**Process:**
- **Endpoint Definition (PermissionsController):**
  - Each endpoint requiring permission is protected with `PermissionAuthorizeAttribute`. For example, the `Get` method requires `PermissionKeysConstant.Customers.VIEW_CUSTOMER`.

- **Access Control:**
  - The attribute checks if the user has the necessary permission claim (`VIEW_CUSTOMER`). If not, it denies access with a `403 Forbidden`.

**Example Code in PermissionsController.cs:**
```csharp

[PermissionAuthorize(PermissionKeysConstant.Customers.VIEW_CUSTOMER)]
[HttpGet("{id}")]
public async Task<ActionResult<ApiResponse<CustomerResponse>>> Get(int id, CancellationToken cancellationToken) {
    var res = await _lookupService.GetCustomerAsync(id, cancellationToken);
    return AsActionResult(res); // Only accessible if user has VIEW_CUSTOMER permission
}
```
## 6. Response Handling

- **Successful Access:**
  - If the Sales Manager has `VIEW_CUSTOMER` permission, they receive the requested customer details.

- **Access Denied:**
  - If permissions are lacking (e.g., trying to delete without `DELETE_CUSTOMER`), a `403 Forbidden` response with a message, potentially in multiple languages, is returned.

### Advantages of This Approach
- **Centralized, Scalable Permissions Management:** The `PermissionsMiddleware` handles all permission checks, centralizing access control and simplifying adjustments.
- **Flexible Role Management:** With the ability to define policies dynamically and manage permissions centrally, the system adapts easily to changes in business requirements.
- **Granular Access Control:** Dynamic policies with AND/OR operators allow complex access control scenarios, supporting a wide range of user roles and permissions.
- **SuperAdmin Override:** SuperAdmins bypass all permission checks, making administrative actions streamlined and secure.
- **Localized Error Responses:** Users are informed of errors in multiple languages, improving usability and accessibility.




![Untitled diagram-2024-10-25-125950](https://github.com/user-attachments/assets/d24a3dcf-d7a6-436f-b283-81ad8ca0b37e)



