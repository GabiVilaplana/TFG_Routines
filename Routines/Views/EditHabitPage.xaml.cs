using Microsoft.Maui.Controls;
using Routines.Models;
using Routines.Utils;

namespace Routines.Views
{
    public partial class EditHabitPage : ContentPage
    {
        //private readonly Habit _habitOriginal;
        private Habit? _habitOriginal;

        public EditHabitPage(Habit habit)
        {
            InitializeComponent();

            if (habit == null)
                throw new ArgumentNullException(nameof(habit), "El hábito no puede ser nulo");

            _habitOriginal = habit;
            CargarDatos();
        }

        private void CargarDatos()
        {
            var categorias = new List<string> { "Salud", "Estudio", "Trabajo", "Ocio", "Otro" };
            var frecuencias = new List<string> { "Diaria", "Semanal", "Mensual" };

            CategoriaPicker.ItemsSource = categorias;
            FrecuenciaPicker.ItemsSource = frecuencias;

            TituloEntry.Text = _habitOriginal.Titulo;

            if (categorias.Contains(_habitOriginal.Categoria))
                CategoriaPicker.SelectedItem = _habitOriginal.Categoria;

            if (frecuencias.Contains(_habitOriginal.Frecuencia))
                FrecuenciaPicker.SelectedItem = _habitOriginal.Frecuencia;
        }


        private async void OnGuardarCambiosClicked(object sender, EventArgs e)
        {
            _habitOriginal.Titulo = TituloEntry.Text?.Trim();
            //_habitOriginal.Categoria = CategoriaPicker.SelectedItem?.ToString();
            //_habitOriginal.Frecuencia = FrecuenciaPicker.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(_habitOriginal.Titulo))
            {
                await DisplayAlert("Error", "El título no puede estar vacío.", "OK");
                return;
            }

            await App.Database.UpdateHabitAsync(_habitOriginal);
            await DisplayAlert("Éxito", "Hábito actualizado correctamente.", "OK");
            await Navigation.PopAsync();
        }

        private async void OnEliminarClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirmación", "¿Estás seguro de que quieres eliminar este hábito?", "Sí", "No");
            if (confirm)
            {
                await App.Database.DeleteHabitAsync(_habitOriginal);
                await DisplayAlert("Eliminado", "Hábito eliminado correctamente.", "OK");
                await Navigation.PopAsync();
            }
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
