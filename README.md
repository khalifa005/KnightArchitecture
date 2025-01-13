<h2 class="mb-3" align="center"> { وَقُلْ رَبِّ زِدْنِي عِلْمًا } </h2>

<p align="center">⭐⭐ When imagination meets innovation, dreams become reality. ⭐⭐</p>

<table>
  <tr>
    <td align="center" style="width: 100%; max-width: 300px; padding: 10px;">
      <img src="https://github.com/user-attachments/assets/cf93aafa-4192-49c7-8228-471aa979b43c" style="width: 100%; height: auto;" alt="Top"><br>
      <b>V1 Primary knight</b>
    </td>
    <td align="center" style="width: 100%; max-width: 300px; padding: 10px;">
      <img src="https://github.com/user-attachments/assets/faba266c-3eba-4fce-afec-ccd04f2efe3b" style="width: 100%; height: auto;" alt="a94ac067-c12c-4357-86f0-62d2cdb8652f"><br>
      <b>V2 Moon knight</b>
    </td>
    <td align="center" style="width: 100%; max-width: 300px; padding: 10px;">
      <img src="https://github.com/user-attachments/assets/1bb1b5c9-73fd-4aa5-9a2e-02b0be9417a0" style="width: 100%; height: auto;" alt="Arabian Knight"><br>
      <b>V3 Arabian knight</b>
    </td>
  </tr>
</table>

## Table of Contents
1. [Knight Template Overview](#knight-template-overview)
2. [Aim and Purpose](#aim-and-purpose)
3. [Why KnightHedgeArchitecture?](#why-knighthedgearchitecture)
4. [Design Approach](#design-approach)
5. [Features](#features)
6. [Screenshots](#screenshots)
7. [Used Packages](#used-packages)
8. [Upcoming in Version 2: Ultimate Knight Hedge](#upcoming-in-version-2-ultimate-knight-hedge)
9. [Postman collection](#postman-collection)
10. [Getting Started](#getting-started)
11. [Contact owner](#contact-owner)


## Knight Template Overview

The `KnightArchitecture` template is a comprehensive, modular solution for developers looking to accelerate the development of enterprise-grade applications. This template provides a solid foundation of essential building blocks such as authentication, authorization, auditing, caching, background jobs, and real-time notifications. Each component is crafted with simplicity and scalability in mind, allowing developers to focus on core business logic rather than setting up repetitive foundational features.

---

## Aim and Purpose

The primary aim of this template is to **save developers time and effort** by offering a fully functional, extensible starting point for any new project. By integrating commonly used services and best practices, `KnightHedgeArchitecture` allows teams to jumpstart their development process while maintaining a focus on best practices. Whether building a new API, a web service, or a complex enterprise application, this template provides a reliable, efficient backbone that’s both flexible and easy to extend.

### Key Goals:
- **Ease of Use**: This template follows a simple and intuitive design approach, making it easy to understand, set up, and extend.
- **Modularity**: Each building block (e.g., authentication, caching, auditing) is a self-contained module, allowing developers to pick and use components as needed.
- **Scalability**: Built with scalability in mind, this template is suitable for applications of all sizes, from small projects to large, enterprise-grade solutions.
- **Standardization**: Provides a structured approach that standardizes code patterns and promotes maintainability, reducing technical debt over time.

## Why KnightHedgeArchitecture?

Setting up the foundational components of an application can be repetitive and time-consuming. The `KnightHedgeArchitecture` template handles these essentials, enabling developers to:
- **Accelerate Development**: Quickly get up and running with a well-designed foundation, eliminating the need to set up repetitive boilerplate code.
- **Implement Best Practices**: Adhere to best practices in security, scalability, and modular design, which are critical in production-grade applications.
- **Focus on Business Logic**: By abstracting core services like authentication, auditing, and error handling, developers can concentrate on implementing business-specific features.
- **Improve Collaboration**: The standardized structure allows team members to understand and contribute to the codebase more efficiently, with clearly defined modules and services.

## Design Approach

The `KnightHedgeArchitecture` template is built with simplicity and readability at its core. Every feature follows a straightforward approach, ensuring that the codebase is easy to understand and modify. This approach makes it ideal for developers of varying experience levels and ensures that new team members can quickly get up to speed.

### Core Design Principles:
- **Single Responsibility Principle**: Each service, module, and class focuses on a single responsibility, promoting code clarity and ease of maintenance.
- **Plug-and-Play**: Each building block is designed to be independent, meaning developers can include or exclude modules without affecting the core functionality.
- **Configuration-Driven**: Using configuration files, developers can quickly set up or modify features (e.g., caching, logging, background jobs) without diving into the codebase.
- **Separation of Concerns**: The codebase is organized into layers (e.g., Domain, Application, Infrastructure), ensuring a clean separation between core logic, infrastructure dependencies, and API logic.

---

## Features

### 1. Authentication and Authorization
- **Dual Authentication (JWT & Basic)**: Enables flexible identity verification.
- **Role-Based Access Control**: Manages user access with roles and permissions.
- **Permission Middleware**: Enforces granular access control on endpoints.

 [![View Documentation](https://img.shields.io/badge/View%20Documentation-008CBA?style=for-the-badge&logo=github&logoColor=white)](https://github.com/khalifa005/KnightHedgeArchitecture/blob/master/Documentation/Authentication_README.md)

### 2. Customizable API Endpoint Security
- **Permission-Based Protection**: Implements `PermissionAuthorizeAttribute` for secure API endpoints.
- **Dynamic Policies**: Allows configurable policies based on role and permission changes without modifying code.

### 3. User and Role Management
- **User Registration and Profile Management**: Enables user sign-up, profile updates, and role assignment.
- **Role and Permission Management**: Admins can dynamically manage roles and permissions.

### 4. Auditing and Activity Logging
- **Change Tracking**: Logs create, update, and delete actions with detailed record history.
- **Excel Export of Logs**: Allows exporting audit trails for compliance and review purposes.
  
[![View Documentation](https://img.shields.io/badge/View%20Documentation-008CBA?style=for-the-badge&logo=github&logoColor=white)](https://github.com/khalifa005/KnightHedgeArchitecture/blob/master/Documentation/Auditing_README.md)

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
  
[![View Documentation](https://img.shields.io/badge/View%20Documentation-008CBA?style=for-the-badge&logo=github&logoColor=white)](https://github.com/khalifa005/KnightHedgeArchitecture/blob/master/Documentation/Notification_README.md)

### 9. Background Job Scheduling
- **Quartz.NET and Hangfire Integration**: Manages scheduled jobs and supports retries.
- **Automated Tasks**: Supports tasks like email dispatch and SMS notifications.
  
[![View Documentation](https://img.shields.io/badge/View%20Documentation-008CBA?style=for-the-badge&logo=github&logoColor=white)](https://github.com/khalifa005/KnightHedgeArchitecture/blob/master/Documentation/BackgroundJobs_README.md)

### 10. PDF Exporting
- **DinkToPdf Integration**: Provides PDF generation capabilities.
- **Automated Export**: Supports transactional documents and custom reports.
- **Dynamic PDF Generation**: Create PDFs using predefined HTML templates and dynamic placeholders.
- **Invoice Generation**: Generate invoices with multi-language support and localized content.
- **PDF Merging**: Combine multiple PDFs into a single document.
- **Flexible Libraries**: Supports multiple tools like QuestPDF, PDFsharp, and NReco for diverse use cases.
- **QuestPDF**: For building complex layouts with precise control over content styling.
- **PDFsharp**: For merging and manipulating PDF documents.
- **NReco.PdfGenerator**: For converting raw HTML into PDF files.

[![View Documentation](https://img.shields.io/badge/View%20Documentation-008CBA?style=for-the-badge&logo=github&logoColor=white)](https://github.com/khalifa005/KnightArchitecture/blob/master/Documentation/Pdf_README.md)

### 11. Excel Export and Import
- **Data Export**: Allows offline reporting and analysis.
- **Bulk Data Import**: Supports audit logs and user lists, maintaining a consistent format with customizable templates.

### 12. Redis and In-Memory Caching
- **StackExchange.Redis Integration**: Distributed caching to reduce database load.
- **Flexible Caching**: Enhances data retrieval efficiency and performance.
  
[![View Documentation](https://img.shields.io/badge/View%20Documentation-008CBA?style=for-the-badge&logo=github&logoColor=white)](https://github.com/khalifa005/KnightHedgeArchitecture/blob/master/Documentation/Caching_README.md)

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
  
[![View Documentation](https://img.shields.io/badge/View%20Documentation-008CBA?style=for-the-badge&logo=github&logoColor=white)](https://github.com/khalifa005/KnightHedgeArchitecture/blob/master/Documentation/FileManagement_README.md)

### 17. Real-Time SignalR Hub Integration
- **Push Notifications**: Provides real-time updates using SignalR.
- **Compatibility**: Supports WebSockets and Long Polling.

### 18. Cross-Origin Resource Sharing (CORS)
- **Configurable CORS Policies**: Allows secure cross-domain interactions based on origin.

### 19. Advanced Entity Framework Generic repo
1. **Simplified CRUD Operations**: Standard methods for Create, Read, Update, and Delete make data interactions consistent across all entities.
2. **Reusability and Maintainability**: A single repository for all entities means less code duplication, reduced errors, and easy maintenance.
3. **Enhanced Performance with Caching**: Built-in support for caching frequently accessed data reduces load on the database.
4. **Advanced Querying**: Supports complex filters, eager loading, pagination, and projections, offering flexibility for data retrieval.
[![View Documentation](https://img.shields.io/badge/View%20Documentation-008CBA?style=for-the-badge&logo=github&logoColor=white)](https://github.com/khalifa005/KnightHedgeArchitecture/blob/master/Documentation/EntityFrameworkGenericRepo_README.md)

---

<p align="center">
  <img src="https://github.com/user-attachments/assets/159a3ae0-72be-48d6-99fc-c9d429ed8ee7" alt="v2 - Copy" width="300" height="300" />
</p>


## Screenshots

This section provides visual examples of key output formats in KnightHedgeArchitecture, including PDF exports, email templates, and SMS notifications.

### PDF Export

Below is an example of a generated PDF document for transactional records or user reports. This PDF is generated using **DinkToPdf**, ensuring customized layouts and consistency across exports.
![image](https://github.com/user-attachments/assets/13cd6047-395c-42c3-9877-37451e8b5282)

### Email Template

Here’s a sample email sent through the **Email Service**, utilizing Razor templates for a professional, branded appearance. Emails can be customized for different notifications, such as welcome emails, alerts, or updates.

![image](https://github.com/user-attachments/assets/9dc4ee9c-9578-4866-86bd-82a54b7a7004)


### SMS Notification

Example SMS message generated for alerts and notifications. SMS notifications are template-based, allowing easy customization for different types of messages.

![image](https://github.com/user-attachments/assets/8d9c96a7-4884-4326-8eb3-b448e98a5a57)


### Auto Ef core Auditing 

Example Auditing data generated automatically by ef core.

![image](https://github.com/user-attachments/assets/2f862e4e-074a-4bb3-9ac7-33cba1f36d65)

### Angular client app
![image](https://github.com/user-attachments/assets/cd0a3a2e-64cb-40c8-9c1f-369e8c846031)

### Docker support
![docker-angular-core-banner](https://github.com/user-attachments/assets/03921328-0a03-42b7-aa04-a8acae23a5ff)


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
## Upcoming in Version 2: Ultimate Moon knight
<p align="center">
  <img src="https://github.com/user-attachments/assets/faba266c-3eba-4fce-afec-ccd04f2efe3b" width="300" height="300" alt="a94ac067-c12c-4357-86f0-62d2cdb8652f" alt="v2 - Copy" width="300" height="300" />
</p>
The upcoming version, **Ultimate Knight Hedge**, introduces new, trending .NET libraries and tools to enhance functionality, improve performance, and simplify integrations. Below are some key additions planned for version 2:

- **Polly**: Advanced resilience and transient fault-handling library. Polly will enable the application to handle retries, circuit breakers, and fallback mechanisms gracefully, improving system reliability.
- **Keycloak**: For secure authentication and authorization.
- **Camunda**: Powerful process automation and workflow management.
- **Angular Dashboard**: An intuitive, responsive UI for managing application data and operations.
- Escalation   
- **Refit**: A RESTful API client library for .NET. Refit will simplify API integrations, enabling effortless setup of HTTP clients with strongly-typed, interface-based HTTP requests.
  
- **Fluent Assertions**: A powerful assertion library for .NET, designed to improve the readability and maintainability of tests. Fluent Assertions will be used to write expressive, readable test assertions, making unit testing more efficient and reliable.
  
- **GitHub Actions**: Built-in CI/CD support using GitHub Actions for automated testing, deployment, and monitoring. Version 2 will provide pre-configured GitHub Action workflows, making continuous integration and deployment simpler and faster.
  
- **Rate Limiting**: Improved rate-limiting controls to manage API consumption, preventing abuse and ensuring fair resource usage. This will provide better control over API traffic and reduce the risk of service overload. done

- **SignalR chat**: realtime chat. done
- 
- **Health check**: Improved monitoring for app status.
- **Advanced looging**: Improved monitoring for app performance and sending emails to it department  is slow performance detected.

- **Auto Escalation**: Automatically escalate tasks or incidents based on predefined rules and timelines, ensuring timely responses and efficient workflow management.

- **Enhanced Modular Architecture**: With additional extensions, such as better support for microservices and domain-driven design (DDD) patterns, Ultimate Knight Hedge will allow for more fine-grained customization and scalability across services.

---

## Postman collection 

All API found here.

 [![View Documentation](https://img.shields.io/badge/View%20Documentation-008CBA?style=for-the-badge&logo=github&logoColor=white)](https://github.com/khalifa005/KnightHedgeArchitecture/blob/master/Documentation/KnightHedge.postman_collection.json)

![image](https://github.com/user-attachments/assets/b938fdf6-0d56-4b05-bffd-70ee31a99158)

---
## Contact owner
[<img src='https://cdn.jsdelivr.net/npm/simple-icons@3.0.1/icons/linkedin.svg' alt='linkedin' height='40'>](https://www.linkedin.com/in/mahmoud-khalifa-643936138/)  

## Getting Started

### Prerequisites:
- .NET SDK
- Entity Framework Core (for migrations)
- Database (e.g., SQL Server, PostgreSQL)
Using the `KnightHedgeArchitecture` template is simple:

1. **Clone the Repository**: Download the repository and open it in your IDE.
2. **Configure Dependencies**: Modify configuration files (`appsettings.json`) for services like database, caching, email, and SMS.
3. **Apply database migrations**: by running: Update-database -  dotnet ef database update
4. **Enable/Disable Modules**: Decide on the modules you need and integrate them as required.
5. **Build Your Features**: Start building your application on top of a robust and reliable architecture.

---

## License
[MIT](LICENSE)

### All images logos generated by mahmoud khalifa using DALL AI model 
![awesome](https://github.com/user-attachments/assets/f0217ae4-105d-482e-8b9c-9ae601b20e80)


## References That Helped Build This Project

1. **[DevOps Pre-requisite Course on KodeKloud](https://learn.kodekloud.com/user/courses/devops-pre-requisite-course?refererPath=%2Fuser%2Flearning-paths%2Fdocker&refererTitle=Docker):**
   This course provides foundational knowledge of Docker and DevOps essentials, crucial for setting up this project.

2. **[Docker Guide for .NET Developers by Code With Mukesh](https://codewithmukesh.com/blog/docker-guide-for-dotnet-developers/):**
   A detailed guide that helped in effectively containerizing .NET applications using Docker.

3. **[Deploy ASP.NET Core Web API to Amazon ECS by Code With Mukesh](https://codewithmukesh.com/blog/deploy-aspnet-core-web-api-to-amazon-ecs/):**
   This tutorial guided the deployment process of the ASP.NET Core Web API on Amazon ECS.

4. **[Microservices Architecture and Implementation on .NET by Udemy](https://www.udemy.com/course/microservices-architecture-and-implementation-on-dotnet/learn/lecture/42551962#questions/21852622):**
   A comprehensive course that influenced the microservices architecture and design patterns implemented in this project.

5. **[postman simulation](https://blog.postman.com/postman-api-performance-testing/):**
   postman-api-performance-testing     

