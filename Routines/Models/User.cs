using SQLite;

namespace Routines.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Usuario { get; set; }

        public string Contraseña { get; set; }
        public string Background { get; set; } = "Azul";
        public string Language { get; set; } = "en";

    }
}
