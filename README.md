# TODO

 **JWT-permissions**:

poly 

[![Support ❤️ Buy me a coffee]](https://www.buymeacoffee.com/yourprofile)
[![Support ❤️ Buy me a coffee](https://yourdomain.com/path-to-your-blue-light-badge-image.png)](https://www.buymeacoffee.com/yourprofile)


<p align="center">
  <img src="https://github.com/user-attachments/assets/2dd2ad50-f923-4c2c-900a-b1ada84b15a9" alt="v2 - Copy" width="300" height="300" />
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/159a3ae0-72be-48d6-99fc-c9d429ed8ee7" alt="v2 - Copy" width="300" height="300" />
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/0036aea1-f0ea-4d6f-9e93-2cb41361525d" alt="v2 - Copy" width="300" height="300" />
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/adadb974-c22f-4bf9-a88f-41d75128b3af" alt="v2 - Copy" width="300" height="300" />
</p>

# Knight Hedge Template

This repository is structured following the principles of **Clean Architecture** and **Domain-Driven Design (DDD)**. The main idea is to decouple the different concerns of the application into layers, ensuring maintainability, scalability, and testability. Each layer has a clear responsibility, making it easier to navigate and extend the project as it grows.

## Table of Contents
1. [KH.Core](#KHCore)
2. [KH.Domain](#KHDomain)
3. [KH.Dto](#KHDto)
4. [KH.PersistenceInfra](#KHPersistenceInfra)
5. [KH.Services](#KHServices)
6. [KH.WebApi](#KHWebApi)
7. [Used Packages](#UsedPackages)
8. [Upcoming Features](#UpcomingFeatures)
9. [How to Run the Project](#HowToRun)

---

## KH.Core

### Description:
`KH.Core` contains the core logic and shared resources across the application. This layer includes attributes, constants, middleware, extensions, and core service contracts. These components are meant to be reusable across different layers of the application, ensuring consistency and adhering to the **DRY (Don't Repeat Yourself)** principle.

### Files:

- **Attributes/**: 
    - `CachedAttribute.cs`: This attribute is used to mark methods or endpoints that should cache their results for improved performance.
    - `PermissionAuthorizeAttribute.cs`: Handles authorization logic to verify user permissions for specific actions.

- **Constant/**: 
    - `ApplicationConstant.cs`: Defines global constants that can be used across the application to avoid hardcoding values and promote reusability.

- **Contracts/Infrastructure/**:
    - `IResponseCacheService.cs`: Defines the contract for caching responses. Implementations of this service will be responsible for caching and retrieving responses from a cache store (e.g., Redis).
    - `ITokenService.cs`: Interface for token management services, typically used for generating and validating JWT tokens for authentication.
    - `IUserPermissionService.cs`: Defines methods to handle user permission checks and roles.

- **Contracts/Persistence/**:
    - `IGenericRepository.cs`: This interface defines a generic repository pattern for CRUD operations on the database, abstracting the database access logic.
    - `IUnitOfWork.cs`: This interface ensures atomic operations by grouping multiple database actions into a transaction that either completes fully or not at all.

- **Extentions/Methods/**:
    - `Common.cs`: Contains common utility methods shared across the application.
    - `CustomStringHelper.cs`: Helper methods for string manipulation, like parsing and formatting.
    - `HttpRequestHelper.cs`: Utility methods to simplify handling HTTP requests.
    - `RandomNumberGenerator.cs`: Used to generate random numbers, often for security purposes (e.g., generating random OTP codes).
    - `UserExtensions.cs`: Extension methods related to user objects, providing helper logic for managing users.

- **Middlewares/**:
    - `ExceptionMiddleware.cs`: A middleware to handle global exceptions and return structured error responses.
    - `PermissionsMiddleware.cs`: Handles permission validation on HTTP requests to ensure the correct user roles can access the API resources.
    - `SwaggerMiddleware.cs`: Enhances the Swagger documentation with dynamic elements.

- **Responses/**:
    - `ApiResponse.cs`: Standardized response wrapper for API results, ensuring consistent structure in all responses.
    - `PagedResponse.cs`: Handles pagination logic for API responses that return large sets of data, making it easier to navigate.
    - `Result.cs`: Represents a result pattern that includes success status, data, and error messages if any.

- **Settings/**:
    - `FileSettings.cs`: Stores settings related to file management (e.g., file upload paths).
    - `TokenSettings.cs`: Configuration for handling JWT tokens, such as token expiry and secret keys.
    - `MailSettings.cs`: SMTP configuration for sending emails.

### Benefits:
- **Centralized Core Functionality**: Having all core contracts and utilities in one place ensures the application is easy to maintain and scale.
- **Reusability**: By defining contracts and extensions here, you avoid repeating code throughout the application.
- **Error Handling and Performance**: The use of middleware like `ExceptionMiddleware` and `PermissionsMiddleware` ensures that errors and security concerns are handled centrally.
  
### How to Use:
- **Contracts**: Implement these contracts in your services for functionality like caching and token management.
- **Middlewares**: Plug these middlewares into your `Startup.cs` for error handling, logging, and permissions checks.
- **Extensions**: Use utility methods in the `Extentions` folder for common functionality, like generating random numbers or formatting strings.

---

## KH.Domain

### Description:
`KH.Domain` contains the core domain entities and business logic, representing the real-world models. This layer is critical as it captures the core concepts and rules of the business, ensuring that they remain the same regardless of how the technology evolves.

### Files:

- **Commons/**:
    - `BaseEntity.cs`: A base class for all entities, providing common fields like `Id`, `CreatedDate`, and `ModifiedDate`.
    - `HistoryTrackerEntity.cs`: A specialized entity that tracks changes over time, useful for audit logs or versioning.

- **Entities/**:
    - `Customer.cs`: Represents a customer in the domain, with properties such as name, contact details, etc.
    - `Role.cs`: Represents a user role, used to assign permissions to users.
    - `User.cs`: Represents a system user, with fields like `UserId`, `Email`, `PasswordHash`, etc.
    - **Lookups/Entities**: These are predefined entities like `City.cs`, `Group.cs`, and `Nationality.cs`, used in the application to hold constant data for dropdowns and selections.

- **Enums/**:
    - `DepartmentEnum.cs`: Enum representing different departments in the organization.
    - `RoleEnum.cs`: Enum representing different roles in the application (Admin, User, etc.).

### Benefits:
- **Domain-Driven Design**: Ensures that the domain logic is at the center of the application, promoting clean and scalable code.
- **Consistency**: By centralizing all domain logic here, the rules and entities remain consistent across the application.
- **Strong Typing**: Enums provide strong typing, ensuring that only valid values are used in code and improving code readability.

### How to Use:
- **Entities**: Use these domain entities in the `Persistence` layer for data persistence.
- **Enums**: Use enums across services and APIs to represent categorical data consistently.

---

## KH.Dto

### Description:
The `KH.Dto` layer contains **Data Transfer Objects (DTOs)**, which act as the intermediary between the domain and external layers (e.g., APIs). DTOs are used to encapsulate and validate the data sent over the network without exposing internal domain entities.

### Files:

- **Models/**:
    - `AuthenticationLoginRequest.cs`: DTO for login requests, containing fields like `Username` and `Password`.
    - `CustomerSearchRequestDto.cs`: DTO for filtering customer search queries.

- **Response Models**:
    - `UserDetailsResponse.cs`: Contains details of a user, typically returned by the API after a user lookup.
    - `CustomerListResponse.cs`: A paged list of customers, used when returning a filtered set of customer data.

- **Validations**:
    - `UserFormValidator.cs`: Validates the `UserForm` DTO, ensuring that all necessary fields are provided and follow the required format.

- **MappingProfiles**:
    - `DomainToDtoProfile.cs`: AutoMapper profile for mapping domain models to DTOs and vice versa.

### Benefits:
- **Separation of Concerns**: DTOs ensure that internal domain logic is not exposed directly through APIs.
- **Data Validation**: Validation ensures that only valid data reaches the domain logic, preventing errors and ensuring data integrity.
- **Encapsulation**: DTOs help encapsulate complex domain models into simpler data objects suitable for network communication.

### How to Use:
- **Mapping**: Use `AutoMapper` to map between domain models and DTOs in your services or controllers.
- **Validation**: Apply validators in controllers or services to ensure data correctness before processing.
- **Response Models**: Return DTOs from the API instead of domain models to decouple the layers.

---

## KH.PersistenceInfra

### Description:
`KH.PersistenceInfra` contains everything related to database interactions, including database configurations, repositories, and migrations. It abstracts the data persistence logic away from the domain, ensuring a clean separation between the data access and business logic.

### Files:

- **Data/**:
    - `AppDbContext.cs`: Entity Framework Core DbContext that manages the database connection and entity sets.
    - `CustomerConfiguration.cs`: Fluent API configuration for the `Customer` entity, defining table mappings and constraints.
    - `DapperContext.cs`: Context for Dapper, used when performance-critical SQL queries are needed, allowing direct access to stored procedures and raw SQL.

- **Migrations**:
    - `20240721125506_InitialCreate.cs`: An example migration for setting up the initial database schema. Migrations allow version control of your database schema changes.

- **Repositories**:
    - `GenericRepository.cs`: Implements a generic repository pattern, allowing for CRUD operations without directly dealing with the database context.
    - `UnitOfWork.cs`: Manages transactions, ensuring atomic database operations across multiple repositories.

- **Stored Procedures**:
    - `CategoriesStatusReportStored.json`: Contains JSON for a stored procedure, allowing predefined queries to be executed for reports or other specific actions.

### Benefits:
- **Abstraction**: The repository pattern abstracts database access, allowing you to change the persistence layer without affecting the business logic.
- **Transaction Management**: Using the Unit of Work ensures that related database changes are applied atomically.
- **Scalability**: By separating the persistence layer, you can optimize or scale it independently from the business logic.

### How to Use:
- **Repositories**: Implement specific repositories for custom data access needs or use the generic repository for basic CRUD operations.
- **Migrations**: Use Entity Framework migrations to keep the database schema in sync with your models.

---

## KH.Services

### Description:
`KH.Services` contains the business logic for the application. It implements the contracts defined in `KH.Core` and handles operations related to the domain entities.

### Files:

- **Features/**:
    - `UserService.cs`: Implements user-related functionality such as registering users, updating profiles, or managing roles.

- **Service Registration**:
    - `ServicesRegisterationService.cs`: Registers all services with the DI container to ensure that dependencies are resolved correctly in other layers.

### Benefits:
- **Separation of Business Logic**: By keeping the business logic separate from the API and persistence layers, you ensure that the application remains modular and easy to maintain.
- **Testability**: The business logic can be easily unit-tested in isolation from the database and API layers.

### How to Use:
- **Implement Services**: Implement business logic here by consuming domain entities and interacting with repositories.
- **Dependency Injection**: Register services in `ServicesRegisterationService.cs` and consume them in your controllers or other services.

---

## KH.WebApi

### Description:
The `KH.WebApi` layer is responsible for exposing the application’s functionalities via REST APIs. It contains the API controllers and handles incoming HTTP requests, routing them to the appropriate services.

### Files:

- **Controllers/**:
    - `UsersController.cs`: Manages all user-related endpoints such as creating users, fetching user details, etc.
    - `WeatherForecastController.cs`: A sample controller that returns mock weather data.
    - `DemoController.cs`: A demo controller used for showcasing basic functionality.

- **Program.cs**: The entry point of the application, setting up the host and starting the web server.
- **Startup.cs**: Configures the application’s services, middlewares, and routing.
- **appsettings.json**: Contains configuration settings like connection strings, logging, and other environment-specific settings.

### Benefits:
- **Exposes Application Logic**: Allows external systems or users to interact with the application via HTTP requests.
- **Centralized Configuration**: All services and middleware are configured in one place (`Startup.cs`), making the application easier to manage.
- **Flexible Routing and Middleware**: Provides a flexible way to route requests and apply middleware such as authentication and logging.

### How to Use:
- **Add Endpoints**: Define new controllers for different entities and services, adding HTTP methods (GET, POST, PUT, DELETE) for each operation.
- **Configure Middleware**: Add middleware (e.g., CORS, logging, authentication) in `Startup.cs` to manage incoming requests and responses.

---

## Used Packages

Here is a list of key packages used in this project and their purpose:

1. **Entity Framework Core**:
   - Used for Object-Relational Mapping (ORM) to interact with databases.
   - Provides migrations, change tracking, and database querying capabilities.

2. **Dapper**:
   - A lightweight ORM for more efficient SQL queries and database access.
   - Used in performance-critical operations where Entity Framework might be slower.

3. **AutoMapper**:
   - Used to map domain entities to DTOs and vice versa, simplifying data transfer and avoiding boilerplate mapping code.

4. **FluentValidation**:
   - A validation framework used to validate DTOs and requests.
   - Ensures that incoming data is in the correct format before processing.

5. **Swashbuckle (Swagger)**:
   - Used to generate API documentation and provide an interactive API interface.
   - Enables testing and visualization of API endpoints via Swagger UI.

6. **Serilog**:
   - A structured logging library that provides easy-to-read logs, helping to track application behavior and troubleshoot issues.

7. **Newtonsoft.Json**:
   - A powerful JSON library used to serialize and deserialize objects to and from JSON.
   - Commonly used in APIs to format and process JSON requests and responses.

---

## Upcoming Features

We are actively working on new features to improve the architecture and user experience. Here is a list of upcoming features that will be added soon:

1. **Role-Based Access Control (RBAC)**:
   - Enhance user authorization by implementing a full role-based access control system with fine-grained permissions.

2. **Audit Logging**:
   - Implement audit logging to track and log all significant user actions, such as data creation, updates, and deletions.

3. **Rate Limiting**:
   - Add rate-limiting middleware to prevent abuse and manage traffic to the API, ensuring fair usage across all clients.

4. **Caching Improvements**:
   - Implement distributed caching mechanisms like Redis to enhance performance, particularly for high-traffic data endpoints.

5. **Real-Time Notifications**:
   - Introduce WebSocket support for real-time notifications to users on important system events, such as status changes, new messages, etc.

6. **Localization Support**:
   - Add multi-language support, enabling the application to serve content in different languages based on user preferences.

7. **Elastic Search**:
   -Global loggig
   
9. **Permission Management**:
   -
10. **JWT**:
   -
11. **IOption Pattern**: done with file seetings
   -        

11. **Bulk add - update**:
12.  **Lock prop ask yousef**:
   -        
    
   -        
13. **Hangfire**:
   -   

14. **Fluent Email**:

17. ** **:
   -

18. ** **:
   -
 

**Global File management**: done
Users CRUD done 
---

## How to Run

### Prerequisites:
- .NET SDK
- Entity Framework Core (for migrations)
- Database (e.g., SQL Server, PostgreSQL)
  
### Steps to Run:
1. Clone the repository.
2. Configure the connection strings in `appsettings.json` for your database.
3. Apply database migrations by running:
   ```bash
   dotnet ef database update

#### VS 2022 - Convert to file-scoped namespace in all files

   After you have configured the .editorconfig, you can configure a 'Code Cleanup' setting to automatically convert all files to use file-scoped namespace. Go to Tools -> Options -> Text Editor -> Code Cleanup -> Configure Code Cleanup. Then add the 'Apply namespace preferences'. Then go to Analyze -> Code Cleanup (or just search for 'Code cleanup') and run the Code Cleanup to automatically change the namespaces to file-scoped.


   Best answer in my opinion is here: https://www.ilkayilknur.com/how-to-convert-block-scoped-namespacees-to-file-scoped-namespaces

It says that you can change the code-style preference (and enable the display of the option to apply this preference in a document / project / solution) by going to Tools => Options => Text Editor => C#=> Code Style and then changing the related preference.

![image](https://github.com/user-attachments/assets/fd3e99bd-facc-4e49-87d5-cd83f3ce5a0c)


https://blog.joaograssi.com/series/authorization-in-asp.net-core/ 



# Authentication, Authorization, and Permissions Process in This Repository

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

---



# Auditing Process in KnightHedgeArchitecture Repository

The auditing process in the **KnightHedgeArchitecture** repository is designed to track changes to various entities, focusing on capturing the essential details of data modifications. Here's a breakdown of the key components involved in the auditing system:

## 1. Audit Types
Audit events are categorized into three types, as defined in the `AuditType.cs` file:

- **Create**: When a new entity is added to the system.
- **Update**: When an existing entity is modified.
- **Delete**: When an entity is removed from the system.

## 2. Audit Entry
The `AuditEntry` class in the `Audit.cs` file is responsible for holding details about changes made to entities. It captures:

- `UserId`: The identifier of the user who performed the action.
- `TableName`: The name of the table being modified.
- `KeyValues`: The primary key of the affected entity.
- `OldValues`: The previous state of the entity before modification.
- `NewValues`: The new state of the entity after modification.
- `ChangedColumns`: Columns that were changed during the action.

The `AuditEntry` also converts its state to a more general `Audit` entity for storage, including serializing changes and timestamps.

## 3. Audit Response
The `AuditResponse.cs` file defines the structure of an audit record that will be returned in responses. It includes:

- `UserId`: The user responsible for the action.
- `Type`: The type of audit event (Create, Update, Delete).
- `TableName`: The affected table.
- `OldValues` and `NewValues`: The values before and after the change.
- `AffectedColumns`: The columns that were modified.
- `PrimaryKey`: The primary key of the entity involved.

## 4. Audit Service
The `IAuditService.cs` file provides services for retrieving and exporting audit trails:

- `GetCurrentUserTrailsAsync`: Retrieves audit logs related to a specific user.
- `ExportToExcelAsync`: Exports audit logs to an Excel file, with options to filter by search terms, including old or new values.

## 5. Audit Table Migration
The `audittable.cs` migration file creates a database table named `AuditTrails`, which stores the audit records. The table contains fields for storing:

- `UserId`: The user who performed the action.
- `Type`: The type of the audit (Create, Update, Delete).
- `TableName`, `OldValues`, `NewValues`, `AffectedColumns`, and `PrimaryKey`: Storing detailed information about the audit event.

## Conclusion
The auditing process in this repository is designed to provide detailed tracking of all significant changes to the system. It stores information about what changes were made, who made them, and when they occurred, allowing for transparency and traceability in the system.



# SQL Query Analysis with Entity Framework Core

This repository provides an analysis of SQL statements generated by Entity Framework Core (EF Core) to retrieve data from a relational database. These queries are part of a multi-step process involving user-related data, roles, permissions, groups, and departments. Below is a detailed breakdown of each query and its purpose.

## Overview of Executed Queries

### complex user query using spliting 
  ```  var detailsUserFromDBX = await repository.GetAsync(id,
    q => q.Include(u => u.UserRoles)
           .ThenInclude(ur => ur.Role)
           .ThenInclude(r => r.RolePermissions)
           .ThenInclude(rp => rp.Permission)
           .Include(u => u.UserGroups)
           .ThenInclude(x => x.Group)
           .Include(u => u.UserDepartments)
           .ThenInclude(d => d.Department),
    splitQuery: true); ```

### 1. **User Information**
```sql
exec sp_executesql N'SELECT TOP(1) [u].[Id], [u].[BirthDate], [u].[CreatedById], ... 
                   FROM [Users] AS [u] 
                   WHERE [u].[Id] = @__id_0 
                   ORDER BY [u].[Id]'
```
- **Purpose**: Retrieve user details from the `Users` table based on the user ID.
- **Execution**: Uses `TOP(1)` to limit results to a single user.
- **Impact**: Ensures efficient retrieval by fetching only one record if the user is found.

### 2. **User Roles and Related Roles**
```sql
exec sp_executesql N'SELECT [t0].[Id], [t0].[CreatedById], ... 
                   FROM (...) AS [t] 
                   INNER JOIN (...) AS [t0] ON [t].[Id] = [t0].[UserId] 
                   ORDER BY [t].[Id], [t0].[Id], [t0].[Id0]'
```
- **Purpose**: Fetch roles assigned to the user, joining the `UserRoles` and `Roles` tables.
- **Execution**: Uses an `INNER JOIN` to link users and roles.
- **Impact**: Multiple records may be returned if the user has multiple roles.

### 3. **Role Permissions**
```sql
exec sp_executesql N'SELECT [t1].[Id], [t1].[CreatedById], ... 
                   FROM (...) AS [t] 
                   INNER JOIN (...) AS [t0] 
                   INNER JOIN (...) AS [t1] ON [t0].[Id0] = [t1].[RoleId] 
                   ORDER BY [t].[Id], [t0].[Id], [t0].[Id0]'
```
- **Purpose**: Retrieve permissions associated with the user's roles.
- **Execution**: Joins `RolePermissions` and `Permissions` tables.
- **Impact**: Returns multiple records based on role-to-permission relationships.

### 4. **User Groups**
```sql
exec sp_executesql N'SELECT [t0].[Id], [t0].[CreatedById], ... 
                   FROM (...) AS [t] 
                   INNER JOIN (...) AS [t0] ON [t].[Id] = [t0].[UserId] 
                   ORDER BY [t].[Id]'
```
- **Purpose**: Fetch groups the user belongs to, linking `UserGroups` and `Groups` tables.
- **Impact**: Can return multiple records based on the user's group memberships.

### 5. **User Departments**
```sql
exec sp_executesql N'SELECT [t0].[Id], [t0].[CreatedById], ... 
                   FROM (...) AS [t] 
                   INNER JOIN (...) AS [t0] ON [t].[Id] = [t0].[UserId] 
                   ORDER BY [t].[Id]'
```
- **Purpose**: Retrieve departments associated with the user, using `UserDepartments` and `Departments` tables.
- **Impact**: Can return multiple records depending on the user's departmental associations.

## General Behavior and Performance Considerations

### 1. **Query Splitting**
- EF Core breaks these operations into multiple queries to reduce complexity and improve performance.
- This avoids issues like SQL Server's limitations on the number of joins in a single query.

### 2. **Indexing**
- Ensure that appropriate indexes exist on foreign key columns (`UserId`, `RoleId`, etc.) to further optimize performance.

### 3. **Execution Plan**
- Use SQL Server Management Studio (SSMS) to analyze the execution plan of these queries and identify potential performance bottlenecks.

### 4. **Data Volume Considerations**
- Performance may vary based on the size of your data. For users with many roles, permissions, groups, or departments, query results can become large, potentially affecting serialization and response times.

## Conclusion
The SQL queries generated by EF Core showcase an efficient way to fetch complex relationships in a relational database. By executing queries in a split manner, EF Core optimizes performance while managing intricate data models. Consider adding indexes and analyzing query execution plans for larger datasets.

# SQL Query Analysis: Without Query Splitting

In this query, data is retrieved without query splitting. This means that all related data is fetched in a single, complex query using multiple `LEFT JOIN` and `INNER JOIN` clauses. Below is an explanation of what's happening in this approach.

```
//crazy query example that needs spliting or it will cause performance issues
var detailsUserFromDB = await repository.GetAsync(id,
  q => q.Include(u => u.UserRoles)
  .ThenInclude(ur => ur.Role)
  .ThenInclude(r=> r.RolePermissions).ThenInclude(rp=> rp.Permission)
  .Include(u => u.UserGroups)
  .ThenInclude(x => x.Group)
  .Include(u => u.UserDepartments)
  .ThenInclude(d => d.Department));
```
  
## Query Breakdown

### 1. **Main User Query (Users Table)**
The query starts by retrieving the main user's data from the `Users` table, limiting the result to one record using `TOP(1)` for the user with `Id = @__id_0`.  
This includes basic information such as:
- `BirthDate`
- `Email`
- `FirstName`
- `LastName`
- Other personal and metadata fields

### 2. **User Roles and Role Permissions (Joined via UserRoles and Roles)**
- The first `LEFT JOIN` is used to retrieve the user's roles from the `UserRoles` table.
- It joins the `Roles` table to fetch role details (e.g., `RoleId`, `NameAr`, `NameEn`, etc.).
- Another `LEFT JOIN` fetches permissions associated with roles from the `RolePermissions` and `Permissions` tables. This allows the query to fetch permissions assigned to the user's roles in the same operation.

### 3. **User Groups (Joined via UserGroups and Groups)**
- The second `LEFT JOIN` retrieves the groups that the user belongs to from the `UserGroups` table.
- It joins the `Groups` table to fetch additional information about the group, such as the group's `Name` and `Description`.

### 4. **User Departments (Joined via UserDepartment and Departments)**
- The third `LEFT JOIN` retrieves departments associated with the user from the `UserDepartment` table.
- It joins the `Departments` table to fetch information like the department's `Name`, `Description`, and other metadata.

## Key Aspects of This Approach (Without Query Splitting)

### 1. **Single Query Execution**
- All necessary data (user, roles, role permissions, groups, departments) is retrieved in a **single large query**.
- This avoids multiple round-trips to the database, retrieving everything in one go.

### 2. **JOINs Complexity**
- The query uses multiple `LEFT JOIN` and `INNER JOIN` clauses to retrieve related data from different tables (e.g., `UserRoles`, `Roles`, `RolePermissions`, `Permissions`, `UserGroups`, `Departments`).
- The number of `JOINs` can become quite large, increasing query complexity.

### 3. **Potential Performance Issues**
- **Performance Impact**: Retrieving all related data in a single query can degrade performance if the dataset is large or there are many related entities (e.g., the user has many roles or permissions).
- **Data Redundancy**: The query might retrieve redundant data. For example, if the user has multiple roles, each role's data will be repeated along with the user's base data, resulting in a larger result set.
- **SQL Server Limits**: Extremely complex queries may hit SQL Server limits (such as the number of joins or query size) and could further degrade performance.

### 4. **Result Set Duplication**
- The user's base information (from the `Users` table) will be repeated for each related record (e.g., for each role or group), leading to a larger result set and potential duplication of data.

## Comparison to Query Splitting

### 1. **Query Splitting**
- **Approach**: In query splitting, multiple smaller queries are executed. Each query retrieves only specific related data (e.g., first fetching the user, then roles, then permissions, etc.).
- **Benefit**: Each query is simpler and more efficient for its specific purpose, though it may involve more round-trips to the database.

### 2. **Without Query Splitting (This Approach)**
- **Approach**: All data is retrieved in one large, complex query, minimizing database round-trips but increasing the complexity of the SQL query.
- **Performance**: This can lead to performance bottlenecks, especially with larger datasets and more relationships between entities.

## Conclusion
This approach, without query splitting, retrieves all related data in one large query using `LEFT JOIN` and `INNER JOIN` clauses. While it reduces the number of database round-trips, it can result in performance issues and data duplication, especially as the dataset grows larger. For complex entity relationships, it is generally recommended to use query splitting to improve performance and manageability.

---

## License
[MIT](LICENSE)
