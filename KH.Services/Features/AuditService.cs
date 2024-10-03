using KH.BuildingBlocks.Extentions.Entities;
using KH.BuildingBlocks.Services;
using Microsoft.Extensions.Localization;
using System.Globalization;

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

    res.Data = mappedLogs;
    return res;
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

