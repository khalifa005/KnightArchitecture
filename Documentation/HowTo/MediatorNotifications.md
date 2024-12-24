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

#### Methods and Expected Behaviors

##### 1. `TestSerializableIsolationLevelAsync`
- **Behavior**: Ensures no other transaction can modify or insert conflicting data until the transaction is committed.
- **Expected Behavior**:
  - Prevents dirty reads, non-repeatable reads, and phantom reads.
  - `SELECT` queries without explicit locks are allowed.
- **Unexpected Behavior**:
  - If no `UPDLOCK` is applied, other transactions can read the same row.

##### 2. `TestRowLockAsync`
- **Behavior**: Locks the row with the highest ID for updates and prevents other transactions from reading or modifying it.
- **Expected Behavior**:
  - Other transactions attempting to read or update the row will wait until the transaction is committed.
  - Prevents inserting new rows into the same locked range.

##### 3. `TestReadCommittedIsolationLevelAsync`
- **Behavior**: Prevents dirty reads but allows non-repeatable reads and phantom reads.
- **Expected Behavior**:
  - Transactions cannot read uncommitted changes from other transactions.
  - Repeated reads may return different data.

##### 4. `TestRepeatableReadIsolationLevelAsync`
- **Behavior**: Prevents dirty and non-repeatable reads but allows phantom reads.
- **Expected Behavior**:
  - Repeated reads of the same data return consistent results within the transaction.
  - New rows matching the query can still be inserted by other transactions.

##### 5. `TestReadUncommittedIsolationLevelAsync`
- **Behavior**: Allows dirty reads, non-repeatable reads, and phantom reads.
- **Expected Behavior**:
  - Reads uncommitted changes made by other transactions.
  - May read incomplete or invalid data.

##### 6. `TestPhantomReadPreventionAsync`
- **Behavior**: Prevents phantom reads by locking the range of rows that match the query.
- **Expected Behavior**:
  - No new rows matching the query condition can be inserted or updated by other transactions.

##### 7. `TestRangeLockingAsync`
- **Behavior**: Demonstrates range locking to prevent inserts or modifications within a specific range.
- **Expected Behavior**:
  - Other transactions attempting to modify or insert rows within the locked range will be blocked.

##### 8. `TestPessimisticLockAsync`
- **Behavior**: Applies pessimistic locking using `UPDLOCK` to prevent other transactions from modifying the same row.
- **Expected Behavior**:
  - Other transactions attempting to modify the row will wait until the lock is released.

##### 9. `TestOptimisticConcurrencyAsync`
- **Behavior**: Demonstrates optimistic concurrency control using version or timestamp fields.
- **Expected Behavior**:
  - Updates succeed only if the row has not been modified by another transaction.
- **Unexpected Behavior**:
  - If another transaction modifies the row before committing, a concurrency exception is thrown.

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

#### 4. **Repeatable Read Isolation Level**
- **Steps**:
  1. Run `TestRepeatableReadIsolationLevelAsync` in your code.
  2. Pause the debugger after the first read.
  3. In SSMS, attempt to:
     - Insert a new row matching the query condition:
       ```sql
       INSERT INTO Roles (Id, Description, Version) VALUES (4, 'New Role', 1);
       ```
       _Expected_: Query succeeds.

#### 5. **Phantom Reads Prevention**
- **Steps**:
  1. Run `TestPhantomReadPreventionAsync` in your code.
  2. Pause the debugger before committing the transaction.
  3. In SSMS, attempt to:
     - Insert a row matching the query condition:
       ```sql
       INSERT INTO Roles (Id, Description, Version) VALUES (5, 'Phantom Role', 1);
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
