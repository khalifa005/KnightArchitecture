using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;

namespace KH.BuildingBlocks.PDF.Quest;

public class QuestHelper
{
  public static Document GenerateReport(string title, int itemsCount)
  {
    return Document.Create(document =>
    {
      document.Page(page =>
      {
        page.Size(PageSizes.A5);
        page.Margin(0.5f, Unit.Inch);

        page.Header()
            .Text(title)
            .Bold()
            .FontSize(24)
            .FontColor(Colors.Blue.Accent2);

        page.Content()
            .PaddingVertical(20)
            .Column(column =>
            {
              column.Spacing(10);

              foreach (var i in Enumerable.Range(0, itemsCount))
              {
                column
                          .Item()
                          .Width(200)
                          .Height(50)
                          .Background(Colors.Grey.Lighten3)
                          .AlignMiddle()
                          .AlignCenter()
                          .Text($"Item {i}")
                          .FontSize(16);
              }
            });

        page.Footer()
            .AlignCenter()
            .PaddingVertical(20)
            .Text(text =>
            {
              text.DefaultTextStyle(TextStyle.Default.FontSize(16));

              text.CurrentPageNumber();
              text.Span(" / ");
              text.TotalPages();
            });
      });
    });
  }

  public static QuestPDF.Infrastructure.IDocument CreateDocument()
  {

    var doc = Document.Create(container =>
    {
      container.Page(page =>
      {
        page.Size(PageSizes.A4);
        page.Margin(2, Unit.Centimetre);
        page.PageColor(Colors.White);
        page.DefaultTextStyle(x => x.FontSize(20));

        page.Header()
            .Text("Hello PDF!")
            .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

        page.Content()
            .PaddingVertical(1, Unit.Centimetre)
            .Column(x =>
            {
              x.Spacing(20);

              x.Item().Text(Placeholders.LoremIpsum());
              x.Item().Image(Placeholders.Image(200, 100));
            });

        page.Footer()
            .AlignCenter()
            .Text(x =>
            {
              x.Span("Page ");
              x.CurrentPageNumber();
            });
      });
    });
    //var docAsBytesArray = doc.GeneratePdf();//if u need bytes array
    return doc;
  }
}

