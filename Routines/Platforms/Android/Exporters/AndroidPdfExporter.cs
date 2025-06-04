#if ANDROID
using Android.Content;
using Android.Graphics;
using Android.Graphics.Pdf;
using Android.OS;
using Android.Widget;
using Java.IO;
using Routines.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using AndroidGraph = Android.Graphics;
using AndroidEnv = Android.OS.Environment;
using JavaFile = Java.IO.File;

namespace Routines.Platforms.Android.Exporters
{
    public class AndroidPdfExporter
    {
        public static void CrearInforme(Context context, string usuario, List<Habit> habitos, List<HabitCheck> checks)
        {
            var pdfDoc = new PdfDocument();
            var pageInfo = new PdfDocument.PageInfo.Builder(595, 842, 1).Create(); // A4
            var page = pdfDoc.StartPage(pageInfo);
            var canvas = page.Canvas;

            int x = 40;
            int y = 60;

            var headerPaint = new AndroidGraph.Paint
            {
                Color = AndroidGraph.Color.Rgb(63, 81, 181),
                TextSize = 22,
                FakeBoldText = true
            };

            var sectionTitlePaint = new AndroidGraph.Paint
            {
                Color = AndroidGraph.Color.Black,
                TextSize = 18,
                FakeBoldText = true
            };

            var bodyPaint = new AndroidGraph.Paint
            {
                Color = AndroidGraph.Color.Black,
                TextSize = 14
            };

            var highlightPaint = new AndroidGraph.Paint
            {
                Color = AndroidGraph.Color.Rgb(232, 245, 233)
            };

            // Título
            canvas.DrawText($"{App.LocManager["ReportTitle"]} – {usuario}", x, y, headerPaint);

            y += 30;
            canvas.DrawText($"{App.LocManager["Date"]}: {DateTime.Now:dd/MM/yyyy}", x, y, bodyPaint);

            y += 30;
            canvas.DrawText($"📋 {App.LocManager["CreatedHabits"]}: {habitos.Count}", x, y, bodyPaint);

            y += 25;
            canvas.DrawText($"✅ {App.LocManager["RecordedCompliances"]}: {checks.Count}", x, y, bodyPaint);

            y += 35;
            canvas.DrawText($"🏆 {App.LocManager["TopHabits"]}:", x, y, sectionTitlePaint);

            var top = checks
                .GroupBy(c => c.HabitTitulo)
                .Select(g => new { Titulo = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(3)
                .ToList();

            foreach (var h in top)
            {
                y += 22;
                canvas.DrawText($"- {h.Titulo}: {h.Count} {App.LocManager["Times"]}", x + 20, y, bodyPaint);
            }

            y += 35;
            canvas.DrawText($"📄 {App.LocManager["ComplianceList"]}:", x, y, sectionTitlePaint);

            var checksOrdered = checks.OrderByDescending(c => c.Fecha).ToList();
            foreach (var (c, i) in checksOrdered.Select((c, i) => (c, i)))
            {
                y += 20;

                if (y > 800) break;

                if (i % 2 == 0)
                {
                    // Rectángulo de fondo alterno
                    canvas.DrawRect(x - 10, y - 15, 550, y + 5, highlightPaint);
                }

                canvas.DrawText($"{c.HabitTitulo} – {c.Fecha:dd/MM/yyyy}", x + 10, y, bodyPaint);
            }

            pdfDoc.FinishPage(page);

            // Guardar
            var fileName = $"Informe_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            var file = new JavaFile(AndroidEnv.GetExternalStoragePublicDirectory(AndroidEnv.DirectoryDownloads), fileName);

            using (var stream = System.IO.File.Create(file.AbsolutePath))
            {
                pdfDoc.WriteTo(stream);
            }

            pdfDoc.Close();

            Toast.MakeText(context, $"{App.LocManager["PdfGenerated"]}: {file.AbsolutePath}", ToastLength.Long).Show();
        }
    }
}
#endif
