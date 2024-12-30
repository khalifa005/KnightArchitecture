using KH.Services.Locking;

namespace KH.WebApi.Controllers;

public class LockingController : BaseApiController
{
  public readonly LockingService _lockingService;
  public readonly IsolationLevelsService _isolationLevelsService;
  public LockingController(LockingService lockingService, IsolationLevelsService isolationLevelsService)
  {
    _lockingService = lockingService;
    _isolationLevelsService = isolationLevelsService;
  }

  [HttpPost("OptimisticConcurrency/{roleId}")]
  public async Task<ActionResult> OptimisticConcurrency(long roleId, CancellationToken cancellationToken)
  {
    //var res = await _lockingService.TestOptimisticConcurrencyAsync(roleId, cancellationToken);
    var res = await _isolationLevelsService.OptimisticConcurrencyAsync(roleId, cancellationToken);
    return Ok(res);
  }

  [HttpPost("ReadUncommitted/{roleId}")]
  public async Task<ActionResult> ReadUncommittedIsolationLevel(long roleId, CancellationToken cancellationToken)
  {
    var res = await _isolationLevelsService.ReadUncommittedIsolationLevelAsync(cancellationToken);
    return Ok(res);
  }

  [HttpPost("ReadCommitted/{roleId}")]
  public async Task<ActionResult> ReadCommittedIsolationLevel(long roleId, CancellationToken cancellationToken)
  {
    var res = await _isolationLevelsService.ReadCommittedIsolationLevelAsync(roleId, cancellationToken);
    return Ok(res);
  }

  [HttpPost("RepeatableRead/{roleId}")]
  public async Task<ActionResult> RepeatableReadIsolationLevel(long roleId, CancellationToken cancellationToken)
  {
    var res = await _isolationLevelsService.RepeatableReadIsolationLevelAsync(roleId, cancellationToken);
    return Ok(res);
  }

  [HttpPost("Serialization/{roleId}")]
  public async Task<ActionResult> SerializableIsolationLevel(long roleId, CancellationToken cancellationToken)
  {
    var res = await _isolationLevelsService.SerializableIsolationLevelAsync(roleId, cancellationToken);
    return Ok(res);
  }

  [HttpGet("RowLock/{roleId}")]
  public async Task<ActionResult> RowLock(long roleId, CancellationToken cancellationToken)
  {
    var res = await _lockingService.TestRowLockAsync(roleId, cancellationToken);
    return Ok();
  }

}

