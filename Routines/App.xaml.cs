using Routines.Data;
using Routines.Resources.Localization;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Routines
{
    public partial class App : Application
    {
        public static LocalizationResourceManager LocManager { get; private set; }

        private static Database database;

        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Routines.db3");
                    //Debug.WriteLine($"Ruta de la base de datos: {dbPath}");
                    database = new Database(dbPath);
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            LocManager = new LocalizationResourceManager(AppResources.ResourceManager);
            Current.Resources["Loc"] = LocManager;

            MainPage = new AppShell();
        }
    }
}
