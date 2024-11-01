# Generic Repository in KnightHedgeArchitecture

## Overview

The `KnightHedgeArchitecture` repository includes an **Entity Framework Generic Repository** that provides a streamlined and consistent approach for data access across the application. By using this repository pattern, the codebase adheres to best practices, maintaining clean separation between business logic and data access. This repository is designed to handle all standard CRUD operations as well as complex queries, making it an **awesome tool** for developers aiming to save time and reduce redundancy in their projects.

The **Generic Repository** pattern, along with the **Unit of Work**, offers a structured approach to managing data operations with transactional support, providing benefits in both code reusability and maintainability.

## Generic Repository Methods and Their Benefits

### 1. Add and Delete Methods
   - **`Add` / `AddAsync`**: Adds a new entity to the database. Asynchronous support allows for non-blocking operations.
   - **`Delete` / `DeleteTracked`**: Deletes an entity, with `DeleteTracked` providing an option to work with tracked entities.
   
   *Benefit*: Simplifies entity creation and deletion, ensuring consistent handling of new data and entity removals across the application.

### 2. Querying and Filtering Methods
   - **`GetQueryable`**: Returns an `IQueryable<T>` that allows for advanced LINQ operations on the dataset.
   - **`FindByAsync`**: Accepts a filter expression to retrieve entities matching specific criteria, with support for eager loading via `Include`.
   - **`GetByExpressionAsync`**: Retrieves a single entity matching a specific expression, with optional tracking for performance.

   *Benefit*: These methods enable efficient querying and filtering with minimal setup, making data retrieval highly customizable and performant.

### 3. Count and Existence Checks
   - **`CountAsync`**: Returns the total count of entities.
   - **`CountByAsync`**: Counts entities based on a specified filter, enabling quick existence checks for data.

   *Benefit*: Great for performance monitoring and validation by providing quick counts without the need to load full datasets.

### 4. Retrieval and Caching
   - **`GetAllAsync`**: Retrieves all entities, with optional caching for lookup-type entities (using `ICacheService`).
   - **`RemoveCache`**: Removes cached data for a specific entity type.

   *Benefit*: Caching frequently accessed data can significantly reduce database load and improve response times, especially for data that changes infrequently.

### 5. Update Methods
   - **`UpdateDetachedEntity`**: Attaches and updates a detached entity, changing its state to `Modified`.
   - **`UpdateFromOldAndNewEntity`**: Updates an entity by copying values from another, useful for handling updates with minimal code.
   - **`UpdateRange`**: Updates multiple entities at once, reducing database round-trips.

   *Benefit*: Ensures efficient updates to the database, providing support for bulk updates and detached entities, which is crucial in performance-critical applications.

### 6. Pagination and Projections
   - **`GetPagedUsingQueryAsync`**: Retrieves data in a paginated format, given a query and pagination parameters.
   - **`GetPagedWithProjectionAsync`**: Provides paginated data with a specific projection, allowing for light-weight data retrieval.

   *Benefit*: Reduces the amount of data transferred for large datasets, enhancing performance for client-side data handling and reporting.

### 7. Batch Operations
   - **`BatchUpdateAsync`**: Performs a batch update on entities that match a filter, utilizing the `ExecuteUpdateAsync` method.
   - **`BatchDeleteAsync`**: Performs a batch deletion on entities matching a filter.

   *Benefit*: Efficiently handles bulk updates and deletions, reducing multiple database calls and improving overall performance.

---

## How to Use

1. **Register the Repository**: The `GenericRepository` is accessed via the `UnitOfWork`, ensuring transactional control over multiple repositories. Simply call `unitOfWork.Repository<TEntity>()` to interact with any entity.
2. **Configure Caching (Optional)**: Enable caching for frequently accessed entities by implementing `ICacheService`, allowing `GetAllAsync` to store data for faster retrieval.

## Example Usage

```csharp
public async Task<IEnumerable<MyEntity>> GetFilteredEntitiesAsync(string filter)
{
    using var unitOfWork = new UnitOfWork(_dbContext, _cacheService);
    var repository = unitOfWork.Repository<MyEntity>();

    // Find entities by a specific filter
    return await repository.FindByAsync(entity => entity.Name.Contains(filter));
}

public async Task<PagedList<MyEntity>> GetPagedEntitiesAsync(int pageNumber, int pageSize)
{
    using var unitOfWork = new UnitOfWork(_dbContext, _cacheService);
    var repository = unitOfWork.Repository<MyEntity>();

    // Use paging
    return await repository.GetPagedUsingQueryAsync(pageNumber, pageSize, repository.GetQueryable());
}
```

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
