using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KH.Services.Pdf;

public static class PdfTemplatesConstant
{
  public static class Layouts
  {
    public const string Customer_Welcome_Layout = "CustomerPdfLayout.html";
    public const string Main_Layout = "PdfLayoutTemplate.html";
  }
  public static class Templates
  {
    public const string Customer_Welcome_Template = "CustomerPdfTemplateBody.html";
    public const string Invoice_Template = "InvoiceTemplate.html";
  }
}

