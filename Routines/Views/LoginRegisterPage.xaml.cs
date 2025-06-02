using Microsoft.Maui.Controls;
using System.Globalization;


namespace Routines
{
    public partial class LoginRegisterPage : ContentPage
    {
        public LoginRegisterPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var user = await App.Database.GetUserAsync(UsernameEntry.Text, PasswordEntry.Text);
            if (user != null)
            {
                await DisplayAlert("Éxito", $"Bienvenido {user.Usuario}", "OK");
                Routines.Utils.Session.UsuarioActual = user;

                var lang = user.Language ?? "en";
                var culture = new CultureInfo(lang);
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
                App.LocManager.SetCulture(culture);

                // Enviar mensaje para que todas las páginas carguen el fondo del usuario logueado
                MessagingCenter.Send(this, Routines.Utils.AppMessages.BackgroundChanged);

                Application.Current.MainPage = new NavigationPage(new Routines.Views.MainMenuPage());
            }
            else
            {
                await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            try
            {
                var nuevoUsuario = new Models.User
                {
                    Usuario = UsernameEntry.Text,
                    Contraseña = PasswordEntry.Text
                };

                await App.Database.AddUserAsync(nuevoUsuario);
                await DisplayAlert("Registrado", "Usuario creado correctamente", "OK");
            }
            catch (ArgumentException ex)
            {
                await DisplayAlert("Validación", ex.Message, "OK");
            }
            catch (InvalidOperationException ex)
            {
                await DisplayAlert("Duplicado", ex.Message, "OK");
            }
        }
        private void SetAppCulture(string lang)
        {
            var culture = new CultureInfo(lang);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }
    }
}
