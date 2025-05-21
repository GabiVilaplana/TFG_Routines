using Microsoft.Maui.Controls;
using Routines.Models;
using Routines.Utils;

namespace Routines.Views
{
    public partial class HabitsListPage : ContentPage
    {
        public HabitsListPage()
        {
            InitializeComponent();
            LoadHabits();
        }

        private async void LoadHabits()
        {
            if (Session.UsuarioActual == null)
            {
                await DisplayAlert("Error", "Sesión no encontrada.", "OK");
                await Navigation.PopAsync();
                return;
            }

            var usuarioId = Session.UsuarioActual.Id;

            var allHabits = await App.Database.GetHabitsByUserAsync(usuarioId);
            var checksHoy = await App.Database.GetChecksByUserAsync(usuarioId);

            var hoy = DateTime.Today;

            foreach (var habit in allHabits)
            {
                habit.EstaHechoHoy = checksHoy.Any(c => c.HabitId == habit.Id && c.Fecha.Date == hoy);
            }

            HabitsList.ItemsSource = allHabits;
            EmptyMessage.IsVisible = allHabits.Count == 0;
        }


        private async void OnHabitSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Habit selectedHabit)
            {
                await Navigation.PushAsync(new EditHabitPage(selectedHabit));
                HabitsList.SelectedItem = null;
            }
        }

        private async void OnHabitCheckClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Habit habit)
            {
                bool hechoHoy = await App.Database.IsHabitDoneTodayAsync(habit.Id, Session.UsuarioActual.Id);

                if (hechoHoy)
                {
                    bool confirmarEliminar = await DisplayAlert("Eliminar hábito", $"¿Eliminar '{habit.Titulo}' de la lista?", "Sí", "No");
                    if (confirmarEliminar)
                    {
                        await App.Database.DeleteHabitAsync(habit);
                        LoadHabits();
                    }
                    return;
                }

                var check = new HabitCheck
                {
                    HabitId = habit.Id,
                    Fecha = DateTime.Today,
                    UsuarioId = Session.UsuarioActual.Id,
                    HabitTitulo = habit.Titulo // ?? Esto es esencial
                };

                await App.Database.AddHabitCheckAsync(check);
                await DisplayAlert("Registrado", $"Hábito '{habit.Titulo}' marcado como hecho hoy.", "OK");
                LoadHabits();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadHabits();
        }
    }
}
