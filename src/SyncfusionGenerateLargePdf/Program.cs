using SyncfusionGenerateLargePdf;


//
// All times are from my local dev machine.
//

// Uncomment one line at a time.
// The method will launch the generated PDF in Chrome.

GeneratePdfHelper.GenerateGridPdf(itemCount: 10);             // 00:00:00.42 
//GeneratePdfHelper.GenerateGridPdf(itemCount: 100);            // 00:00:00.52
//GeneratePdfHelper.GenerateGridPdf(itemCount: 1_000);          // 00:00:01.53
//GeneratePdfHelper.GenerateGridPdf(itemCount: 10_000);         // 00:00:12.18
//GeneratePdfHelper.GenerateGridPdf(itemCount: 100_000);        // 00:10:07.12
//GeneratePdfHelper.GenerateGridPdf(itemCount: 1_600_000);      // Cancelled after 45 minutes and only ~250k rows.
