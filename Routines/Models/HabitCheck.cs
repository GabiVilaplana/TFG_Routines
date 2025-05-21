using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routines.Models
{
    public class HabitCheck
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int HabitId { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
        public string HabitTitulo { get; set; }
    }
}
