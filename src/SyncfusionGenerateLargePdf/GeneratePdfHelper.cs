using System.Diagnostics;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;

namespace SyncfusionGenerateLargePdf;

public class GeneratePdfHelper
{
    private enum ColumnIndex
    {
        ManufacturerImporter,
        Type,
        Attribute1,
        Model,
        Attribute2,
        Attribute3,
        Attribute4,
    }

    public static void GenerateGridPdf(int itemCount)
    {
        string? outputFilePath;
        var swTotal = Stopwatch.StartNew();
        try
        {
            using var pdfDocument = new PdfDocument();

            pdfDocument.PageSettings.Size = PdfPageSize.A4;
            pdfDocument.PageSettings.Margins = new PdfMargins { Top = 40, Left = 40, Bottom = 0, Right = 40, };

            // Create the new PdfGrid.
            var pdfGrid = new PdfGrid();

            // Pad the cells uniformly.
            pdfGrid.Style.CellPadding = new PdfPaddings(2, 2, 2, 2);


            //
            // Add the columns.
            //

            pdfGrid.Columns.Add(count: 7);

            // Two headers: table header and column headers. Repeat them on every page.
            pdfGrid.Headers.Add(2);
            pdfGrid.RepeatHeader = true;


            // Add the table header.
            var pdfTableHeader = pdfGrid.Headers[0];
            pdfTableHeader.Cells[0].Value = "Table of Items";
            pdfTableHeader.Cells[0].ColumnSpan = 7;
            pdfTableHeader.Cells[0].StringFormat = new PdfStringFormat
            {
                Alignment = PdfTextAlignment.Center,
                LineAlignment = PdfVerticalAlignment.Middle,
            };
            pdfTableHeader.Cells[0].Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Regular);



            // Add the column headers.

            var pdfColHeader = pdfGrid.Headers[1];
            pdfColHeader.Cells[(int)ColumnIndex.ManufacturerImporter].Value = "Manufacturer / Importer";
            pdfColHeader.Cells[(int)ColumnIndex.Type].Value = "Type";
            pdfColHeader.Cells[(int)ColumnIndex.Attribute1].Value = "Attribute 1";
            pdfColHeader.Cells[(int)ColumnIndex.Model].Value = "Model";
            pdfColHeader.Cells[(int)ColumnIndex.Attribute2].Value = "Attribute 2";
            pdfColHeader.Cells[(int)ColumnIndex.Attribute3].Value = "Attribute 3";
            pdfColHeader.Cells[(int)ColumnIndex.Attribute4].Value = "Attribute 4";
            pdfColHeader.Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);

            Console.WriteLine($"[{swTotal.Elapsed}] Finished creating headers.");


            //
            // Add rows
            //

            Enumerable.Range(0, itemCount).Iterate((item, ix) =>
            {
                var row = pdfGrid.Rows.Add();

                row.Cells[(int)ColumnIndex.ManufacturerImporter].Value = $"Manufacturer / Importer{ix} dsflkjasdkfajsdkfjasdkfjaksdjfaksdjfaskdjfadskjf";
                row.Cells[(int)ColumnIndex.Type].Value = $"Type{ix}";
                row.Cells[(int)ColumnIndex.Attribute1].Value = $"Attribute1_{ix}";
                row.Cells[(int)ColumnIndex.Model].Value = $"Model{ix}";
                row.Cells[(int)ColumnIndex.Attribute2].Value = $"Attribute2_{ix}";
                row.Cells[(int)ColumnIndex.Attribute3].Value = $"Attribute3_{ix}";
                row.Cells[(int)ColumnIndex.Attribute4].Value = $"Attribute4_{ix}";

                row.Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Regular);

                var actualPage = ix + 1;
                if (actualPage % 1000 == 0)
                {
                    Console.WriteLine($"[{swTotal.Elapsed}][{ix:N0}] Finished creating 1,000 rows.");
                }
            });


            //
            // Draw the grid on the page.
            //

            Console.WriteLine($"[{swTotal.Elapsed}] Before Pages.Add() and Draw()...");
            var page = pdfDocument.Pages.Add();
            pdfGrid.Draw(page, PointF.Empty);
            Console.WriteLine($"[{swTotal.Elapsed}] After Pages.Add() and Draw().");


            //
            // Add a footer
            //

            Console.WriteLine($"[{swTotal.Elapsed}] Before add footer...");
            //Create a page template that is used as footer
            var footerBounds = new RectangleF(x: 0, y: 0, width: pdfDocument.Pages[0].GetClientSize().Width, height: 50);
            var footerTemplate = new PdfPageTemplateElement(footerBounds);
            var footerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 9, PdfFontStyle.Bold);
            var footerLayoutRect = new RectangleF(0, 0, footerTemplate.Width, footerTemplate.Height);

            var footerStringFormat = new PdfStringFormat
            {
                Alignment = PdfTextAlignment.Center,
                LineAlignment = PdfVerticalAlignment.Middle,
            };

            footerTemplate.Graphics.DrawString("Generic Footer Text", footerFont, PdfBrushes.Black, footerLayoutRect, footerStringFormat);

            // Add the footer template at the bottom
            pdfDocument.Template.Bottom = footerTemplate;
            Console.WriteLine($"[{swTotal.Elapsed}] After add footer.");


            // Save and close the document.
            Console.WriteLine($"[{swTotal.Elapsed}] Before save...");
            outputFilePath = SaveFile(pdfDocument, "GeneratedGridPdf.pdf");
            Console.WriteLine($"[{swTotal.Elapsed}] After save.");
        }
        finally
        {
            swTotal.Stop();
            Console.WriteLine($"Generating a PDF with {itemCount:N0} items took {swTotal.Elapsed}.");
        }

        if (!string.IsNullOrWhiteSpace(outputFilePath))
        {
            // Launch the PDF in a new browser tab.
            using Process process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = "chrome";
            process.StartInfo.Arguments = outputFilePath;
            process.Start();
        }
    }

    private static string SaveFile(PdfDocument pdfDocument, string outputFileName)
    {
        var outputFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), outputFileName);

        using (var generatedPdfStream = File.Open(outputFile, FileMode.Create, FileAccess.Write))
        {
            pdfDocument.Save(generatedPdfStream);
        }

        return outputFile;
    }
}
