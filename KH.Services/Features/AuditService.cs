using KH.BuildingBlocks.Extentions.Entities;
using KH.BuildingBlocks.Services;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace KH.Services.Features;
public class AuditService : IAuditService
{

  private readonly IUnitOfWork _unitOfWork;
  private readonly IMapper _mapper;
  private readonly IExcelService _excelService;
  private readonly IStringLocalizer<AuditService> _localizer;

  public AuditService(
      IMapper mapper,
      IUnitOfWork unitOfWork)
  {
    _mapper = mapper;
    _unitOfWork = unitOfWork;
    //_excelService = excelService;
  }

  public async Task<ApiResponse<List<AuditResponse>>> GetCurrentUserTrailsAsync(string userId)
  {
    var res = new ApiResponse<List<AuditResponse>>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<Audit>();

    var context = repository.GetQueryable();
    var trails = await context.Where(a => a.UserId == userId).OrderByDescending(a => a.Id).Take(250).ToListAsync();
    var mappedLogs = _mapper.Map<List<AuditResponse>>(trails);

    // Reformat the serialized values in each log
    foreach (var log in mappedLogs)
    {
      // Reformat oldValues and newValues by cleaning up and deserializing, also handle PascalCase keys
      log.OldValues = FormatAsKeyValueWithReadableKeys(log.OldValues);
      log.NewValues = FormatAsKeyValueWithReadableKeys(log.NewValues);

      // Reformat affectedColumns and primaryKey similarly
      log.AffectedColumns = FormatAffectedColumns(log.AffectedColumns);
      log.PrimaryKey = FormatAsKeyValue(log.PrimaryKey);

      // Format DateTime field to be more readable
      log.DateTime = FormatDateTime(Convert.ToDateTime(log.DateTime));

    }

    res.Data = mappedLogs;
    return res;
  }

  // Helper method to format JSON as key-value pairs with PascalCase or camelCase keys made readable
  private string FormatAsKeyValueWithReadableKeys(string jsonString)
  {
    if (string.IsNullOrWhiteSpace(jsonString))
      return jsonString;

    try
    {
      // Clean up the escaped characters and line breaks
      string cleanedJson = Regex.Unescape(jsonString);

      // Parse the JSON string into a JsonElement
      var parsedJson = JsonSerializer.Deserialize<JsonElement>(cleanedJson);

      // If it's not an object, return as-is
      if (parsedJson.ValueKind != JsonValueKind.Object)
      {
        return cleanedJson;
      }

      // Build key-value pairs as a list of strings with readable keys
      var keyValuePairs = new List<string>();

      foreach (var property in parsedJson.EnumerateObject())
      {
        // Convert the property name to a more readable format (PascalCase/camelCase -> space-separated words)
        var readableKey = ConvertToReadableFormat(property.Name);

        // Handle arrays by joining values with commas
        if (property.Value.ValueKind == JsonValueKind.Array)
        {
          var arrayValues = property.Value.EnumerateArray().Select(v => v.ToString());
          keyValuePairs.Add($"{readableKey}: {string.Join(", ", arrayValues)}");
        }
        // Handle regular values
        else
        {
          keyValuePairs.Add($"{readableKey}: {property.Value}");
        }
      }

      // Join the key-value pairs into a single string
      return string.Join(", ", keyValuePairs);
    }
    catch
    {
      // If deserialization fails, return the original string
      return jsonString;
    }
  }

  // Helper method to convert PascalCase or camelCase into a more readable format
  private string ConvertToReadableFormat(string name)
  {
    return Regex.Replace(name, "(\\B[A-Z])", " $1");
  }

  // Helper method to format the DateTime to a more readable format
  private string FormatDateTime(DateTime dateTime)
  {
    // Format the DateTime to "MM/dd/yyyy HH:mm:ss"
    return dateTime.ToString("MM/dd/yyyy HH:mm:ss");
  }

  // Helper method to properly format the affectedColumns string
  private string FormatAffectedColumns(string jsonString)
  {
    if (string.IsNullOrWhiteSpace(jsonString))
      return jsonString;

    try
    {
      // Clean up the escaped characters
      string cleanedJson = Regex.Unescape(jsonString);

      // Deserialize the string (which is an array in this case)
      var parsedJson = JsonSerializer.Deserialize<JsonElement>(cleanedJson);

      // If it's an array, join the values with commas
      if (parsedJson.ValueKind == JsonValueKind.Array)
      {
        var arrayValues = parsedJson.EnumerateArray().Select(v => v.ToString());
        return string.Join(", ", arrayValues);
      }

      // If it's not an array, return the cleaned string as-is
      return cleanedJson;
    }
    catch
    {
      // If deserialization fails, return the original string
      return jsonString;
    }
  }
  private string FormatAsKeyValue(string jsonString)
  {
    if (string.IsNullOrWhiteSpace(jsonString))
      return jsonString;

    try
    {
      // Clean up the escaped characters and line breaks
      string cleanedJson = Regex.Unescape(jsonString);

      // Parse the JSON string into a JsonElement
      var parsedJson = JsonSerializer.Deserialize<JsonElement>(cleanedJson);

      // If it's not an object, return as-is
      if (parsedJson.ValueKind != JsonValueKind.Object)
      {
        return cleanedJson;
      }

      // Build key-value pairs as a list of strings
      var keyValuePairs = new List<string>();

      foreach (var property in parsedJson.EnumerateObject())
      {
        // Handle arrays by joining values with commas
        if (property.Value.ValueKind == JsonValueKind.Array)
        {
          var arrayValues = property.Value.EnumerateArray().Select(v => v.ToString());
          keyValuePairs.Add($"{property.Name}: {string.Join(", ", arrayValues)}");
        }
        // Handle regular values
        else
        {
          keyValuePairs.Add($"{property.Name}: {property.Value}");
        }
      }

      // Join the key-value pairs into a single string
      return string.Join(", ", keyValuePairs);
    }
    catch
    {
      // If deserialization fails, return the original string
      return jsonString;
    }
  }

  public async Task<ApiResponse<string>> ExportToExcelAsync(string userId, string searchString = "", bool searchInOldValues = false, bool searchInNewValues = false)
  {
    var repository = _unitOfWork.Repository<Audit>();

    var context = repository.GetQueryable();
    var trails = await context.Where(a => a.UserId == userId).OrderByDescending(a => a.Id).Take(250).ToListAsync();

    var res = new ApiResponse<string>((int)HttpStatusCode.OK);

    //_localizer["Audit trails"]
    var data = await _excelService.ExportAsync(trails, sheetName: "Audit trails",
        mappers: new Dictionary<string, Func<Audit, object>>
        {
                    { _localizer["Table Name"], item => item.TableName },
                    { _localizer["Type"], item => item.Type },
                    { _localizer["Date Time (Local)"], item => DateTime.SpecifyKind(item.DateTime, DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture) },
                    { _localizer["Date Time (UTC)"], item => item.DateTime.ToString("G", CultureInfo.CurrentCulture) },
                    { _localizer["Primary Key"], item => item.PrimaryKey },
                    { _localizer["Old Values"], item => item.OldValues },
                    { _localizer["New Values"], item => item.NewValues },
        });

    res.Data = data;
    return res;
  }
}

