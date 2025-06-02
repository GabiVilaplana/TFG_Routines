using Microsoft.Maui.Controls;
using Routines.Utils.Background;

namespace Routines.Views
{
    public class BasePage : ContentPage
    {
        protected Image BackgroundImage;

        public BasePage()
        {
            BackgroundImage = new Image
            {
                Aspect = Aspect.AspectFill,
                Opacity = 0.7,
                IsVisible = true,
                Source = BackgroundManager.Instance.CurrentBackgroundImage
            };

            var grid = new Grid();
            grid.Children.Add(BackgroundImage);
            Content = grid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BackgroundImage.Source = BackgroundManager.Instance.CurrentBackgroundImage;
        }

        public void SetContent(View view)
        {
            if (Content is Grid grid)
            {
                if (grid.Children.Count > 1)
                    grid.Children.RemoveAt(1); // elimina el contenido anterior

                grid.Children.Add(view); // inserta el nuevo contenido encima del fondo
            }
        }
    }
}
