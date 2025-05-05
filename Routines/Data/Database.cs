using SQLite;
using Routines.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Routines.Data
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);

            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<Habit>().Wait();
        }

        public Task<List<User>> GetUsersAsync()
        {
            return _database.Table<User>().ToListAsync();
        }

        public Task<User> GetUserAsync(string usuario, string contraseña)
        {
            return _database.Table<User>()
                .Where(u => u.Usuario == usuario && u.Contraseña == contraseña)
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddUserAsync(User user)
        {
            // Validar longitud
            if (string.IsNullOrWhiteSpace(user.Usuario) || user.Usuario.Length < 4 || user.Usuario.Length > 10)
                throw new ArgumentException("El nombre de usuario debe tener entre 4 y 10 caracteres.");

            if (string.IsNullOrWhiteSpace(user.Contraseña) || user.Contraseña.Length < 4 || user.Contraseña.Length > 10)
                throw new ArgumentException("La contraseña debe tener entre 4 y 10 caracteres.");

            // Validar duplicado
            var existente = await _database.Table<User>()
                .Where(u => u.Usuario == user.Usuario)
                .FirstOrDefaultAsync();

            if (existente != null)
                throw new InvalidOperationException("Ese nombre de usuario ya está en uso.");

            return await _database.InsertAsync(user);
        }

        public Task<List<Habit>> GetHabitsByUserAsync(int usuarioId)
        {
            return _database.Table<Habit>()
                            .Where(h => h.UsuarioId == usuarioId)
                            .ToListAsync();
        }

        // Añadir un hábito
        public Task<int> AddHabitAsync(Habit habit)
        {
            return _database.InsertAsync(habit);
        }

        // Modificar un hábito existente
        public Task<int> UpdateHabitAsync(Habit habit)
        {
            return _database.UpdateAsync(habit);
        }

        // Eliminar un hábito
        public Task<int> DeleteHabitAsync(Habit habit)
        {
            return _database.DeleteAsync(habit);
        }

        // Obtener Hábito por id
        public Task<Habit> GetHabitByIdAsync(int id)
        {
            return _database.Table<Habit>()
                .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
