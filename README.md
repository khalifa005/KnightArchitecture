

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

## Features

### 1. Authentication and Authorization
- **Dual Authentication (JWT & Basic)**: Enables flexible identity verification.
- **Role-Based Access Control**: Manages user access with roles and permissions.
- **Permission Middleware**: Enforces granular access control on endpoints.

### 2. Customizable API Endpoint Security
- **Permission-Based Protection**: Implements `PermissionAuthorizeAttribute` for secure API endpoints.
- **Dynamic Policies**: Allows configurable policies based on role and permission changes without modifying code.

### 3. User and Role Management
- **User Registration and Profile Management**: Enables user sign-up, profile updates, and role assignment.
- **Role and Permission Management**: Admins can dynamically manage roles and permissions.

### 4. Auditing and Activity Logging
- **Change Tracking**: Logs create, update, and delete actions with detailed record history.
- **Excel Export of Logs**: Allows exporting audit trails for compliance and review purposes.

### 5. Centralized Logging with Serilog
- **Request and Error Logging**: Tracks HTTP requests, responses, and errors for audit and troubleshooting.

### 6. Email Service
- **Single and Bulk Emails**: Supports customized templates for welcome and notification emails.
- **Email Tracking**: Logs email statuses for audits and follow-up.
- **Retry and Scheduling**: Failed emails can be retried or scheduled for future delivery.

### 7. SMS Notification Service
- **Template-based Messaging**: Customizable SMS templates for various alerts.
- **Logging**: Tracks SMS delivery statuses, including failures.

### 8. Advanced Notification System
- **SMS and Email Notifications**: Manages notifications with customizable templates.
- **Tracking and Logging**: Logs SMS and email statuses, maintaining a detailed record of communications.

### 9. Background Job Scheduling
- **Quartz.NET and Hangfire Integration**: Manages scheduled jobs and supports retries.
- **Automated Tasks**: Supports tasks like email dispatch and SMS notifications.

### 10. PDF Exporting
- **DinkToPdf Integration**: Provides PDF generation capabilities.
- **Automated Export**: Supports transactional documents and custom reports.

### 11. Excel Export and Import
- **Data Export**: Allows offline reporting and analysis.
- **Bulk Data Import**: Supports audit logs and user lists, maintaining a consistent format with customizable templates.

### 12. Redis and In-Memory Caching
- **StackExchange.Redis Integration**: Distributed caching to reduce database load.
- **Flexible Caching**: Enhances data retrieval efficiency and performance.

### 13. Localization and Multi-language Support
- **Localized Error Messages**: Supports languages like English and Arabic.
- **User Language Preferences**: Customizable for a personalized experience.

### 14. Modular Architecture
- **Independent Services**: Provides separate modules for notifications, caching, auditing, and file handling.
- **Scalability**: Allows easy addition of new modules and features.

### 15. Error Handling and Validation
- **FluentValidation**: Ensures structured model validation.
- **Localized Error Handling**: Provides language-specific error messages.

### 16. File Management and Storage
- **Secure File Uploads**: Processes files with validation checks.
- **Media Management**: Organizes and retrieves media files efficiently.

### 17. Real-Time SignalR Hub Integration
- **Push Notifications**: Provides real-time updates using SignalR.
- **Compatibility**: Supports WebSockets and Long Polling.

### 18. Cross-Origin Resource Sharing (CORS)
- **Configurable CORS Policies**: Allows secure cross-domain interactions based on origin.

---

## Screenshots

This section provides visual examples of key output formats in KnightHedgeArchitecture, including PDF exports, email templates, and SMS notifications.

### PDF Export

Below is an example of a generated PDF document for transactional records or user reports. This PDF is generated using **DinkToPdf**, ensuring customized layouts and consistency across exports.

![PDF Export Screenshot](link-to-pdf-screenshot.png)

### Email Template

Here’s a sample email sent through the **Email Service**, utilizing Razor templates for a professional, branded appearance. Emails can be customized for different notifications, such as welcome emails, alerts, or updates.

![Email Template Screenshot](link-to-email-screenshot.png)

### SMS Notification

Example SMS message generated for alerts and notifications. SMS notifications are template-based, allowing easy customization for different types of messages.

![SMS Notification Screenshot](link-to-sms-screenshot.png)


### Auto Ef core Auditing 

Example SMS message generated for alerts and notifications. SMS notifications are template-based, allowing easy customization for different types of messages.

![SMS Notification Screenshot](link-to-sms-screenshot.png)



## Used Packages

Here is a list of key packages used in this project and their purpose:

### 1. Entity Framework Core

- **Purpose**: Provides Object-Relational Mapping (ORM) to interact with databases.
- **Features**: Supports migrations, change tracking, and database querying capabilities, making database operations simpler and more organized.

### 2. Dapper

- **Purpose**: Lightweight ORM for more efficient SQL queries and database access.
- **Usage**: Used in performance-critical operations where Entity Framework might be slower, enabling rapid and optimized data retrieval.

### 3. AutoMapper

- **Purpose**: Simplifies object-to-object mapping, reducing boilerplate code.
- **Usage**: Maps domain entities to Data Transfer Objects (DTOs) and vice versa, used across services and entities to enhance modularity and maintainability.

### 4. FluentValidation

- **Purpose**: Validation framework used to enforce data integrity.
- **Usage**: Validates DTOs and requests, ensuring that incoming data conforms to expected formats in API controllers and services.

### 5. Swashbuckle (Swagger)

- **Purpose**: Generates interactive API documentation.
- **Usage**: Provides a visual interface for API endpoints via Swagger UI, allowing for easy testing and documentation of API functions.

### 6. Serilog

- **Purpose**: Structured logging for easy-to-read logs.
- **Usage**: Logs application behavior and helps with troubleshooting, enhancing error tracking and performance monitoring.

### 7. Newtonsoft.Json

- **Purpose**: JSON library for serialization and deserialization.
- **Usage**: Formats and processes JSON requests and responses, crucial for APIs that handle JSON data.

### 8. Quartz.NET

- **Purpose**: Robust job scheduling library.
- **Usage**: Enables background job scheduling, allowing automation of repetitive tasks and scheduled processing.

### 9. DinkToPdf

- **Purpose**: PDF generation library.
- **Usage**: Used for creating and customizing PDF documents, particularly in reporting and documentation modules.

### 10. StackExchange.Redis

- **Purpose**: Redis cache support for distributed caching.
- **Usage**: Reduces database load and improves performance by caching frequently accessed data.

### 11. Hangfire

- **Purpose**: Background job processing library.
- **Usage**: Manages tasks like email dispatching, SMS notifications, and other background processes, making asynchronous task handling efficient.

---
## Upcoming in Version 2: Ultimate Knight Hedge
<p align="center">
  <img src="https://github.com/user-attachments/assets/2dd2ad50-f923-4c2c-900a-b1ada84b15a9" alt="v2 - Copy" width="300" height="300" />
</p>
The upcoming version, **Ultimate Knight Hedge**, introduces new, trending .NET libraries and tools to enhance functionality, improve performance, and simplify integrations. Below are some key additions planned for version 2:

- **Polly**: Advanced resilience and transient fault-handling library. Polly will enable the application to handle retries, circuit breakers, and fallback mechanisms gracefully, improving system reliability.
  
- **Refit**: A RESTful API client library for .NET. Refit will simplify API integrations, enabling effortless setup of HTTP clients with strongly-typed, interface-based HTTP requests.
  
- **Fluent Assertions**: A powerful assertion library for .NET, designed to improve the readability and maintainability of tests. Fluent Assertions will be used to write expressive, readable test assertions, making unit testing more efficient and reliable.
  
- **GitHub Actions**: Built-in CI/CD support using GitHub Actions for automated testing, deployment, and monitoring. Version 2 will provide pre-configured GitHub Action workflows, making continuous integration and deployment simpler and faster.
  
- **Rate Limiting**: Improved rate-limiting controls to manage API consumption, preventing abuse and ensuring fair resource usage. This will provide better control over API traffic and reduce the risk of service overload.
  
- **Enhanced Modular Architecture**: With additional extensions, such as better support for microservices and domain-driven design (DDD) patterns, Ultimate Knight Hedge will allow for more fine-grained customization and scalability across services.

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
