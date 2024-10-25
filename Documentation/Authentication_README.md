
# Authentication, Authorization, and Permissions Process in This Repository
Protecting your API endpoints with dynamic policies in ASP.NET Core 
This project utilizes JWT-based authentication and role-based authorization to protect its API endpoints. Additionally, custom permissions are implemented using middleware to ensure that users have the necessary access rights to specific resources based on their roles.

## Overall Process Flow

### User Request:
The process begins when a user makes a request to the API. The request contains a JWT token that is included in the headers, which will be validated for authentication.

### `UseAuthentication` Middleware:
- The first step in the process is authentication. The `UseAuthentication()` middleware inspects the request and checks the JWT token to authenticate the user.
  - If the token is valid, the user's identity and claims are set in the `HttpContext.User`.
  - If the user is not authenticated (e.g., the token is missing or invalid), a `401 Unauthorized` response is returned, and the request does not proceed any further.

### `PermissionsMiddleware`:
- After authentication, the `PermissionsMiddleware` is invoked. This middleware performs two key tasks:
  1. **Check for Authentication**: If the user is authenticated, it proceeds to the next step.
  2. **Fetch and Validate Permissions**: The `IUserPermissionService` fetches the permissions assigned to the user based on their role. These permissions are added to the user's claims.
     - If the user lacks the necessary permissions, an Access Denied message is returned. Permissions are fetched from the database or cache and may depend on the user's specific role.

### `UseAuthorization` Middleware:
- After permissions are validated, the `UseAuthorization()` middleware ensures that the user has the correct authorization level (e.g., roles or policies) to access the requested resource.
- Authorization is enforced based on the claims that were set during the previous steps.

### Controller Action Execution:
- Once authentication, permissions, and authorization checks are successfully passed, the API controller action is executed, and the requested resource is processed and returned.

## Permissions Process

In addition to standard role-based authorization, this project uses a custom permissions-based approach to fine-tune user access control. Here’s how permissions are handled:

### `PermissionsMiddleware`:
- The `PermissionsMiddleware` ensures that authenticated users have the necessary permissions to access specific endpoints or resources. This middleware leverages `IUserPermissionService`, which fetches user permissions from the database.

### Permission Claims:
- Once the user's permissions are fetched, they are added to the user’s claims. These claims can be checked at any point within the API to ensure that the user has the necessary rights to perform actions (e.g., view, edit, delete).

### Database-Based Permissions:
- Permissions are stored in the database and are tied to user roles. Each role can have different sets of permissions, and these are fetched dynamically during each request.

### Authorization Based on Permissions:
- Once the user's permissions are added to the claims, authorization policies are enforced based on these permissions. If a user lacks the required permission for an action, access is denied.

## Visual Flowchart
(TODO: Add a flowchart here to visualize the process)

## Detailed Example of Permissions Handling

Consider an example where an API endpoint requires the user to have a specific permission to access financial data. Here's how the flow would work:

1. The user makes a request to `/api/finance/report`.
2. The JWT token is validated by `UseAuthentication()`.
3. The `PermissionsMiddleware` checks if the user is authenticated and fetches permissions (e.g., `ViewFinancialReports`).
4. The permissions are added to the user's claims.
5. The `UseAuthorization` middleware checks if the user has the `ViewFinancialReports` permission.
6. If the user has the required permission, the request proceeds to the controller, and the financial report is returned.
7. If the user lacks the required permission, a `403 Forbidden` response is returned, indicating that the user doesn't have the necessary access rights.

## Middleware Configuration in `Startup.cs`

The `Startup.cs` file configures the authentication, permissions, and authorization middleware:

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseAuthentication();  // Handles authentication (JWT)
    app.UseMiddleware<PermissionsMiddleware>();  // Custom permissions handling
    app.UseAuthorization();  // Role-based authorization
}
```

# .NET 8 Application - Authentication and Authorization Flow

This application implements a flexible and secure authentication, authorization, and permissions management system. It utilizes JWT and Basic authentication schemes, dynamic permission policies, and a middleware-based permissions management approach.

## Key Features

### 1. **Dynamic Permission-Based Authorization**
- **Custom Permission Policy:** The application uses dynamic policies to control access, allowing flexibility to extend or modify permissions without changing the core code.
- **Fine-Grained Control:** You can define permission checks using both `AND` and `OR` logic, ensuring users have precise access based on permissions, not just roles.

### 2. **Dual Authentication Mechanism**
- **Basic or JWT Authentication:** The app supports both JWT and Basic authentication schemes, selecting the appropriate one based on the `Authorization` header.
- **Single Entry Point:** The `BasicOrJwt` policy ensures seamless authentication for both scheme types.

### 3. **SuperAdmin Role Override**
- **SuperAdmin Bypass:** Users with the `SuperAdminRole` can bypass permission checks, which simplifies management for administrative operations.

### 4. **Middleware-Based Permission Management**
- **Scalable Design:** The `PermissionsMiddleware` dynamically retrieves and attaches permissions to the user’s claims and can be extended with caching for improved performance.
- **Pluggable Permission System:** The permissions middleware centralizes permission management, allowing easy adaptation to new requirements.

### 5. **Comprehensive Error Handling**
- **Localized Error Responses:** Custom error responses are provided for various authentication and authorization scenarios, with support for both English and Arabic.

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
