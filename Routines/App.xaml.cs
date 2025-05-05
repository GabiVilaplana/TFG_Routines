using Routines.Data;
using System.IO;
using System.Diagnostics;

namespace Routines
{
    public partial class App : Application
    {
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
            MainPage = new AppShell();
        }
    }
}
