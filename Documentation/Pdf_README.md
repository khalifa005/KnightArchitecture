# PDF Service Documentation

The PDF Service in the Knight Architecture repository provides dynamic and flexible PDF generation capabilities. It leverages various libraries to generate, manage, and merge PDFs with support for templates and localization.

---

## Features
- **Dynamic PDF Generation**: Create PDFs using predefined HTML templates and dynamic placeholders.
- **Invoice Generation**: Generate invoices with multi-language support and localized content.
- **PDF Merging**: Combine multiple PDFs into a single document.
- **Flexible Libraries**: Supports multiple tools like QuestPDF, PDFsharp, and NReco for diverse use cases.

---

## Core Technologies and Packages
The PDF Service uses the following libraries:
1. **[QuestPDF](https://www.questpdf.com/)**: For building complex layouts with precise control over content styling.
2. **[PDFsharp](https://pdfsharp.net/)**: For merging and manipulating PDF documents.
3. **[NReco.PdfGenerator](https://www.nrecosite.com/pdf_generator.aspx)**: For converting raw HTML into PDF files.
4. **[DinkToPDF](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http)**: For handling HTML into PDF files.


---

## Service Interface

### Methods

#### 1. ExportUserDetailsPdfAsync
- **Description**: Generates a user details PDF based on filters.
- **Parameters**:
  - `UserFilterRequest param`: Filter criteria for user data.
  - `CancellationToken cancellationToken`: Allows cancellation of the operation.
- **Returns**: `Task<byte[]>` containing the PDF as a byte array.

#### 2. GeneratePdfWithDynamicContent
- **Description**: Creates a PDF from an HTML template with placeholder values.
- **Parameters**:
  - `string templateName`: Name of the template file.
  - `string layoutName`: Name of the layout file.
  - `Dictionary<string, string> placeholdersWithValues`: Content placeholders.
  - `string language`: Language for content localization (default is "en").
- **Returns**: `Task<byte[]>`.

#### 3. ExportUserInvoicePdf
- **Description**: Generates an invoice PDF with localized content.
- **Parameters**:
  - `string language`: Language preference (default is "en").
- **Returns**: `Task<byte[]>`.

#### 4. MergePdfsAsync
- **Description**: Combines multiple PDFs into one.
- **Parameters**:
  - `IEnumerable<IFormFile> formFiles`: List of PDF files to merge.
- **Returns**: `Task<byte[]>`.

#### 5. GeneratePdfFromHtmlWithNReco
- **Description**: Converts raw HTML to a PDF using NReco.
- **Parameters**:
  - `string htmlContent`: HTML content for PDF generation.
- **Returns**: `byte[]`.

---

## Service Implementation

The `PdfService` implements `IPdfService` and provides additional methods and helper functions.

### Highlighted Implementations

#### 1. Dynamic Content Replacement
- The `ReplacePlaceholders` method replaces placeholders in HTML templates with dynamic content.
- Example Placeholder: `{{USER_NAME}}`.

#### 2. Multi-Library Support
- **QuestPDF**: Used for advanced layouts (e.g., invoices).
- **PDFsharp**: Used for merging PDFs.
- **NReco.PdfGenerator**: Converts raw HTML into PDF files for simple use cases.

#### 3. Error Handling
- All methods are wrapped in `try-catch` blocks to log and handle exceptions gracefully.

---

## Web API Controller

The `PdfController` provides API endpoints for client-side interaction with the service.

### Endpoints

#### 1. `POST /WelcomePDFTemplateFromHTML`
- **Description**: Generates a "Welcome" PDF with user details.
- **Body**: `UserFilterRequest` object.
- **Returns**: The PDF as a downloadable file.

#### 2. `POST /InvoicePDFTemplateFromHTML`
- **Description**: Generates an invoice PDF with dynamic placeholders.
- **Body**: `UserFilterRequest` object.
- **Returns**: The PDF as a downloadable file.

#### 3. `POST /MergeMultiplePdfsWithSharp`
- **Description**: Merges multiple uploaded PDFs.
- **Body**: List of PDF files (`IFormFile`).
- **Returns**: The merged PDF file.

---

## Templates

All HTML templates are stored in the `Templates/PDF` directory. Example templates include:
1. **CustomerPdfTemplateBody.html**: For user details PDFs.
2. **InvoiceTemplate.html**: For generating invoices.
3. **PdfLayoutTemplate.html**: A layout wrapper for other templates.

### Template Placeholders
- **Dynamic Content**: Defined as `{{PLACEHOLDER_NAME}}` in templates.
- **Examples**:
  - `{{USER_NAME}}`: Replaced with the user's name.
  - `{{INVOICE_TOTAL}}`: Replaced with the invoice total.

---

## Setup and Configuration

### 1. Prerequisites
- Install the required NuGet packages:
  ```bash
  dotnet add package QuestPDF
  dotnet add package PDFsharp
  dotnet add package NReco.PdfGenerator
  dotnet add package DinkToPDF
