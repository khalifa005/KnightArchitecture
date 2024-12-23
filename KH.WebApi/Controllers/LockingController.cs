using KH.Services.Locking;

namespace KH.WebApi.Controllers;

public class LockingController : BaseApiController
{
  public readonly LockingService _lockingService;
  public LockingController(LockingService lockingService)
  {
    _lockingService = lockingService;
  }

  [HttpGet("Serialization/{userId}")]
  public async Task<ActionResult> SerializableIsolationLevel(long userId, CancellationToken cancellationToken)
  {
    var res = await _lockingService.TestSerializableIsolationLevelAsync(userId, cancellationToken);
    return Ok();
  }

  [HttpGet("RowLock/{userId}")]
  public async Task<ActionResult> RowLock(long userId, CancellationToken cancellationToken)
  {
    var res = await _lockingService.TestRowLockAsync(userId, cancellationToken);
    return Ok();
  }

  [HttpPost("ReadCommitted/{roleId}")]
  public async Task<ActionResult> TestReadCommittedIsolationLevel(long roleId, CancellationToken cancellationToken)
  {
    var res = await _lockingService.TestReadCommittedIsolationLevelAsync(roleId, cancellationToken);
    return Ok(res);
  }

  [HttpPost("RepeatableRead/{roleId}")]
  public async Task<ActionResult> TestRepeatableReadIsolationLevel(long roleId, CancellationToken cancellationToken)
  {
    var res = await _lockingService.TestRepeatableReadIsolationLevelAsync(roleId, cancellationToken);
    return Ok(res);
  }

  [HttpPost("ReadUncommitted")]
  public async Task<ActionResult> TestReadUncommittedIsolationLevel(CancellationToken cancellationToken)
  {
    var res = await _lockingService.TestReadUncommittedIsolationLevelAsync(cancellationToken);
    return Ok(res);
  }

  [HttpPost("PhantomReadPrevention")]
  public async Task<ActionResult> TestPhantomReadPrevention(CancellationToken cancellationToken)
  {
    var res = await _lockingService.TestPhantomReadPreventionAsync(cancellationToken);
    return Ok(res);
  }

  [HttpPost("RangeLocking/{startId}/{endId}")]
  public async Task<ActionResult> TestRangeLocking(long startId, long endId, CancellationToken cancellationToken)
  {
    var res = await _lockingService.TestRangeLockingAsync(startId, endId, cancellationToken);
    return Ok(res);
  }

  [HttpPost("PessimisticLock/{roleId}")]
  public async Task<ActionResult> TestPessimisticLock(long roleId, CancellationToken cancellationToken)
  {
    var res = await _lockingService.TestPessimisticLockAsync(roleId, cancellationToken);
    return Ok(res);
  }

  [HttpPost("OptimisticConcurrency/{roleId}")]
  public async Task<ActionResult> TestOptimisticConcurrency(long roleId, CancellationToken cancellationToken)
  {
    var res = await _lockingService.TestOptimisticConcurrencyAsync(roleId, cancellationToken);
    return Ok(res);
  }

}

