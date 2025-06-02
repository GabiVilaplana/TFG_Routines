using Microsoft.Maui.Controls;
using Routines.Models;
using Routines.Utils;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Routines.Views
{
    public partial class ProgressPage : ContentPage
    {
        public ProgressPage()
        {
            InitializeComponent();
        }

        private async void LoadStats()
        {
            var usuarioId = Session.UsuarioActual?.Id ?? 0;

            var habitos = await App.Database.GetHabitsByUserAsync(usuarioId);
            var checks = await App.Database.GetChecksByUserAsync(usuarioId);

            var checksValidos = checks.Where(c => !string.IsNullOrEmpty(c.HabitTitulo)).ToList();

            HabitosCreadosLabel.Text = $"📋 {App.LocManager["CreatedHabits"]}: {habitos.Count}";
            CumplimientosRegistradosLabel.Text = $"✅ {App.LocManager["RecordedCompliances"]}: {checks.Count}";

            var top = checksValidos
                .GroupBy(c => c.HabitTitulo)
                .Select(g => new { Titulo = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(3)
                .ToList();

            Top1Label.Text = top.Count > 0 ? $"1. {top[0].Titulo}     {top[0].Count} {App.LocManager["Times"]}" : "1. -";
            Top2Label.Text = top.Count > 1 ? $"2. {top[1].Titulo}     {top[1].Count} {App.LocManager["Times"]}" : "2. -";
            Top3Label.Text = top.Count > 2 ? $"3. {top[2].Titulo}     {top[2].Count} {App.LocManager["Times"]}" : "3. -";
        }

        private async void OnVerCumplimientosTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HabitChecksPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadStats();
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
