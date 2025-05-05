using SQLite;

namespace Routines.Models
{
    public class Habit
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Titulo { get; set; }
        public string Categoria { get; set; } // Salud, Estudio, etc.
        public string Frecuencia { get; set; } // Diaria, Semanal...

        public int UsuarioId { get; set; } // 🔗 Clave foránea al usuario que lo creó
    }
}