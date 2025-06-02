using Microsoft.Maui.Controls;
using Routines.Utils;
using Routines.Utils.Background;

namespace Routines.Views
{
    public partial class MainMenuPage : ContentPage
    {
        public MainMenuPage()
        {
            InitializeComponent();
            BindingContext = App.LocManager;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = App.LocManager;

            SaludoLabel.Text = $"{App.LocManager["Greeting"]}, {Session.UsuarioActual?.Usuario ?? "Usuario"}";

            // Aplicar fondo al entrar
            SetUserBackground();

            // Suscribirse a cambios de fondo
            MessagingCenter.Subscribe<SettingsPage>(this, AppMessages.BackgroundChanged, sender =>
            {
                SetUserBackground();
            });
        }

        private void SetUserBackground()
        {
            var bg = Session.UsuarioActual?.Background ?? "blue";
            BackgroundImage.Source = $"{bg}.png";
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<SettingsPage>(this, AppMessages.BackgroundChanged);
        }

        private async void OnHabitsClicked(object sender, EventArgs e) =>
            await Navigation.PushAsync(new HabitsListPage());

        private async void OnAddHabitClicked(object sender, EventArgs e) =>
            await Navigation.PushAsync(new AddHabitPage());

        private async void OnCalendarClicked(object sender, EventArgs e) =>
            await Navigation.PushAsync(new CalendarPage());

        private async void OnProgressClicked(object sender, EventArgs e) =>
            await Navigation.PushAsync(new ProgressPage());

        private async void OnExportClicked(object sender, EventArgs e) =>
            await Navigation.PushAsync(new ExportPage());

        private async void OnSettingsClicked(object sender, EventArgs e) =>
            await Navigation.PushAsync(new SettingsPage());
    }
}
