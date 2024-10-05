namespace KH.BuildingBlocks.Contracts;

public interface IResult
{
  List<string> Messages { get; set; }

  bool Succeeded { get; set; }
}

public interface IResult<out T> : IResult
{
  T Data { get; }
}
