using System.Diagnostics;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;

namespace SyncfusionGenerateLargePdf;

public static class ChangeProducerHelper
{
    public static async Task ChangeProducerAsync(string inputFile)
    {
        var swTotal = Stopwatch.StartNew();

        try
        {
            var inputFileDir = Path.GetDirectoryName(inputFile);
            var fileNameNoExt = Path.GetFileNameWithoutExtension(inputFile);

            var outputFileName = $"{fileNameNoExt}_fbproducer{Path.GetExtension(inputFile)}";
            var outputFilePath = Path.Combine(inputFileDir!, outputFileName);

            //Console.WriteLine(inputFileDir);
            //Console.WriteLine(fileNameNoExt);
            //Console.WriteLine(outputFilePath);

            using var inputFileStream = File.OpenRead(inputFile);

            // Load the PDF document.
            Console.WriteLine($"[{swTotal.Elapsed}] Load PDF start...");
            using var pdfDoc = new PdfLoadedDocument(file: inputFileStream);
            Console.WriteLine($"[{swTotal.Elapsed}] Load PDF end.");

            // Ensure we're listed as the Producer of the PDF.
            Console.WriteLine($"[{swTotal.Elapsed}] Set Producer start...");
            SetFastBoundAsProducer(pdfDoc);
            Console.WriteLine($"[{swTotal.Elapsed}] Set Producer end.");

            // Create a new output file.
            Console.WriteLine($"[{swTotal.Elapsed}] Write output file start...");
            using var outputFileStream = File.OpenWrite(outputFilePath);
            pdfDoc.Save(outputFileStream);
            Console.WriteLine($"[{swTotal.Elapsed}] Write output file end.");

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
            await Task.CompletedTask;
        }
        finally
        {
            swTotal.Stop();
            Console.WriteLine($"Changing Producer took {swTotal.Elapsed}.");
        }
    }

    /// <summary>
    /// Ensure FastBound is listed as the Producer of the PDF.
    /// </summary>
    public static void SetFastBoundAsProducer(PdfDocumentBase pdfDocument)
    {
        //Check.NotNull(pdfDocument);

        var producer = "FastBound - Firearms Compliance Software";
        pdfDocument.DocumentInformation.Producer = producer;
        pdfDocument.DocumentInformation.XmpMetadata.PDFSchema.Producer = producer;
    }
}
