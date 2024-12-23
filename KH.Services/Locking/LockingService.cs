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
    var lastRoleId = await repository.ExecuteSqlSingleAsync<long>(
        "SELECT TOP 1 Id FROM Roles WITH (UPDLOCK, ROWLOCK) ORDER BY Id DESC",
        cancellationToken
    );

    await _unitOfWork.CommitTransactionAsync(cancellationToken);

    return string.Empty;
  }

}

