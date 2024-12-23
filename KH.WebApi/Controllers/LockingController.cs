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
}

