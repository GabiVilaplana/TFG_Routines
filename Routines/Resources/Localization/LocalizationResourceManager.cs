using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using Routines.Utils;

namespace Routines.Resources.Localization
{
    public class LocalizationResourceManager : INotifyPropertyChanged
    {
        private readonly ResourceManager _resourceManager;
        private CultureInfo _currentCulture;


        public LocalizationResourceManager(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            _currentCulture = CultureInfo.CurrentUICulture;
        }

        public string this[string text] => _resourceManager.GetString(text, _currentCulture) ?? $"[{text}]";

        public void SetCulture(CultureInfo culture)
        {
            _currentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;

            // 🔁 Notificar cambio global para refrescar TODAS las propiedades
            OnPropertyChanged(null); // esto es clave para que funcione en las vistas
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public string GreetingLabel => $"{this["Greeting"]}, {Session.UsuarioActual?.Usuario ?? "Usuario"}";
        public string MainMenuPrompt => this["MainMenuPrompt"];
        public string MenuMyHabits => this["MenuMyHabits"];
        public string MenuAddHabit => this["MenuAddHabit"];
        public string MenuCalendar => this["MenuCalendar"];
        public string MenuProgress => this["MenuProgress"];
        public string MenuExport => this["MenuExport"];
        public string MenuSettings => this["MenuSettings"];
        public string Routines => this["Routines"]; 
        public string ExportTitle => this["ExportTitle"];
        public string GenerateReport => this["GenerateReport"];
        public string ExportToPDF => this["ExportToPDF"];
    }
}
