using Microsoft.Maui.Controls;
using System.Text;
using System.IO;
using Routines.Models;
using Routines.Utils;

#if ANDROID
using Android.App;
using Routines.Platforms.Android.Exporters;
#endif

namespace Routines.Views
{
    public partial class ExportPage : ContentPage
    {
        public ExportPage()
        {
            InitializeComponent();
        }

        private async void OnExportClicked(object sender, EventArgs e)
        {
#if ANDROID
            var usuario = Session.UsuarioActual?.Usuario ?? "Desconocido";
            var habitos = await App.Database.GetHabitsByUserAsync(Session.UsuarioActual.Id);
            var checks = await App.Database.GetChecksByUserAsync(Session.UsuarioActual.Id);

            var context = Platform.CurrentActivity ?? Android.App.Application.Context;
            AndroidPdfExporter.CrearInforme(context, usuario, habitos, checks);
#else
    await DisplayAlert("Error", "Exportación solo disponible en Android", "OK");
#endif
        }
    }
}
