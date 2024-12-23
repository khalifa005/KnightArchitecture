# Locking and Isolation Levels in Entity Framework

## Overview
This repository provides a detailed implementation and explanation of locking and isolation levels in a database context using Entity Framework and SQL Server. The implementation focuses on managing concurrent transactions effectively by leveraging different isolation levels and locking mechanisms.

The main goal of this project is to:
- Prevent concurrency issues such as **dirty reads**, **non-repeatable reads**, and **phantom reads**.
- Demonstrate how to enforce locking to ensure data consistency.
- Test and validate different behaviors of isolation levels and locking using both code and SQL Server Management Studio (SSMS).

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

## Implementation
### `LockingService`
The `LockingService` class demonstrates the use of different isolation levels and locking mechanisms in database operations.

#### Methods

##### 1. `TestSerializableIsolationLevelAsync`
Demonstrates the **Serializable** isolation level, ensuring that no other transaction can modify or insert conflicting data until the transaction is committed.

```csharp
public async Task<string> TestSerializableIsolationLevelAsync(long role, CancellationToken cancellationToken)
{
    await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken: cancellationToken);

    var repository = _unitOfWork.Repository<Domain.Entities.Role>();
    var roleEntity = await repository.ExecuteSqlSingleAsync<Domain.Entities.Role>(
        "SELECT * FROM Roles WITH (UPDLOCK) WHERE Id = @Id",
        new { Id = role },
        cancellationToken
    );

    if (roleEntity != null)
    {
        roleEntity.Description = "internal test";
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    await _unitOfWork.CommitTransactionAsync(cancellationToken);
    return string.Empty;
}
```

##### 2. `TestRowLockAsync`
Demonstrates row-level locking using `UPDLOCK` and `ROWLOCK` hints.

```csharp
public async Task<string> TestRowLockAsync(long role, CancellationToken cancellationToken)
{
    await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken: cancellationToken);

    var repository = _unitOfWork.Repository<Domain.Entities.Role>();

    var lastRoleId = await repository.ExecuteSqlSingleAsync<long>(
        "SELECT TOP 1 Id FROM Roles WITH (UPDLOCK, ROWLOCK) ORDER BY Id DESC",
        cancellationToken
    );

    await _unitOfWork.CommitTransactionAsync(cancellationToken);
    return string.Empty;
}
```

##### 3. `TestReadCommittedIsolationLevelAsync`
Demonstrates the **Read Committed** isolation level to prevent dirty reads.

```csharp
public async Task<string> TestReadCommittedIsolationLevelAsync(long role, CancellationToken cancellationToken)
{
    await _unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken: cancellationToken);

    var repository = _unitOfWork.Repository<Domain.Entities.Role>();
    var roleEntity = await repository.GetQueryable().FirstOrDefaultAsync(x => x.Id == role);

    if (roleEntity != null)
    {
        roleEntity.Description = "ReadCommitted Test";
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    await _unitOfWork.CommitTransactionAsync(cancellationToken);
    return string.Empty;
}
```

---

## Testing

### Prerequisites
1. **Database Setup**:
   - Create a `Roles` table:
     ```sql
     CREATE TABLE Roles (
         Id BIGINT PRIMARY KEY,
         Description NVARCHAR(255),
         Version INT
     );
     ```
   - Populate with sample data:
     ```sql
     INSERT INTO Roles (Id, Description, Version)
     VALUES (1, 'Role 1', 1), (2, 'Role 2', 1), (3, 'Role 3', 1);
     ```

2. **Tooling**:
   - Use Visual Studio to debug the code.
   - Use SQL Server Management Studio (SSMS) to monitor transactions and locks.

3. **Monitoring Tools**:
   - Use `sys.dm_tran_locks` to view active locks:
     ```sql
     SELECT * FROM sys.dm_tran_locks;
     ```
   - Use Activity Monitor in SSMS to observe session activity.

### Testing Scenarios

#### 1. **Serializable Isolation Level**
- **Behavior**: Prevents other transactions from modifying or inserting conflicting rows.
- **Steps**:
  1. Run `TestSerializableIsolationLevelAsync` in your code.
  2. Pause the debugger before committing the transaction.
  3. In SSMS, attempt to:
     - Read the same row:
       ```sql
       SELECT * FROM Roles WHERE Id = 1;
       ```
       _Expected_: Query succeeds (Serializable does not block reads).
     - Update the same row:
       ```sql
       UPDATE Roles SET Description = 'Conflict Test' WHERE Id = 1;
       ```
       _Expected_: Query hangs until the transaction commits or rolls back.

#### 2. **Row Locking with UPDLOCK**
- **Behavior**: Prevents other transactions from reading or modifying the locked row.
- **Steps**:
  1. Run `TestRowLockAsync` in your code.
  2. Pause the debugger before committing the transaction.
  3. In SSMS, attempt to:
     - Read the locked row:
       ```sql
       SELECT * FROM Roles WHERE Id = 1;
       ```
       _Expected_: Query hangs.

#### 3. **Read Committed Isolation Level**
- **Behavior**: Prevents dirty reads.
- **Steps**:
  1. Run `TestReadCommittedIsolationLevelAsync` in your code.
  2. Pause the debugger before committing the transaction.
  3. In SSMS, attempt to:
     - Read the same row:
       ```sql
       SELECT * FROM Roles WHERE Id = 1;
       ```
       _Expected_: Query succeeds.
     - Update the same row:
       ```sql
       UPDATE Roles SET Description = 'Conflict Test' WHERE Id = 1;
       ```
       _Expected_: Query hangs until the transaction commits or rolls back.

---

## Observing Locks
To monitor locks in real-time:

1. **View Active Transactions**:
   ```sql
   DBCC OPENTRAN;
   ```

2. **View Locks for a Specific Session**:
   ```sql
   SELECT * FROM sys.dm_tran_locks WHERE request_session_id = <SessionID>;
   ```

3. **Activity Monitor**:
   - Open Activity Monitor in SSMS to view blocking sessions and lock details.

---

## Conclusion
This repository demonstrates how to handle locking and isolation levels in a transactional context effectively. By leveraging the provided examples and testing scenarios, you can ensure data consistency and integrity in your application.

Feel free to contribute or raise issues for enhancements!
