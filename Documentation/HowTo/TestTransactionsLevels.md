

# Table of Contents

1. [Locking and Isolation Levels in Entity Framework](#locking-and-isolation-levels-in-entity-framework)
2. [Key Concepts](#key-concepts)
3. [Optimistic Concurrency Control Example](#optimistic-concurrency-control-example)
4. [Read Uncommitted Isolation Level Example](#read-uncommitted-isolation-level-example)
5. [Read Committed Isolation Level Example](#read-committed-isolation-level-example)
6. [Repeatable Read Isolation Level Example](#repeatable-read-isolation-level-example)
7. [Handling Deadlocks with IsolationLevel.Serializable and Explicit Lock Hints in EF Core](#Serializable-IsolationLevel)

---
# Locking and Isolation Levels in Entity Framework

## Overview
This repository provides a detailed implementation and explanation of locking and isolation levels in a database context using Entity Framework and SQL Server. The implementation focuses on managing concurrent transactions effectively by leveraging different isolation levels and locking mechanisms.

## Key Concepts

### Isolation Levels
1. **Read Uncommitted**: Allows dirty reads, non-repeatable reads, and phantom reads.
2. **Read Committed**: Prevents dirty reads but allows non-repeatable reads and phantom reads.
3. **Repeatable Read**: Prevents dirty reads and non-repeatable reads but allows phantom reads.
4. **Serializable**: Prevents dirty reads, non-repeatable reads, and phantom reads.

### Locking Mechanisms
- **Shared Lock (`S`)**: Allows reading but prevents writes until the lock is released.
- **Update Lock (`U`)**: Prevents other transactions from acquiring locks that would conflict with an update.
- **Exclusive Lock (`X`)**: Prevents all other access (read/write) to the locked resource.
- **Key-Range Locks**: Prevents phantom reads by locking ranges of keys.

---

# Optimistic Concurrency Control Example

This section demonstrates optimistic concurrency control using version or timestamp fields. This approach ensures that multiple users can update the same data independently, and any conflicts are checked at the time of saving the data.

## Description

The following example illustrates how to handle concurrency conflicts in EF Core. The method `OptimisticConcurrencyAsync` demonstrates how changes to the same record are handled when another process modifies the record during an active transaction.

#### Current Record:

| Id  | NameAr       | NameEn       | Description               | RowVersion           |
|-----|--------------|--------------|---------------------------|----------------------|
| 10  | وكيل عمل     | Agent user   | empty   | 0x00000000000167EE   |


### How to Test

1. Execute the `OptimisticConcurrencyAsync` method in your application and set break point after the role selection.

### Code Implementation

```csharp
/// <summary>
/// Multiple users can update the same data independently, and conflicts are checked at the time of saving the data.
/// Demonstrates optimistic concurrency control using version or timestamp fields.
/// </summary>
public async Task<string> OptimisticConcurrencyAsync(long roleId, CancellationToken cancellationToken)
{
    // Begin a transaction
    await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

    var existingEntity = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == 10, cancellationToken: cancellationToken);
    if (existingEntity == null)
    {
        return $"Role with id:{roleId} does not exist.";
    }

    try
    {
        existingEntity.Description = "Test Optimistic Concurrency from code";
        // EF Core automatically checks for concurrency conflicts and updates the RowVersion.
        // By comparing the RowVersion or Timestamp field in the database with the one initially read.

        await _dbContext.SaveChangesAsync();

        // Commit the transaction
        await transaction.CommitAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        // If a concurrency conflict is detected, the transaction is rolled back and an exception is thrown.
        await transaction.RollbackAsync();
        throw new InvalidOperationException("Concurrency conflict detected. Another user may have updated this record.");
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        throw new Exception("An error occurred during the transaction.", ex);
    }

    return $"Role with id:{existingEntity.Id} has been updated inside transaction with Optimistic Concurrency";
}
```

2. Open SQL Server Management Studio (SSMS).
3. Execute the following query to simulate another process modifying the record but after :

```sql
USE [KnightTemplateDb]
GO

UPDATE Roles
SET Description = 'Updated from SQL Server'
WHERE Id = 10;
GO
```

#### Current Record after db modification will be:
![image](https://github.com/user-attachments/assets/1b4efe05-2723-4b7c-b7d6-a9f5c7d19b65)
- **now continue the code flow SaveChangesAsync:** a `DbUpdateConcurrencyException` will be thrown "errorMessage": "Concurrency conflict detected. Another user may have updated this record the version is diffrent".
![image](https://github.com/user-attachments/assets/e9360f65-a097-49f6-8ac4-1aa089f3f22e)
- **now run the code again without making direct db update:** u will get a response (Role with id:10 has been updated inside transaction with Optimistic Concurrency).
  
![image](https://github.com/user-attachments/assets/34f5e827-de1e-4392-9827-acdeb92ecd7a)

- **Generated SQL Query once we continue to (on SaveChangesAsync):** 
```sql
UPDATE Roles
SET
    Description = 'Test Optimistic Concurrency from code',
    RowVersion = @newRowVersion
WHERE
    Id = 10 AND RowVersion = @currentRowVersion;

``` 


# Read Uncommitted Isolation Level Example

This section demonstrates how to use the `ReadUncommitted` isolation level to allow dirty reads, non-repeatable reads, and phantom reads. This approach ensures that data can be read and updated without requiring a commit, enabling maximum concurrency at the expense of potential inconsistencies.

## Description

The following example illustrates how the `ReadUncommitted` isolation level works in EF Core. The method `ReadUncommittedIsolationLevelAsync` demonstrates how one transaction can read uncommitted changes made by another transaction, which can lead to dirty reads.

#### Current Record:

| Id  | NameAr       | NameEn       | Description               | RowVersion           |
|-----|--------------|--------------|---------------------------|----------------------|
| 10  | وكيل عمل     | Agent user   | empty                    | 0x00000000000167EE   |

### How to Test

1. Execute the `ReadUncommittedIsolationLevelAsync` method in your application and set a breakpoint before the `SaveChangesAsync` call.

### Code Implementation

```csharp
/// <summary>
/// Allows dirty reads, non-repeatable reads, and phantom reads.
/// Demonstrates the Read Uncommitted isolation level in EF Core.
/// </summary>
public async Task<string> ReadUncommittedIsolationLevelAsync(CancellationToken cancellationToken)
{
    // Begin a transaction
    await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

    var existingEntity = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == 10, cancellationToken: cancellationToken);

    try
    {
        if (existingEntity != null)
        {
            existingEntity.Description = "Dirty Read - Read Uncommitted level from code";
            await _dbContext.SaveChangesAsync();
            // Another transaction with ReadUncommitted isolation level can read this entity with the updated value
            // even if the current transaction is not yet committed.
        }

        // Commit the transaction
        await transaction.CommitAsync();
    }
    catch (Exception ex)
    {
        // Rollback the transaction in case of an exception
        await transaction.RollbackAsync();

        // Handle the exception (log, rethrow, etc.)
        throw new Exception("An error occurred during the transaction.", ex);
    }

    return $"Role with id:{existingEntity.Id} has been updated inside transaction with Read Uncommitted Isolation Level";
}
```

2. Open SQL Server Management Studio (SSMS).
3. Before the current transaction commits, execute the following query in a new transaction to simulate a dirty read:

```sql
USE [KnightTemplateDb]
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
GO

SELECT * FROM Roles WHERE Id = 10;
GO
```

#### Current Record after executing the `SaveChangesAsync` but before committing the transaction:

| Id  | NameAr       | NameEn       | Description                                   | RowVersion           |
|-----|--------------|--------------|-----------------------------------------------|----------------------|
| 10  | وكيل عمل     | Agent user   | Dirty Read - Read Uncommitted level from code | 0x00000000000167EE   |

- **Dirty Read Observation:** The updated value for `Description` will be visible to other transactions with the `ReadUncommitted` isolation level, even though the current transaction hasn't been committed yet.

#### SQL Output After Committing

4. Commit the transaction by allowing the `transaction.CommitAsync` to execute in the code. Run the following query to verify the changes:

```sql
USE [KnightTemplateDb]
GO

SELECT * FROM Roles WHERE Id = 10;
GO
```

- **Result:** The updated value for `Description` is now visible to all isolation levels.

#### Generated SQL Query:

```sql
UPDATE Roles
SET
    Description = 'Dirty Read - Read Uncommitted level from code'
WHERE
    Id = 10;
```

### Notes

- If another process or transaction with a higher isolation level (e.g., `ReadCommitted`) tries to access the record before the commit, it won't see the uncommitted changes.
- Transactions using the `ReadUncommitted` isolation level can result in inconsistent or incomplete data, making it suitable for scenarios where performance is prioritized over data accuracy.

By following these steps, you can observe how the `ReadUncommitted` isolation level allows other transactions to read uncommitted changes and how the database state reflects these changes post-commit.


# Read Committed Isolation Level Example

This section demonstrates how to use the `ReadCommitted` isolation level to prevent dirty reads while allowing non-repeatable reads and phantom reads. This ensures that data modifications within a transaction are not visible to other transactions until the transaction is committed.

## Description

The following example illustrates how the `ReadCommitted` isolation level works in EF Core. The method `ReadCommittedIsolationLevelAsync` demonstrates how changes to a record are isolated from other transactions until the current transaction is committed.

#### Current Record:

| Id  | NameAr       | NameEn       | Description               | RowVersion           |
|-----|--------------|--------------|---------------------------|----------------------|
| 10  | وكيل عمل     | Agent user   | empty                    | 0x00000000000167EE   |

### How to Test

1. Execute the `ReadCommittedIsolationLevelAsync` method in your application and set a breakpoint before the `transaction.CommitAsync` call.

### Code Implementation

```csharp
/// <summary>
/// Prevents dirty reads but allows non-repeatable reads and phantom reads.
/// Demonstrates the Read Committed isolation level in EF Core.
/// </summary>
public async Task<string> ReadCommittedIsolationLevelAsync(long role, CancellationToken cancellationToken)
{
    // Begin a transaction
    await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

    var existingEntity = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == 10, cancellationToken: cancellationToken);

    try
    {
        if (existingEntity != null)
        {
            existingEntity.Description = "Test ReadCommitted level from code";
            await _dbContext.SaveChangesAsync();
        }

        // Commit the transaction
        await transaction.CommitAsync();
    }
    catch (Exception ex)
    {
        // Rollback the transaction in case of an exception
        await transaction.RollbackAsync();

        // Handle the exception (log, rethrow, etc.)
        throw new Exception("An error occurred during the transaction.", ex);
    }

    return $"Role with id:{existingEntity.Id} has been updated inside transaction with ReadCommitted Isolation Level";
}
```

2. Open SQL Server Management Studio (SSMS).
3. Before the current transaction commits, execute the following query in a new transaction to check for isolation:

```sql
USE [KnightTemplateDb]
GO

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
GO

SELECT * FROM Roles WHERE Id = 10;
GO
```

#### Current Record after executing the `SaveChangesAsync` and after committing the transaction:

| Id  | NameAr       | NameEn       | Description                                   | RowVersion           |
|-----|--------------|--------------|-----------------------------------------------|----------------------|
| 10  | وكيل عمل     | Agent user   | Test ReadCommitted level from code            | 0x00000000000167EE   |

- **Observation:** Other transactions cannot see the updated value for `Description` until the current transaction is committed except the  READ UNCOMMITTED LEVEL will be able to see the value even before commiting transaction.

#### SQL Output After Committing

4. Commit the transaction by allowing the `transaction.CommitAsync` to execute in the code. Run the following query to verify the changes:

```sql
USE [KnightTemplateDb]
GO

SELECT * FROM Roles WHERE Id = 10;
GO
```

- **Result:** The updated value for `Description` is now visible to all transactions.

#### Generated SQL Query:

```sql
UPDATE Roles
SET
    Description = 'Test ReadCommitted level from code'
WHERE
    Id = 10;
```

### Notes

- Transactions using the `ReadCommitted` isolation level prevent dirty reads but still allow for non-repeatable reads and phantom reads. This level is the default isolation level in most relational databases.
- Using this isolation level is suitable for scenarios requiring a balance between consistency and concurrency.

By following these steps, you can observe how the `ReadCommitted` isolation level ensures isolation of data changes until a transaction is committed and how the database state reflects these changes post-commit.

# Repeatable Read Isolation Level Example

This section demonstrates how to use the `RepeatableRead` isolation level to prevent dirty and non-repeatable reads while allowing phantom reads. This ensures that data read during a transaction cannot be modified by other transactions until the transaction is complete, as it locks the data being read.

## Description

The following example illustrates how the `RepeatableRead` isolation level works in EF Core. The method `RepeatableReadIsolationLevelAsync` demonstrates how consistency is maintained for multiple reads of the same data during a transaction.

#### Current Record:

| Id  | NameAr       | NameEn       | Description               | RowVersion           |
|-----|--------------|--------------|---------------------------|----------------------|
| 10  | وكيل عمل     | Agent user   | empty                    | 0x00000000000167EE   |

### How to Test

1. Execute the `RepeatableReadIsolationLevelAsync` method in your application and set a breakpoint before the `SaveChangesAsync` call.

### Code Implementation

```csharp
/// <summary>
/// Prevents dirty and non-repeatable reads but allows phantom reads.
/// Demonstrates the Repeatable Read isolation level in EF Core.
/// </summary>
public async Task<string> RepeatableReadIsolationLevelAsync(long role, CancellationToken cancellationToken)
{
    // Begin a transaction
    await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

    // First read
    var existingEntity = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == role, cancellationToken: cancellationToken);

    if (existingEntity == null)
    {
        return $"Role with id:{role} does not exist or could not be found.";
    }

    try
    {
        // Simulate additional read to ensure no data has changed.
        var sameRoleEntity = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == role, cancellationToken: cancellationToken);

        if (existingEntity.Description != sameRoleEntity?.Description)
        {
            throw new InvalidOperationException("Non-repeatable read detected.");
        }

        existingEntity.Description = "RepeatableRead Test";
        await _dbContext.SaveChangesAsync();

        // Commit the transaction
        await transaction.CommitAsync();
    }
    catch (Exception ex)
    {
        // Rollback the transaction in case of an exception
        await transaction.RollbackAsync();

        // Handle the exception (log, rethrow, etc.)
        throw new Exception("An error occurred during the transaction.", ex);
    }

    return $"Role with id:{existingEntity.Id} has been updated inside transaction with RepeatableRead Isolation Level";
}
```

2. Open SQL Server Management Studio (SSMS).
3. While the transaction is active, execute the following query in a new transaction to observe isolation:

```sql
USE [KnightTemplateDb]
GO

SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
GO

SELECT * FROM Roles WHERE Id = 10;
GO
```

#### Current Record after executing the first read and before committing:

| Id  | NameAr       | NameEn       | Description                                   | RowVersion           |
|-----|--------------|--------------|-----------------------------------------------|----------------------|
| 10  | وكيل عمل     | Agent user   | empty                                      | 0x00000000000167EE   |

- **Observation:** The same data is guaranteed to be read in subsequent reads within the same transaction. If another transaction attempts to modify the data while the current transaction is active, the update process will be locked until the current transaction completes.

#### SQL Output After Committing

4. Commit the transaction by allowing the `transaction.CommitAsync` to execute in the code. Run the following query to verify the changes:

```sql
USE [KnightTemplateDb]
GO

SELECT * FROM Roles WHERE Id = 10;
GO
```

- **Result:** The updated value for `Description` is now visible to all transactions.

#### Generated SQL Query:

```sql
UPDATE Roles
SET
    Description = 'RepeatableRead Test'
WHERE
    Id = 10;
```

### Notes

- Transactions using the `RepeatableRead` isolation level ensure that the same data read multiple times during the transaction remains consistent and that no other transaction can modify the data until the current transaction completes.
- This isolation level is suitable for scenarios requiring consistent reads but where phantom reads are acceptable.

By following these steps, you can observe how the `RepeatableRead` isolation level maintains consistency for multiple reads within a transaction and how the database state reflects these changes post-commit.


# Serializable IsolationLevel 

## Overview

When using **IsolationLevel.Serializable** in EF Core, it ensures the highest level of isolation and consistency but can lead to **deadlocks** in high-concurrency scenarios. This document describes the behavior of Serializable isolation, the issues that may arise, and solutions to avoid deadlocks using **explicit lock hints**.

Key Characteristics:

- Prevents Dirty Reads:
A transaction cannot read data that has been modified but not yet committed by another transaction.

- Prevents Non-Repeatable Reads:
If a transaction reads a row, no other transaction can modify or delete that row until the first transaction completes.

- Prevents Phantom Reads:
If a transaction queries a set of rows based on a condition, no other transaction can insert new rows that would satisfy that condition until the first transaction completes.

- Locking:
The database locks the data being read or modified for the duration of the transaction, ensuring no other transactions can read or modify it.

- Ensures Full Isolation:
Serializable isolation provides the same results as if all transactions were executed serially (one at a time), even though they may actually execute concurrently.

- Performance Impact:
Due to the high level of locking, it can cause contention, deadlocks, and reduced concurrency, making it less suitable for high-throughput systems.

---
# Handling Deadlocks with IsolationLevel Serializable and Explicit Lock Hints in EF Core

## Problem Description

**Serializable Isolation Level** ensures:
- No other transaction can modify the rows being read until the transaction commits.
- It places strict locks on rows and even range locks to prevent phantom reads.

However, these strict locking rules can cause **deadlocks** in concurrent transactions. Here's an example scenario:

### Scenario
1. **C# Code:** Starts a transaction with `IsolationLevel.Serializable`.
2. Reads a row from the `Roles` table (`SELECT` query).
3. Updates the same row (`UPDATE` query).

```csharp
await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

var existingEntity = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == 10);
if (existingEntity != null)
{
    existingEntity.Description = "Test Serializable level";
    await _dbContext.SaveChangesAsync();
}
await transaction.CommitAsync();
```

4. **SQL Script:** Concurrently executes an `UPDATE` statement on the same row.

```sql
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION;

UPDATE [dbo].[Roles]
SET Description = 'Updated safely with SERIALIZABLE'
WHERE Id = 10;

COMMIT TRANSACTION;
```

### Deadlock Cause
- The **C# code** acquires a shared lock (**S-lock**) when reading the row.
- The **SQL script** tries to acquire an exclusive lock (**X-lock**) for the `UPDATE` statement but is blocked by the C# transaction.
- The **C# transaction** tries to upgrade its lock to an exclusive lock (**X-lock**) during `SaveChangesAsync` but is blocked by the SQL transaction.

This circular dependency causes a **deadlock**.

---

## Solution: Explicit Lock Hints

### Why Use Lock Hints?
Explicit lock hints help control how SQL Server locks rows, minimizing the risk of deadlocks by acquiring the correct type of lock upfront.

### Applying Lock Hints in EF Core

#### 1. **Using `FromSqlRaw` for Reading with Lock Hints**

Use the `WITH (UPDLOCK, ROWLOCK)` lock hints to acquire an update lock immediately:

```csharp
var query = @"
    SELECT * FROM [Roles] WITH (UPDLOCK, ROWLOCK)
    WHERE Id = @roleId";

var existingEntity = await _dbContext.Roles
    .FromSqlRaw(query, new SqlParameter("@roleId", roleId))
    .FirstOrDefaultAsync(cancellationToken);

if (existingEntity != null)
{
    existingEntity.Description = "Test Serializable level with lock hints";
    await _dbContext.SaveChangesAsync();
}
```

#### 2. **Using `ExecuteSqlRaw` for Updates with Lock Hints**

Apply the `WITH (UPDLOCK, ROWLOCK)` hint during the `UPDATE` operation:

```csharp
var sql = @"
    UPDATE [Roles] WITH (UPDLOCK, ROWLOCK)
    SET Description = @description
    WHERE Id = @roleId";

var parameters = new[]
{
    new SqlParameter("@description", "Updated safely with explicit lock hints"),
    new SqlParameter("@roleId", roleId)
};

await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
```

### How This Solves the Problem
- **`UPDLOCK`**: Acquires an update lock immediately, preventing other transactions from acquiring conflicting locks.
- **`ROWLOCK`**: Minimizes the scope of the lock to a single row, reducing contention.

---

## Alternatives

### 1. **Lower the Isolation Level**
Use a less restrictive isolation level, such as **Read Committed**:

```sql
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
BEGIN TRANSACTION;

UPDATE [Roles]
SET Description = 'Updated safely'
WHERE Id = 10;

COMMIT TRANSACTION;
```

### 2. **Sequence the Transactions**
Ensure one transaction commits or rolls back before starting the other to avoid contention.

### 3. **Retry Logic**
Implement retry mechanisms to handle deadlocks gracefully.

---

## Key Takeaways
- **Serializable Isolation Level** provides strict consistency but is prone to deadlocks.
- Using explicit lock hints (`UPDLOCK`, `ROWLOCK`) in EF Core helps control locking behavior and reduces deadlock risk.
- Always balance isolation level and locking strategy based on application requirements.

---


