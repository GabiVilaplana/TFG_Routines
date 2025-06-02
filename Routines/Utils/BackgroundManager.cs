using System.ComponentModel;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;

namespace Routines.Utils.Background
{
    public class BackgroundManager : INotifyPropertyChanged
    {
        private const string BackgroundPrefKey = "AppBackground";

        // Singleton
        private static BackgroundManager _instance;
        public static BackgroundManager Instance => _instance ??= new BackgroundManager();

        private string _currentBackgroundName;

        public event PropertyChangedEventHandler PropertyChanged;

        private BackgroundManager()
        {
            _currentBackgroundName = Preferences.Get(BackgroundPrefKey, "blue");
        }

        public string CurrentBackgroundName
        {
            get => _currentBackgroundName;
            private set
            {
                if (_currentBackgroundName != value)
                {
                    _currentBackgroundName = value;
                    Preferences.Set(BackgroundPrefKey, value);
                    OnPropertyChanged(nameof(CurrentBackgroundName));
                    OnPropertyChanged(nameof(CurrentBackgroundImage));
                }
            }
        }

        public ImageSource CurrentBackgroundImage
        {
            get => ImageSource.FromFile(GetBackgroundImageName(_currentBackgroundName));
        }

        private string GetBackgroundImageName(string name)
        {
            return name switch
            {
                "blue" => "blue.png",
                "green" => "green.png",
                "red" => "red.png",
                "yellow" => "yellow.png",
                "orange" => "orange.png",
                "purple" => "purple.png",
                _ => "blue.png"
            };
        }

        public void SetBackground(string name)
        {
            CurrentBackgroundName = name;
        }

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
