using Routines.Utils;
using System.Globalization;

namespace Routines.Views
{
    public partial class CalendarPage : ContentPage
    {
        private DateTime _fechaActual = DateTime.Today;

        public CalendarPage()
        {
            InitializeComponent();
            _ = MostrarMes(_fechaActual); // sin await si es desde OnAppearing o constructor
        }

        private async Task MostrarMes(DateTime fecha)
        {
            var tareasUsuario = await App.Database.GetHabitsByUserAsync(Session.UsuarioActual.Id);
            var tareasPorDia = tareasUsuario
                .Where(h => h.FechaAsignada.HasValue)
                .GroupBy(h => h.FechaAsignada.Value.Date)
                .ToDictionary(g => g.Key, g => g.ToList());

            MesActualLabel.Text = fecha.ToString("MMMM yyyy", CultureInfo.GetCultureInfo("es-ES"));
            CalendarioGrid.Children.Clear();
            CalendarioGrid.RowDefinitions.Clear();
            CalendarioGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < 7; i++)
                CalendarioGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            var primerDiaDelMes = new DateTime(fecha.Year, fecha.Month, 1);
            int diasEnMes = DateTime.DaysInMonth(fecha.Year, fecha.Month);
            int offset = ((int)primerDiaDelMes.DayOfWeek + 6) % 7; // Ajuste para que Lunes = 0

            int totalCeldas = offset + diasEnMes;
            int filas = (int)Math.Ceiling(totalCeldas / 7.0);

            for (int i = 0; i < filas; i++)
                CalendarioGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            for (int dia = 1; dia <= diasEnMes; dia++)
            {
                int celda = offset + dia - 1;
                int fila = celda / 7;
                int columna = celda % 7;

                var fechaDia = new DateTime(fecha.Year, fecha.Month, dia);

                var boton = new Button
                {
                    Text = dia.ToString(),
                    TextColor = Colors.Black, // Mejor contraste
                    BackgroundColor = tareasPorDia.ContainsKey(fechaDia)
                        ? Color.FromArgb("#FFD54F") // amarillo intenso
                        : Color.FromArgb("#BBDEFB"), // azul claro
                    CornerRadius = 6,
                    FontAttributes = FontAttributes.Bold,
                    Padding = 5
                };

                boton.Clicked += async (s, e) =>
                {
                    if (tareasPorDia.TryGetValue(fechaDia, out var tareas))
                    {
                        string resumen = string.Join("\n", tareas.Select(t => $"• {t.Titulo}"));
                        await DisplayAlert($"Tareas del {fechaDia:dd/MM}", resumen, "OK");
                    }
                    else
                    {
                        await DisplayAlert("Sin tareas", "No hay tareas para este día.", "OK");
                    }
                };

                Grid.SetRow(boton, fila);
                Grid.SetColumn(boton, columna);
                CalendarioGrid.Children.Add(boton);
            }
        }

        private void OnPreviousMonthClicked(object sender, EventArgs e)
        {
            _fechaActual = _fechaActual.AddMonths(-1);
            MostrarMes(_fechaActual);
        }

        private void OnNextMonthClicked(object sender, EventArgs e)
        {
            _fechaActual = _fechaActual.AddMonths(1);
            MostrarMes(_fechaActual);
        }
    }
}
