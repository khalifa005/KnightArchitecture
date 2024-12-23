using AngleSharp.Css.Values;
using KH.BuildingBlocks.Apis.Entities;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static QuestPDF.Helpers.Colors;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KH.Services.Locking;

public class LockingService
{
  //Use the sys.dm_tran_locks dynamic management view to observe locking behavior:
  //sql
  //Copy code

  //SELECT* FROM sys.dm_tran_locks;

  private readonly IUnitOfWork _unitOfWork;
  public LockingService(
      IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  /// <summary>
  /// Tests the Serializable Isolation Level.
  /// Prevents dirty reads, non-repeatable reads, and phantom reads.
  /// </summary>
  public async Task<string> TestSerializableIsolationLevelAsync(long role, CancellationToken cancellationToken)
  {
    // Serializable isolation level ensures no other transaction can read or modify the same data until the transaction completes.
    await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken: cancellationToken);

    var repository = _unitOfWork.Repository<Domain.Entities.Role>();
    var query = repository.GetQueryable();
    //Serializable isolation level enforces that no other transaction can read or modify the same data until the transaction completes

    // Retrieve the entity to be updated (tracked by default).
    var roleEntity = await query.FirstOrDefaultAsync(x => x.Id == role);

    // Update the entity description.
    roleEntity.Description = "internal test";
    await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);

    // Behavior Notes:
    // - Update Statement: If modifying the same row, other transactions will wait until this transaction is committed.
    // - Insert Statement: Allowed if it doesn't violate constraints or range locks imposed by the Serializable level.
    // allow other to read the data
    await _unitOfWork.CommitTransactionAsync(cancellationToken: cancellationToken);

    return string.Empty;
  }

  /// <summary>
  /// Tests Row Locking using UPDLOCK and ROWLOCK hints.
  /// Locks the row with the highest ID for updates.
  /// lock adding new row
  /// </summary>
  public async Task<string> TestRowLockAsync(long role, CancellationToken cancellationToken)
  {
    await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken: cancellationToken);

    var repository = _unitOfWork.Repository<Domain.Entities.Role>();

    // Locks the row with the highest ID for updates.
    // Other transactions must wait if they try to lock the same row.
    //prevent reading the same row data 
    //prevent adding a new row data 
    var lastRoleId = await repository.ExecuteSqlSingleAsync<long>(
        "SELECT TOP 1 Id FROM Roles WITH (UPDLOCK, ROWLOCK) ORDER BY Id DESC",
        cancellationToken: cancellationToken
    );

    await _unitOfWork.CommitTransactionAsync(cancellationToken);

    return string.Empty;
  }



  /// <summary>
  /// Tests the Read Committed Isolation Level.
  /// Prevents dirty reads but allows non-repeatable reads and phantom reads.
  /// it cannot read uncommitted changes from other transactions.
  /// </summary>
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


  /// <summary>
  /// Tests the Repeatable Read Isolation Level.
  /// Prevents dirty and non-repeatable reads but allows phantom reads.
  /// </summary>
  public async Task<string> TestRepeatableReadIsolationLevelAsync(long role, CancellationToken cancellationToken)
  {
    await _unitOfWork.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken: cancellationToken);

    var repository = _unitOfWork.Repository<Domain.Entities.Role>();

    // First read
    var roleEntity = await repository.GetQueryable().FirstOrDefaultAsync(x => x.Id == role);
    if (roleEntity != null)
    {
      // Simulate additional read to ensure no data has changed.
      var sameRoleEntity = await repository.GetQueryable().FirstOrDefaultAsync(x => x.Id == role);

      if (roleEntity.Description != sameRoleEntity?.Description)
      {
        throw new InvalidOperationException("Non-repeatable read detected.");
      }

      roleEntity.Description = "RepeatableRead Test";
      await _unitOfWork.CommitAsync(cancellationToken);
    }

    await _unitOfWork.CommitTransactionAsync(cancellationToken);

    return string.Empty;
  }


  /// <summary>
  /// Tests the Read Uncommitted Isolation Level.
  /// Allows dirty reads, non-repeatable reads, and phantom reads.
  /// </summary>
  public async Task<string> TestReadUncommittedIsolationLevelAsync(CancellationToken cancellationToken)
  {
    await _unitOfWork.BeginTransactionAsync(IsolationLevel.ReadUncommitted, cancellationToken: cancellationToken);

    var repository = _unitOfWork.Repository<Domain.Entities.Role>();

    // Dirty read example
    var roleEntity = await repository.GetQueryable().FirstOrDefaultAsync();
    if (roleEntity != null)
    {
      roleEntity.Description = "Dirty Read Test";
      await _unitOfWork.CommitAsync(cancellationToken);
    }

    await _unitOfWork.CommitTransactionAsync(cancellationToken);

    return string.Empty;
  }

  /// <summary>
  /// Prevents phantom reads by locking the range of rows that match the query.
  /// </summary>
  public async Task<string> TestPhantomReadPreventionAsync(CancellationToken cancellationToken)
  {
    await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken: cancellationToken);

    var repository = _unitOfWork.Repository<Domain.Entities.Role>();

    // Lock the range of rows to prevent phantom reads.
    var roles = await repository.ExecuteSqlSingleAsync<List<Domain.Entities.Role>>(
        "SELECT * FROM Roles WITH (HOLDLOCK, UPDLOCK)",
        cancellationToken: cancellationToken
    );

    // Process roles (e.g., update descriptions)
    foreach (var role in roles)
    {
      role.Description = "Phantom Prevention Test";
    }

    await _unitOfWork.CommitTransactionAsync(cancellationToken);

    return string.Empty;
  }

  /// <summary>
  /// Demonstrates range locking to prevent inserts or modifications within a specific range.
  /// </summary>
  public async Task<string> TestRangeLockingAsync(long startId, long endId, CancellationToken cancellationToken)
  {
    await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken: cancellationToken);

    var repository = _unitOfWork.Repository<Domain.Entities.Role>();


    // Lock the range of rows
    var rolesInRange = await repository.ExecuteSqlSingleAsync<List<Domain.Entities.Role>>(
        "SELECT * FROM Roles WITH (HOLDLOCK, UPDLOCK) WHERE Id BETWEEN @StartId AND @EndId",
        new { StartId = startId, EndId = endId },
        cancellationToken: cancellationToken
    );

    foreach (var role in rolesInRange)
    {
      role.Description = "Range Lock Test";
    }

    await _unitOfWork.CommitTransactionAsync(cancellationToken);

    return string.Empty;
  }

  /// <summary>
  /// Demonstrates pessimistic locking using UPDLOCK to prevent other transactions from modifying the same row.
  /// </summary>
  public async Task<string> TestPessimisticLockAsync(long role, CancellationToken cancellationToken)
  {
    await _unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken: cancellationToken);

    var repository = _unitOfWork.Repository<Domain.Entities.Role>();

    // Apply pessimistic lock to the row
    var lockedRole = await repository.ExecuteSqlSingleAsync<Domain.Entities.Role>(
        "SELECT * FROM Roles WITH (UPDLOCK) WHERE Id = @Id",
        new { Id = role },
        cancellationToken: cancellationToken
    );

    if (lockedRole != null)
    {
      lockedRole.Description = "Pessimistic Lock Test";
      repository.UpdateDetachedEntity(lockedRole);
      await _unitOfWork.CommitAsync(cancellationToken);
    }

    await _unitOfWork.CommitTransactionAsync(cancellationToken);

    return string.Empty;
  }


  /// <summary>
  /// Demonstrates optimistic concurrency control using version or timestamp fields.
  /// </summary>
  public async Task<string> TestOptimisticConcurrencyAsync(long roleId, CancellationToken cancellationToken)
  {
    var repository = _unitOfWork.Repository<Domain.Entities.Role>();

    // Retrieve the entity with its row version.
    var roleEntity = await repository.GetQueryable()
                                     .FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);

    if (roleEntity == null)
      throw new KeyNotFoundException($"Role with ID {roleId} not found.");

    // Simulate an update.
    roleEntity.Description = "Optimistic Concurrency Test";

    try
    {
      // No explicit call to UpdateAsync.
      // Just commit the changes.
      await _unitOfWork.CommitAsync(cancellationToken);

      // EF Core will automatically detect changes and update the RowVersion.
    }
    catch (DbUpdateConcurrencyException)
    {
      throw new InvalidOperationException("Concurrency conflict detected. Another user may have updated this record.");
    }

    return "Update successful.";
  }


}

