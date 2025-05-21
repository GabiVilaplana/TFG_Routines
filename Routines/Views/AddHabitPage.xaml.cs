using Microsoft.Maui.Controls;
using Routines.Models;
using Routines.Utils;

namespace Routines.Views
{
    public partial class AddHabitPage : ContentPage
    {
        public AddHabitPage()
        {
            InitializeComponent();
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
            await DisplayAlert("Guardado", "El hábito ha sido creado correctamente.", "OK");
            await Navigation.PopAsync();
        }
    }
}