namespace KH.BuildingBlocks.Services;

public interface IResult<out T> : IResult
{
  T Data { get; }
}
