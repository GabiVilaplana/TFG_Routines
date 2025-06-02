using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

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
            OnPropertyChanged(string.Empty); // Refresca bindings
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
