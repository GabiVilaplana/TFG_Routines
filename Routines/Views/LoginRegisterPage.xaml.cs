using Microsoft.Maui.Controls;


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
                await DisplayAlert("�xito", $"Bienvenido {user.Usuario}", "OK");
                Routines.Utils.Session.UsuarioActual = user;
                Application.Current.MainPage = new NavigationPage(new Routines.Views.MainMenuPage());
            }
            else
            {
                await DisplayAlert("Error", "Usuario o contrase�a incorrectos", "OK");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            try
            {
                var nuevoUsuario = new Models.User
                {
                    Usuario = UsernameEntry.Text,
                    Contrase�a = PasswordEntry.Text
                };

                await App.Database.AddUserAsync(nuevoUsuario);
                await DisplayAlert("Registrado", "Usuario creado correctamente", "OK");
            }
            catch (ArgumentException ex)
            {
                await DisplayAlert("Validaci�n", ex.Message, "OK");
            }
            catch (InvalidOperationException ex)
            {
                await DisplayAlert("Duplicado", ex.Message, "OK");
            }
        }
    }
}
