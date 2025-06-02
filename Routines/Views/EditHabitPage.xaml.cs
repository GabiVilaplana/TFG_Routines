 using Microsoft.Maui.Controls;
using Routines.Models;
using Routines.Resources.Localization;
using Routines.Utils;

namespace Routines.Views
{
    public partial class EditHabitPage : ContentPage
    {
        private Habit? _habitOriginal;

        public EditHabitPage(Habit habit)
        {
            InitializeComponent();

            BindingContext = App.LocManager;

            if (habit == null)
                throw new ArgumentNullException(nameof(habit), "The habit cannot be null");

            _habitOriginal = habit;
            CargarDatos();
        }

        private void CargarDatos()
        {
            var categorias = new List<string>
            {
                App.LocManager["Health"],
                App.LocManager["Study"],
                App.LocManager["Work"],
                App.LocManager["Exercise"],
                App.LocManager["Leisure"],
            };

            var frecuencias = new List<string>
            {
                App.LocManager["Daily"],
                App.LocManager["Weekly"],
                App.LocManager["Monthly"]
            };

            CategoriaPicker.ItemsSource = categorias;
            FrecuenciaPicker.ItemsSource = frecuencias;
            FechaAsignadaPicker.Date = _habitOriginal.FechaAsignada ?? DateTime.Today;

            TituloEntry.Text = _habitOriginal.Titulo;

            if (categorias.Contains(_habitOriginal.Categoria))
                CategoriaPicker.SelectedItem = _habitOriginal.Categoria;

            if (frecuencias.Contains(_habitOriginal.Frecuencia))
                FrecuenciaPicker.SelectedItem = _habitOriginal.Frecuencia;
        }


        private async void OnGuardarCambiosClicked(object sender, EventArgs e)
        {
            _habitOriginal.Titulo = TituloEntry.Text?.Trim();
            _habitOriginal.FechaAsignada = FechaAsignadaPicker.Date;

            if (string.IsNullOrWhiteSpace(_habitOriginal.Titulo))
            {
                await DisplayAlert(App.LocManager["Error"], App.LocManager["EmptyTitleMessage"], "OK");
                return;
            }

            await App.Database.UpdateHabitAsync(_habitOriginal);
            await DisplayAlert(App.LocManager["Success"], App.LocManager["HabitUpdated"], "OK");
            await Navigation.PopAsync();
        }

        private async void OnEliminarClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert(
                App.LocManager["Confirmation"],
                App.LocManager["DeleteConfirm"],
                App.LocManager["Yes"],
                App.LocManager["No"]
            );

            if (confirm)
            {
                await App.Database.DeleteHabitAsync(_habitOriginal);
                await DisplayAlert(App.LocManager["Removing"], App.LocManager["HabitDeleted"], "OK");
                await Navigation.PopAsync();
            }
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
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
