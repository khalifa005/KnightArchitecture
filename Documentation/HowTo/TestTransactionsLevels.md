# Locking and Isolation Levels in Entity Framework

## Overview
This repository provides a detailed implementation and explanation of locking and isolation levels in a database context using Entity Framework and SQL Server. The implementation focuses on managing concurrent transactions effectively by leveraging different isolation levels and locking mechanisms.

---

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



