using Microsoft.Maui.Controls;

namespace Routines.Views
{
    public partial class InitPage : ContentPage
    {
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
    }
}
