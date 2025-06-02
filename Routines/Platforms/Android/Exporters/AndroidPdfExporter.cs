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
            var paint = new AndroidGraph.Paint();
            var pageInfo = new PdfDocument.PageInfo.Builder(595, 842, 1).Create(); // A4

            var page = pdfDoc.StartPage(pageInfo);
            var canvas = page.Canvas;

            int x = 40;
            int y = 60;

            paint.TextSize = 20;
            paint.FakeBoldText = true;
            canvas.DrawText($"{App.LocManager["ReportTitle"]} – {usuario}", x, y, paint);

            paint.TextSize = 14;
            paint.FakeBoldText = false;

            y += 40;
            canvas.DrawText($"{App.LocManager["Date"]}: {DateTime.Now:dd/MM/yyyy}", x, y, paint);

            y += 40;
            canvas.DrawText($"📋 {App.LocManager["CreatedHabits"]}: {habitos.Count}", x, y, paint);

            y += 30;
            canvas.DrawText($"✅ {App.LocManager["RecordedCompliances"]}: {checks.Count}", x, y, paint);

            y += 30;
            canvas.DrawText($"🏆 {App.LocManager["TopHabits"]}:", x, y, paint);

            var top = checks
                .GroupBy(c => c.HabitTitulo)
                .Select(g => new { Titulo = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(3)
                .ToList();

            foreach (var h in top)
            {
                y += 25;
                canvas.DrawText($"- {h.Titulo}: {h.Count} {App.LocManager["Times"]}", x + 20, y, paint);
            }

            y += 40;
            canvas.DrawText($"📄 {App.LocManager["ComplianceList"]}:", x, y, paint);

            foreach (var c in checks.OrderByDescending(c => c.Fecha))
            {
                y += 20;
                if (y > 800) break; // evitar salir del borde
                canvas.DrawText($"{c.HabitTitulo} – {c.Fecha:dd/MM/yyyy}", x + 20, y, paint);
            }

            pdfDoc.FinishPage(page);

            // Guardar en carpeta pública Downloads
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
