using Microsoft.Maui.Controls;
using Routines.Models;
using Routines.Utils;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Routines.Views
{
    public partial class ProgressPage : ContentPage
    {
        public ProgressPage()
        {
            InitializeComponent();
           // LoadStats();
        }

        private async void LoadStats()
        {
            var usuarioId = Session.UsuarioActual?.Id ?? 0;

            var habitos = await App.Database.GetHabitsByUserAsync(usuarioId);
            var checks = await App.Database.GetChecksByUserAsync(usuarioId);

            // Filtrar checks válidos
            var checksValidos = checks.Where(c => !string.IsNullOrEmpty(c.HabitTitulo)).ToList();

            // Estadísticas
            HabitosCreadosLabel.Text = $"📋 Hábitos creados: {habitos.Count}";
            CumplimientosRegistradosLabel.Text = $"✅ Cumplimientos registrados: {checks.Count}";

            // Último hábito registrado
            //var ultimoCheck = checks
            //                    .Where(c => !string.IsNullOrWhiteSpace(c.HabitTitulo))
            //                    .OrderByDescending(c => c.Fecha)
            //                    .FirstOrDefault();

            //UltimoHabitoLabel.Text = $"🕒 Último hábito registrado: {ultimoCheck?.HabitTitulo ?? "Ninguno"}";

            // Top 3 hábitos más cumplidos (por título)
            var top = checksValidos
                .GroupBy(c => c.HabitTitulo)
                .Select(g => new { Titulo = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(3)
                .ToList();

            Top1Label.Text = top.Count > 0 ? $"1. {top[0].Titulo}     {top[0].Count} veces" : "1. -";
            Top2Label.Text = top.Count > 1 ? $"2. {top[1].Titulo}     {top[1].Count} veces" : "2. -";
            Top3Label.Text = top.Count > 2 ? $"3. {top[2].Titulo}     {top[2].Count} veces" : "3. -";
        }

        private async void OnVerCumplimientosTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HabitChecksPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadStats();
        }
    }
}
