<p align="center">
  <img src="https://github.com/user-attachments/assets/2dd2ad50-f923-4c2c-900a-b1ada84b15a9" alt="v2 - Copy" width="300" height="300" />
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/159a3ae0-72be-48d6-99fc-c9d429ed8ee7" alt="v2 - Copy" width="300" height="300" />
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


### 1. Dual Authentication Mechanism
   - **Supports JWT and Basic Authentication**: Enables flexible identity verification for both internal and external users.
   - **Secure Access Control**: Provides token-based and session-based authentication, catering to various user roles and requirements.

### 2. Dynamic, Role-Based Permission Management
   - **Middleware-Driven Permissions**: Leverages middleware with `AND/OR` logic, allowing complex, fine-grained control over user access.
   - **Dynamic Policies**: Allows for policy updates without changing core code, ensuring adaptability to evolving business requirements.
   - **Centralized Permission Checks**: Simplifies permission management and improves scalability.

### 3. Auditing and Change Tracking
   - **Tracks User Actions**: Logs modifications with old and new values for compliance and detailed auditing.
   - **Excel Export/Import**: Exports and imports audit logs, making data accessible and easy to analyze.
   - **Customizable Audit Viewing**: Access is permission-controlled, allowing only authorized users to view audit trails.

### 4. Localized Error Handling
   - **Multi-Language Support**: Provides error messages in multiple languages, such as English and Arabic, enhancing accessibility.
   - **User-Friendly Debugging**: Delivers detailed error feedback to simplify troubleshooting and improve the user experience.

### 5. Middleware-Based Access Control
   - **PermissionsMiddleware**: Centralizes access control, enforcing permission checks on each request.
   - **SuperAdmin Bypass**: Allows SuperAdmins to bypass permission checks, streamlining high-level administrative tasks.

### 6. Comprehensive Background Job Support
   - **Quartz.NET Integration**: Supports scheduling and running background jobs like email dispatch and SMS notifications.
   - **Enhanced Automation**: Allows scheduled tasks, improving user engagement and operational efficiency.

### 7. Modular Architecture for Extensibility
   - **Easy Integration of New Modules**: Allows for seamless addition of new features.
   - **Modular Services**: Provides services for caching, file management, auditing, and notifications, simplifying future enhancements.

### 8. Granular and Configurable Caching Support
   - **Redis and In-Memory Caching**: Offers flexible caching options for optimized data storage.
   - **Distributed Caching Support**: Ensures enhanced performance, ideal for scalable environments.

### 9. Advanced Notification System
   - **SMS and Email Notifications**: Manages notifications with customizable templates.
   - **Tracking and Logging**: Logs SMS and email statuses, maintaining a detailed record of communications.

### 10. Excel Export and File Management Services
   - **Data Export to Excel**: Allows users to export reports and audits for offline access.
   - **Secure File Management**: Validates file uploads with format checks, ensuring safe and compliant handling.

### 11. Scalable Localization and Preference Management
   - **User Preferences and Language Support**: Centralizes user preferences and supports multiple languages, making it ideal for global applications.
   - **ServerPreferenceManager**: Centralizes and manages user preferences across services.

### 12. Customizable Controllers and API Endpoint Security
   - **Custom Attributes for Endpoint Protection**: Implements `PermissionAuthorizeAttribute` for fine-grained endpoint security.
   - **Dynamic Permission Policies**: Allows easy customization of API access based on role and permissions.

---

Each of these features contributes to a secure, flexible, and scalable system designed to meet the needs of enterprise-level applications. `KnightHedgeArchitecture` provides the tools necessary for comprehensive user management, auditing, and secure, extensible API development.

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
```

## License
[MIT](LICENSE)
