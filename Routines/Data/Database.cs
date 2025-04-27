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

        public Task<int> AddUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }
    }
}
