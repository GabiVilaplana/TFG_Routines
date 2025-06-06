﻿using SQLite;
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
            _database.CreateTableAsync<HabitCheck>().Wait();
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
                throw new ArgumentException("The username must be between 4 and 10 characters.");

            if (string.IsNullOrWhiteSpace(user.Contraseña) || user.Contraseña.Length < 4 || user.Contraseña.Length > 10)
                throw new ArgumentException("The password must be between 4 and 10 characters.");

            // Validar duplicado
            var existente = await _database.Table<User>()
                .Where(u => u.Usuario == user.Usuario)
                .FirstOrDefaultAsync();

            if (existente != null)
                throw new InvalidOperationException("That username is already in use.");

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

        // Añadir una validación a un hábito
        public Task<int> AddHabitCheckAsync(HabitCheck check)
        {
            return _database.InsertAsync(check);
        }

        // Obtener validaciones por Usuario
        public Task<List<HabitCheck>> GetChecksByUserAsync(int usuarioId)
        {
            return _database.Table<HabitCheck>()
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();
        }

        // Obtener validaciones por Hábito
        public Task<List<HabitCheck>> GetChecksByHabitAsync(int habitId)
        {
            return _database.Table<HabitCheck>()
                .Where(c => c.HabitId == habitId)
                .ToListAsync();
        }

        public async Task<bool> IsHabitDoneTodayAsync(int habitId, int userId)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var check = await _database.Table<HabitCheck>()
                .Where(hc => hc.HabitId == habitId &&
                             hc.UsuarioId == userId &&
                             hc.Fecha >= today && hc.Fecha < tomorrow)
                .FirstOrDefaultAsync();

            return check != null;
        }

        public Task<int> UpdateUserAsync(User user)
        {
            return _database.UpdateAsync(user);
        }


    }
}
