using Microsoft.Maui.Controls;
using Routines.Utils;

namespace Routines.Views
{
    public partial class MainMenuPage : ContentPage
    {
        public string UsuarioNombre => Session.UsuarioActual?.Usuario ?? "Usuario";

        public MainMenuPage()
        {
            InitializeComponent();
            SaludoLabel.Text = $"Hola, {Session.UsuarioActual?.Usuario ?? "Usuario"}";
        }

        private async void OnHabitsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HabitsListPage());
        }

        private async void OnAddHabitClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddHabitPage());
        }

        private async void OnCalendarClicked(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new CalendarPage());
        }

        private async void OnProgressClicked(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new ProgressPage());
        }

        private async void OnExportClicked(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new ExportPage());
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new SettingsPage());
        }
    }
}
