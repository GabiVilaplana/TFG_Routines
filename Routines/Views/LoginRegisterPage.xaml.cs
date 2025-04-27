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
                await DisplayAlert("Éxito", $"Bienvenido {user.Usuario}", "OK");
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var nuevoUsuario = new Models.User
            {
                Usuario = UsernameEntry.Text,
                Contraseña = PasswordEntry.Text
            };

            await App.Database.AddUserAsync(nuevoUsuario);
            await DisplayAlert("Registrado", "Usuario creado correctamente", "OK");
        }
    }
}
