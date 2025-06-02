using Microsoft.Maui.Controls;
using Routines.Models;
using Routines.Utils;
using System;
using System.Linq;

namespace Routines.Views
{
    public partial class HabitChecksPage : ContentPage
    {
        public HabitChecksPage()
        {
            InitializeComponent();
            LoadChecks();
        }

        private async void LoadChecks()
        {
            var usuarioId = Session.UsuarioActual?.Id ?? 0;
            var checks = await App.Database.GetChecksByUserAsync(usuarioId);

            // Mostrar primero los más recientes
            ChecksList.ItemsSource = checks
                .OrderByDescending(c => c.Fecha)
                .ToList();
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
