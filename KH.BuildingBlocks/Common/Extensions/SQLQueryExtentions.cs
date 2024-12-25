
namespace KH.BuildingBlocks.Infrastructure;
public static class SQLQueryExtentions
{
  // Helper extension to check if the reader has a specific column
  public static bool HasColumn(this System.Data.IDataRecord reader, string columnName)
  {
    for (var i = 0; i < reader.FieldCount; i++)
    {
      if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
      {
        return true;
      }
    }
    return false;
  }
}
