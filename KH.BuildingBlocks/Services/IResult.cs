namespace KH.BuildingBlocks.Services;

public interface IResult
{
  List<string> Messages { get; set; }

  bool Succeeded { get; set; }
}
