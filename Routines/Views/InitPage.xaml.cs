using Microsoft.Maui.Controls;
using Routines.Debug;
using System.Timers;

namespace Routines.Views
{
    public partial class InitPage : ContentPage
    {
        int tapCount = 0;
        DateTime lastTapTime = DateTime.MinValue;
        System.Timers.Timer longPressTimer;

        public InitPage()
        {
            InitializeComponent();
            AnimateText();

        }

        private async void AnimateText()
        {
            while (true)
            {
                await TapToContinueLabel.FadeTo(0.3, 1000);
                await TapToContinueLabel.FadeTo(1.0, 1000);
            }
        }

        private void TapArea_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginRegisterPage());
        }

        private async void LogoImage_Tapped(object sender, EventArgs e)
        {
            var now = DateTime.Now;

            if ((now - lastTapTime).TotalMilliseconds < 600)
                tapCount++;
            else
                tapCount = 1;

            lastTapTime = now;

            if (tapCount >= 3)
            {
                tapCount = 0;
                await Debug.DebugTools.ExportarBaseDeDatosAsync();
            }
        }
        private async void BackgroundImage_Tapped(object sender, EventArgs e)
        {
            var now = DateTime.Now;

            if ((now - lastTapTime).TotalMilliseconds < 600)
                tapCount++;
            else
                tapCount = 1;

            lastTapTime = now;

            if (tapCount >= 5)
            {
                tapCount = 0;
                await DebugTools.ImportarBaseDeDatosAsync();
                await DisplayAlert("Importación", "Base de datos importada correctamente", "OK");
            }
        }
    }
}
