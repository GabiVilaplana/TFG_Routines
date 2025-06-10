using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

#if ANDROID
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Routines; // Para acceder a MainActivity.Instance
#endif

namespace Routines.Debug
{
    internal static class DebugTools
    {
        public static async Task ExportarBaseDeDatosAsync()
        {
            try
            {
                string dbPath = Path.Combine(
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
                    "Routines.db3");

#if ANDROID
                var activity = MainActivity.Instance ?? throw new Exception("No hay actividad activa");

                if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(activity, new[] { Manifest.Permission.WriteExternalStorage }, 1);
                    await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Permiso requerido",
                        "Otorga permiso de almacenamiento para exportar.", "OK");
                    return;
                }

                string downloadPath = Path.Combine(
                    Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath,
                    "Routines_Export.db3");

                File.Copy(dbPath, downloadPath, overwrite: true);

                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Exportación completada",
                    $"Archivo guardado en:\n{downloadPath}", "OK");
#else
                await Application.Current.MainPage.DisplayAlert("Error", "Exportación solo disponible en Android.", "OK");
#endif
            }
            catch (Exception ex)
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Error al exportar", ex.Message, "OK");
            }
        }

        public static async Task ImportarBaseDeDatosAsync()
        {
            try
            {
#if ANDROID
        var activity = MainActivity.Instance ?? throw new Exception("No hay actividad activa");

        if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions(activity, new[] { Manifest.Permission.ReadExternalStorage }, 1);
            await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Permiso requerido",
                "Otorga permiso de almacenamiento para importar.", "OK");
            return;
        }

        string importPath = Path.Combine(
            Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath,
            "Routines_Import.db3");

        string appDbPath = Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
            "Routines.db3");

        if (!File.Exists(importPath))
        {
            await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Error",
                $"No se encontró el archivo:\n{importPath}", "OK");
            return;
        }

        // Este try interno asegura que si falla el Copy, no se muestra el mensaje de éxito
        try
        {
            File.Copy(importPath, appDbPath, overwrite: true);
        }
        catch (Exception copyEx)
        {
            await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Error al copiar la base de datos", copyEx.Message, "OK");
            return;
        }

        await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Importación completada",
            "La base de datos fue reemplazada correctamente.", "OK");
#else
                await Application.Current.MainPage.DisplayAlert("Error",
                    "Función de importación solo disponible en Android.", "OK");
#endif
            }
            catch (Exception ex)
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Error inesperado al importar",
                    ex.Message, "OK");
            }
        }
    }
}
