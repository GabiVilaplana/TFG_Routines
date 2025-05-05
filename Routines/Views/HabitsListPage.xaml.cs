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

        private void LoadHabits()
        {
            _ = LoadHabitsAsync(); // Llama sin esperar para no bloquear OnAppearing
        }

        private async Task LoadHabitsAsync()
        {
            if (Session.UsuarioActual == null)
            {
                await DisplayAlert("Error", "Sesión no encontrada. Por favor, inicia sesión nuevamente.", "OK");
                await Navigation.PopAsync();
                return;
            }

            var allHabits = await App.Database.GetHabitsByUserAsync(Session.UsuarioActual.Id);
            HabitsList.ItemsSource = allHabits;
            EmptyMessage.IsVisible = allHabits == null || allHabits.Count == 0;
        }

        private async void OnHabitSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Habit selectedHabit)
            {
                await Navigation.PushAsync(new EditHabitPage(selectedHabit));
                HabitsList.SelectedItem = null;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadHabits();
        }
    }
}
