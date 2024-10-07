using KH.BuildingBlocks.Contracts;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Drawing;

namespace KH.BuildingBlocks.Services;
public class ExcelService : IExcelService
{
  private readonly IStringLocalizer<ExcelService> _localizer;

  public ExcelService()
  {
    //_localizer = localizer;
  }

  public async Task<string> ExportAsync<TData>(IEnumerable<TData> data
      , Dictionary<string, Func<TData, object>> mappers
      , string sheetName = "Sheet1")
  {
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    using var p = new ExcelPackage();
    p.Workbook.Properties.Author = "BlazorHero";
    //p.Workbook.Worksheets.Add(_localizer["Audit Trails"]);
    p.Workbook.Worksheets.Add("Audit Trails");
    var ws = p.Workbook.Worksheets[0];
    ws.Name = sheetName;
    ws.Cells.Style.Font.Size = 11;
    ws.Cells.Style.Font.Name = "Calibri";

    var colIndex = 1;
    var rowIndex = 1;

    var headers = mappers.Keys.Select(x => x).ToList();

    foreach (var header in headers)
    {
      var cell = ws.Cells[rowIndex, colIndex];

      var fill = cell.Style.Fill;
      fill.PatternType = ExcelFillStyle.Solid;
      fill.BackgroundColor.SetColor(Color.LightBlue);

      var border = cell.Style.Border;
      border.Bottom.Style =
          border.Top.Style =
              border.Left.Style =
                  border.Right.Style = ExcelBorderStyle.Thin;

      cell.Value = header;

      colIndex++;
    }

    var dataList = data.ToList();
    foreach (var item in dataList)
    {
      colIndex = 1;
      rowIndex++;

      var result = headers.Select(header => mappers[header](item));

      foreach (var value in result)
      {
        ws.Cells[rowIndex, colIndex++].Value = value;
      }
    }

    using (ExcelRange autoFilterCells = ws.Cells[1, 1, dataList.Count + 1, headers.Count])
    {
      autoFilterCells.AutoFilter = true;
      autoFilterCells.AutoFitColumns();
    }

    var byteArray = await p.GetAsByteArrayAsync();
    return Convert.ToBase64String(byteArray);
  }

  public async Task<IResult<IEnumerable<TEntity>>> ImportAsync<TEntity>(Stream stream, Dictionary<string, Func<DataRow, TEntity, object>> mappers, string sheetName = "Sheet1")
  {
    var result = new List<TEntity>();
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    using var p = new ExcelPackage();
    stream.Position = 0;
    await p.LoadAsync(stream);

    var ws = p.Workbook.Worksheets[sheetName];
    if (ws == null)
    {
      return await Result<IEnumerable<TEntity>>.FailAsync(string.Format("Sheet with name {0} does not exist!", sheetName));
    }

    var dt = new DataTable();
    var titlesInFirstRow = true;
    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
    {
      dt.Columns.Add(titlesInFirstRow ? firstRowCell.Text : $"Column {firstRowCell.Start.Column}");
    }

    var startRow = titlesInFirstRow ? 2 : 1;
    var headers = mappers.Keys.ToList();
    var errors = new List<string>();

    // Validate headers
    foreach (var header in headers)
    {
      if (!dt.Columns.Contains(header))
      {
        errors.Add(string.Format("Header '{0}' does not exist in table!", header));
      }
    }

    if (errors.Any())
    {
      return await Result<IEnumerable<TEntity>>.FailAsync(errors);
    }

    // Process rows
    for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
    {
      try
      {
        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
        DataRow row = dt.Rows.Add();
        var item = (TEntity)Activator.CreateInstance(typeof(TEntity));

        // Add cell values to DataRow
        foreach (var cell in wsRow)
        {
          row[cell.Start.Column - 1] = cell.Text;
        }

        // Apply mappers to map the DataRow to TEntity
        headers.ForEach(x => mappers[x](row, item));

        result.Add(item);
      }
      catch (Exception e)
      {
        return await Result<IEnumerable<TEntity>>.FailAsync(e.Message);
      }
    }

    return await Result<IEnumerable<TEntity>>.SuccessAsync(result, "Import Success");
  }

}