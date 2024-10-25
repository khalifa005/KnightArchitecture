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
