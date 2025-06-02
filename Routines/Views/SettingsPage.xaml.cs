using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Routines.Utils;
using System.Globalization;

namespace Routines.Views
{
    public partial class SettingsPage : ContentPage
    {
        readonly Dictionary<string, string> BackgroundImages = new()
        {
            ["blue"] = "blue.png",
            ["green"] = "green.png",
            ["red"] = "red.png",
            ["yellow"] = "yellow.png",
            ["orange"] = "orange.png",
            ["purple"] = "purple.png"
        };

        // 🧠 Flag para evitar bucles infinitos
        private bool _isReloading = false;

        public SettingsPage()
        {
            InitializeComponent();

            BindingContext = App.LocManager;

            var savedBg = Session.UsuarioActual?.Background ?? "blue";
            ApplyBackground(savedBg);

            var savedLang = Session.UsuarioActual?.Language ?? "es";
            LanguagePicker.SelectedIndex = savedLang == "es" ? 0 : 1;
        }

        private async void OnBackgroundSelected(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is string selectedBg && BackgroundImages.ContainsKey(selectedBg))
            {
                ApplyBackground(selectedBg);

                Session.UsuarioActual.Background = selectedBg;
                await App.Database.UpdateUserAsync(Session.UsuarioActual);

                MessagingCenter.Send(this, AppMessages.BackgroundChanged);
            }
        }

        private void ApplyBackground(string bgName)
        {
            if (BackgroundImages.TryGetValue(bgName, out var imageName))
                BackgroundImage.Source = imageName;
        }

        private async void OnLanguageSelected(object sender, EventArgs e)
        {
            // ⛔️ Evita bucles infinitos
            if (_isReloading)
                return;

            if (LanguagePicker.SelectedIndex == -1)
                return;

            var selectedLang = LanguagePicker.SelectedIndex == 0 ? "es" : "en";

            // Evitar que se vuelva a lanzar el evento al recargar
            if (selectedLang == Session.UsuarioActual?.Language)
                return;

            Session.UsuarioActual.Language = selectedLang;
            await App.Database.UpdateUserAsync(Session.UsuarioActual);

            var culture = new CultureInfo(selectedLang);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            App.LocManager.SetCulture(culture);

            await DisplayAlert(
                App.LocManager["LanguageChanged"],
                App.LocManager["LanguageChangedAdvice"],
                App.LocManager["OK"]
            );

            _isReloading = true;

            // 🧠 Recargar vista sin provocar bucle
            var nuevaPagina = new SettingsPage();
            if (Parent is NavigationPage nav)
            {
                await nav.Navigation.PushAsync(nuevaPagina);
                await Task.Delay(100); // deja respirar al thread antes de salir
                nav.Navigation.RemovePage(this);
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(nuevaPagina);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var savedBg = Session.UsuarioActual?.Background ?? "blue";
            ApplyBackground(savedBg);

            var savedLang = Session.UsuarioActual?.Language ?? "es";
            LanguagePicker.SelectedIndex = savedLang == "es" ? 0 : 1;
        }
    }
}
