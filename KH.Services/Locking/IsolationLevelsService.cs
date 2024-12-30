using KH.PersistenceInfra.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KH.Services.Locking;

//https://github.com/khalifa005/KnightArchitecture/blob/master/Documentation/HowTo/TestTransactionsLevels.md
public class IsolationLevelsService
{
  private readonly AppDbContext _dbContext;
  public IsolationLevelsService(
    AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  /// <summary>
  /// Multiple users can update the same data independently, and conflicts are checked at the time of saving the data
  /// demonstrates optimistic concurrency control using version or timestamp fields.
  /// How to test
  /// Simulate another process modifying the record during the current transaction still active.
  /// Open SQL Server Management Studio (SSMS) and execute the following query
  /// UPDATE Roles
  /// SET Description = 'Updated from SQL Server'
  /// WHERE Id = 10;
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
      //EF Core automatically checks for concurrency conflicts and update the RowVersion.
      //by comparing the RowVersion or Timestamp field in the database with the one initially read.

      await _dbContext.SaveChangesAsync();
      //When SaveChangesAsync is called, EF Core generates the following SQL query 
      //UPDATE Roles
      //SET
      //Description = 'Test Optimistic Concurrency from code',
      //RowVersion = @newRowVersion
      //WHERE
      //Id = 10 AND RowVersion = @currentRowVersion;


      // Commit the transaction
      await transaction.CommitAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      //once it is detected, the transaction is rolled back and an exception is thrown.
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

  /// <summary>
  /// Allows dirty reads, non-repeatable reads, and phantom reads.
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
        //if another transaction with ReadUncommitted level try to read this entity it will get the new value
        //even if we didn't call transaction.CommitAsync yet
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

    return $"Role with id:{existingEntity.Id} has been updated inside transaction with Read Uncommitted Isolation Level ";
  }

  /// <summary>
  /// Prevents dirty reads but allows non-repeatable reads and phantom reads.
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

    return $"Role with id:{existingEntity.Id} has been updated inside transaction with ReadCommitted Isolation Level ";
  }

  /// <summary>
  /// Prevents dirty and non-repeatable reads but allows phantom reads.
  /// </summary>
  public async Task<string> RepeatableReadIsolationLevelAsync(long role, CancellationToken cancellationToken)
  {
    // Begin a transaction
    await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

    // First read after this line other transaction won't be able to update the same row data
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

    return $"Role with id:{existingEntity.Id} has been updated inside transaction with RepeatableRead Isolation Level ";

  }

  /// <summary>
  /// Prevents phantom reads by locking the range of rows that match the query.
  /// A transaction reads a range of rows (e.g., WHERE Price > 100).
  /// No Dirty Reads: Transactions do not read data written by uncommitted transactions.
  /// No Non-Repeatable Reads: A transaction always reads the same data during its execution.
  /// No Phantom Reads: Ensures that new rows matching a WHERE clause cannot be inserted or modified by other transactions until the current transaction completes.

  /// </summary>
  public async Task<string> SerializableIsolationLevelAsync(long role, CancellationToken cancellationToken)

  {
    // Begin a transaction
    await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
    //once i read this cann't be modified by other transactions s-lock
    //var existingEntity = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == 10, cancellationToken: cancellationToken);

    var query = @"
            SELECT * FROM [Roles] WITH (UPDLOCK, ROWLOCK)
            WHERE Id = @roleId";

    var existingEntity = await _dbContext.Roles
        .FromSqlRaw(query, new SqlParameter("@roleId", 10))
        .FirstOrDefaultAsync(cancellationToken);


    try
    {
      if (existingEntity != null)
      {
        existingEntity.Description = "Test Serializable level from code";
        await _dbContext.SaveChangesAsync();
        //x-lock
        //after this line get executed no other Serializable transaction can read the same data till we commit the transaction
      }

      //  // Define the SQL command with parameters
      //  var sql = @"
      //    UPDATE Roles
      //    SET Description = {1}, UpdatedDate = {2}
      //    WHERE Id = {0}";

      //  // Execute the raw SQL command
      //  int rowsAffected = await _dbContext.Database.ExecuteSqlRawAsync(
      //      sql,
      //      10,
      //      "Test Serializable level from code direct raw sql update",
      //      DateTime.UtcNow
      //  );

      //  var rolesResult = await _dbContext.Roles
      //.FromSqlRaw("SELECT * FROM Roles WITH (HOLDLOCK, UPDLOCK) WHERE Id = {0}", 10)
      //.ToListAsync(cancellationToken);

      //  var rolesResultWithLock = await _dbContext.Roles
      //.FromSqlInterpolated($"SELECT * FROM Roles WITH (HOLDLOCK, UPDLOCK) WHERE Id  = {10}")
      //.ToListAsync();


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

    return $"Role with id:{existingEntity.Id} has been updated inside transaction with Serializable Isolation Level ";
  }

  /// <summary>

  /// Locks the row with the ID for updates.
  /// lock adding new row and update the specified row
  /// </summary>
  public async Task<string> RowLockAsync(long role, CancellationToken cancellationToken)
  {
    // Begin a transaction
    await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

    var roleResultWithLock = await _dbContext.Roles
  .FromSqlInterpolated($"SELECT * FROM Roles WITH (HOLDLOCK, UPDLOCK) WHERE Id  = {10}")
  .FirstOrDefaultAsync(cancellationToken);

    try
    {
      if (roleResultWithLock != null)
      {
        roleResultWithLock.Description = "Test RowLock from code";
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

    return $"Role with id:{roleResultWithLock?.Id} has been updated inside transaction with Serializable Isolation Level ";
  }


  



}
