using Microsoft.Maui.Controls;
using Routines.Models;
using Routines.Resources.Localization;
using Routines.Utils;

namespace Routines.Views
{
    public partial class AddHabitPage : ContentPage
    {
        public AddHabitPage()
        {
            InitializeComponent();
            BindingContext = App.LocManager;

            // Cargar categorías traducidas
            CategoriaPicker.ItemsSource = new List<string>
            {
                App.LocManager["Health"],
                App.LocManager["Study"],
                App.LocManager["Work"],
                App.LocManager["Exercise"],
                App.LocManager["Leisure"]
            };

            FrecuenciaPicker.ItemsSource = new List<string>
            {
                App.LocManager["Daily"],
                App.LocManager["Weekly"],
                App.LocManager["Monthly"]
            };
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            var nuevo = new Habit
            {
                Titulo = TituloEntry.Text,
                Categoria = CategoriaPicker.SelectedItem?.ToString(),
                Frecuencia = FrecuenciaPicker.SelectedItem?.ToString(),
                UsuarioId = Session.UsuarioActual.Id,
                FechaAsignada = FechaAsignadaPicker.Date.Date
            };

            await App.Database.AddHabitAsync(nuevo);

            await DisplayAlert(App.LocManager["Success"], App.LocManager["HabitCreated"], App.LocManager["OK"]);
            await Navigation.PopAsync();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetUserBackground();

            MessagingCenter.Subscribe<SettingsPage>(this, AppMessages.BackgroundChanged, sender =>
            {
                SetUserBackground();
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<SettingsPage>(this, AppMessages.BackgroundChanged);
        }

        private void SetUserBackground()
        {
            var bg = Session.UsuarioActual?.Background ?? "blue";
            BackgroundImage.Source = $"{bg}.png";
        }
    }
}
