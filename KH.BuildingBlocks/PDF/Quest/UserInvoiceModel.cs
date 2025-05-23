using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Drawing;

namespace KH.BuildingBlocks.PDF.Quest;

public class UserInvoiceItemResponse
{
  public int UserId { get; set; }
  public string Description { get; set; }
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
}
public class InvoiceModel
{
  public int InvoiceNumber { get; set; }
  public DateTime IssueDate { get; set; }
  public DateTime DueDate { get; set; }

  public AddressPdf SellerAddress { get; set; }
  public AddressPdf CustomerAddress { get; set; }

  public List<OrderItem> Items { get; set; }
  public string Comments { get; set; }
}

public class OrderItem
{
  public string Name { get; set; }
  public decimal Price { get; set; }
  public int Quantity { get; set; }
}

public class AddressPdf
{
  public string CompanyName { get; set; }
  public string Street { get; set; }
  public string City { get; set; }
  public string State { get; set; }
  public string Email { get; set; }
  public string Phone { get; set; }
}

public class InvoiceDocument : IDocument
{
  //public static Image LogoImage { get; } = Image.FromFile("logo.png");

  public InvoiceModel Model { get; }

  public InvoiceDocument(InvoiceModel model)
  {
    Model = model;
  }

  public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

  public void Compose(IDocumentContainer container)
  {
    container
        .Page(page =>
        {
          page.Margin(50);

          page.Header().Element(ComposeHeader);
          page.Content().Element(ComposeContent);

          page.Footer().AlignCenter().Text(text =>
          {
            text.CurrentPageNumber();
            text.Span(" / ");
            text.TotalPages();
          });
        });
  }

  void ComposeHeader(IContainer container)
  {
    container.Row(row =>
    {
      row.RelativeItem().Column(column =>
      {
        column
            .Item().Text($"Invoice #{Model.InvoiceNumber}")
            .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

        column.Item().Text(text =>
        {
          text.Span("Issue date: ").SemiBold();
          text.Span($"{Model.IssueDate:d}");
        });

        column.Item().Text(text =>
        {
          text.Span("Due date: ").SemiBold();
          text.Span($"{Model.DueDate:d}");
        });
      });

      //row.ConstantItem(175).Image(LogoImage);
    });
  }

  void ComposeContent(IContainer container)
  {
    container.PaddingVertical(40).Column(column =>
    {
      column.Spacing(20);

      column.Item().Row(row =>
      {
        row.RelativeItem().Component(new AddressComponent("From", Model.SellerAddress));
        row.ConstantItem(50);
        row.RelativeItem().Component(new AddressComponent("For", Model.CustomerAddress));
      });

      column.Item().Element(ComposeTable);

      var totalPrice = Model.Items.Sum(x => x.Price * x.Quantity);
      column.Item().PaddingRight(5).AlignRight().Text($"Grand total: {totalPrice:C}").SemiBold();

      if (!string.IsNullOrWhiteSpace(Model.Comments))
        column.Item().PaddingTop(25).Element(ComposeComments);
    });
  }

  void ComposeTable(IContainer container)
  {
    var headerStyle = TextStyle.Default.SemiBold();

    container.Table(table =>
    {
      table.ColumnsDefinition(columns =>
      {
        columns.ConstantColumn(25);
        columns.RelativeColumn(3);
        columns.RelativeColumn();
        columns.RelativeColumn();
        columns.RelativeColumn();
      });

      table.Header(header =>
      {
        header.Cell().Text("#");
        header.Cell().Text("Product").Style(headerStyle);
        header.Cell().AlignRight().Text("Unit price").Style(headerStyle);
        header.Cell().AlignRight().Text("Quantity").Style(headerStyle);
        header.Cell().AlignRight().Text("Total").Style(headerStyle);

        header.Cell().ColumnSpan(5).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
      });

      foreach (var item in Model.Items)
      {
        var index = Model.Items.IndexOf(item) + 1;

        table.Cell().Element(CellStyle).Text($"{index}");
        table.Cell().Element(CellStyle).Text(item.Name);
        table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price:C}");
        table.Cell().Element(CellStyle).AlignRight().Text($"{item.Quantity}");
        table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price * item.Quantity:C}");

        static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
      }
    });
  }

  void ComposeComments(IContainer container)
  {
    container.ShowEntire().Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
    {
      column.Spacing(5);
      column.Item().Text("Comments").FontSize(14).SemiBold();
      column.Item().Text(Model.Comments);
    });
  }
}

public class AddressComponent : IComponent
{
  private string Title { get; }
  private AddressPdf Address { get; }

  public AddressComponent(string title, AddressPdf address)
  {
    Title = title;
    Address = address;
  }

  public void Compose(IContainer container)
  {
    container.ShowEntire().Column(column =>
    {
      column.Spacing(2);

      column.Item().Text(Title).SemiBold();
      column.Item().PaddingBottom(5).LineHorizontal(1);

      column.Item().Text(Address.CompanyName);
      column.Item().Text(Address.Street);
      column.Item().Text($"{Address.City}, {Address.State}");
      column.Item().Text(Address.Email);
      column.Item().Text(Address.Phone);
    });
  }
}

