using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KH.BuildingBlocks.Infrastructure;
public static class SQLQueryHelper
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
