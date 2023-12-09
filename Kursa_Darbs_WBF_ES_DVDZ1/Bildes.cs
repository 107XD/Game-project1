using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Kursa_Darbs_WBF_ES_DVDZ1
{
    internal class BurgerIngredient
    {
        public Image Image { get; private set; }
        public Point Position { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int IngredientNumber { get; private set; }
        public bool IsActive { get; set; }

        public Rect Rect => new Rect(Position, new Size(Width, Height));

        public string IngredientType { get; private set; }

        public BurgerIngredient(string imageLocation, double initialX, double initialY, int ingredientNumber)
        {
            {
                Image = new Image
                {
                    Source = new BitmapImage(new Uri(imageLocation, UriKind.RelativeOrAbsolute)),
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(0, 0, 0, 0)
                };
                Width = 100;
                Height = 100;
                IngredientNumber = ingredientNumber;
                Position = new Point(initialX, initialY);
            }
        }
    }
}