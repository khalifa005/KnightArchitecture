using FluentEmail.Core;
using KH.BuildingBlocks.Apis.Entities;
using KH.Services.Audits.Contracts;
using System.Data;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace KH.Services.Audits.Implementation;
public class AuditService : IAuditService
{

  private readonly IUnitOfWork _unitOfWork;
  private readonly IMapper _mapper;
  private readonly IExcelService _excelService;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IHostEnvironment _env;
  private readonly ILogger<AuditService> _logger;
  public AuditService(
      IMapper mapper,
      ILogger<AuditService> logger,
      IHostEnvironment env,
      IExcelService excelService,
      IUnitOfWork unitOfWork,
      IHttpContextAccessor httpContextAccessor)
  {
    _env = env;
    _logger = logger;
    _mapper = mapper;
    _unitOfWork = unitOfWork;
    _excelService = excelService;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<ApiResponse<List<AuditResponse>>> GetCurrentUserTrailsAsync(string userId, CancellationToken cancellationToken)
  {
    var res = new ApiResponse<List<AuditResponse>>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<Audit>();

    var pagedAuditListResult = await repository.GetPagedWithProjectionAsync<AuditResponse>(
    pageNumber: 1,
    pageSize: 250,
    filterExpression: a => a.UserId == userId,
    projectionExpression: a => new AuditResponse(a),
    orderBy: query => query.OrderByDescending(u => u.Id),
    tracking: false
);
    var mappedLogs = pagedAuditListResult.Items.Select(x => x).ToList();

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

  public async Task<ApiResponse<string>> ExportToExcelAsync(string userId, CancellationToken cancellationToken, string searchString = "", bool searchInOldValues = false, bool searchInNewValues = false)
  {
    var repository = _unitOfWork.Repository<Audit>();

    var context = repository.GetQueryable();
    var trails = await context.Where(a => a.UserId == userId).OrderByDescending(a => a.Id).Take(250).ToListAsync(cancellationToken);

    var res = new ApiResponse<string>((int)HttpStatusCode.OK);

    // Reformat the serialized values in each log
    foreach (var audit in trails)
    {
      // Reformat oldValues and newValues by cleaning up and deserializing, also handle PascalCase keys
      audit.OldValues = FormatAsKeyValueWithReadableKeys(audit.OldValues);
      audit.NewValues = FormatAsKeyValueWithReadableKeys(audit.NewValues);

      // Reformat affectedColumns and primaryKey similarly
      audit.AffectedColumns = FormatAffectedColumns(audit.AffectedColumns);
      audit.PrimaryKey = FormatAsKeyValue(audit.PrimaryKey);

      // Format DateTime field to be more readable
      audit.DateTime = audit.DateTime;
    }

    var data = await _excelService.ExportAsync(trails, sheetName: "Audit trails",
    mappers: new Dictionary<string, Func<Audit, object>>
    {
        { "Table Name", item => item.TableName },
        { "Type", item => item.Type },
        { "User Id", item => item.UserId},
        { "Date Time (Local)", item => DateTime.SpecifyKind(item.DateTime, DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture) },
        { "Date Time (UTC)", item => item.DateTime.ToString("G", CultureInfo.CurrentCulture) },
        { "Primary Key", item => item.PrimaryKey },
        { "Old Values", item => item.OldValues },
        { "New Values", item => item.NewValues },
        { "Affected Columns", item => item.AffectedColumns},
    });

    res.Data = data;
    return res;
  }

  /// <summary>
  /// example of importing data from external resources
  /// </summary>
  /// <param name="file"></param>
  /// <returns></returns>
  public async Task<ApiResponse<string>> ImportExternalAudit(IFormFile file, CancellationToken cancellationToken)
  {
    if (file == null || file.Length == 0)
    {
      return new ApiResponse<string>((int)HttpStatusCode.BadRequest, "No file uploaded");
    }

    // Read the uploaded file into a MemoryStream
    using var stream = new MemoryStream();
    await file.CopyToAsync(stream, cancellationToken);
    stream.Position = 0; // Reset the stream position to the beginning

    // Import the file using the Excel service
    var result = await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, Audit, object>>
    {
        { "Table Name", (row, item) => item.TableName = row["Table Name"].ToString() },
        { "Type", (row, item) => item.Type = row["Type"].ToString() },
        { "User Id", (row, item) => item.UserId= row["User Id"].ToString() },
        { "Date Time (Local)", (row, item) => item.DateTime = DateTime.TryParse(row["Date Time (Local)"].ToString(), out var localDateTime) ? localDateTime : default },
        { "Primary Key", (row, item) => item.PrimaryKey = row["Primary Key"].ToString() },
        { "Affected Columns", (row, item) => item.AffectedColumns= row["Affected Columns"].ToString() },
        { "Old Values", (row, item) => item.OldValues = row["Old Values"].ToString() },
        { "New Values", (row, item) => item.NewValues = row["New Values"].ToString() }
    }, "Audit trails");
    //above sheet name must be as file Audit trails

    // Process the result
    if (result.Succeeded)
    {
      var requestId = _httpContextAccessor.HttpContext?.TraceIdentifier ?? "internal-process";


      result.Data.ForEach(x => x.RequestId = requestId);
      var importedData = result.Data;

      await _unitOfWork.Repository<Audit>().AddRangeAsync(importedData.ToList(), cancellationToken);

      await _unitOfWork.CommitAsync(cancellationToken);

      var res = new ApiResponse<string>((int)HttpStatusCode.OK);
      res.Data = "imported";
      return res;
    }
    else
    {
      return new ApiResponse<string>((int)HttpStatusCode.BadRequest, string.Join(", ", result.Messages));
    }
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
}

