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
                await DisplayAlert("Error", App.LocManager["SessionNotFound"], "OK");
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

        private async void OnHabitSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Habit selectedHabit)
            {
                await Navigation.PushAsync(new EditHabitPage(selectedHabit));
                HabitsList.SelectedItem = null;
            }
        }

        private async void OnHabitTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is Habit habit)
            {
                await Navigation.PushAsync(new EditHabitPage(habit));
            }
        }

        private async void OnHabitCheckClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Habit habit)
            {
                bool hechoHoy = await App.Database.IsHabitDoneTodayAsync(habit.Id, Session.UsuarioActual.Id);

                if (hechoHoy)
                {
                    bool confirmarEliminar = await DisplayAlert(
                        App.LocManager["Remove"],
                        string.Format(App.LocManager["RemoveConfirm"], habit.Titulo),
                        App.LocManager["Yes"],
                        App.LocManager["No"]
                    );

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
                    HabitTitulo = habit.Titulo
                };

                await App.Database.AddHabitCheckAsync(check);
                await DisplayAlert(App.LocManager["Registered"], string.Format(App.LocManager["HabitMarkedDone"], habit.Titulo), "OK");
                LoadHabits();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadHabits();
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
