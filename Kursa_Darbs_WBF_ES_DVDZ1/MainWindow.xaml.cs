
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Kursa_Darbs_WBF_ES_DVDZ1
{
    public partial class MainWindow : Window
    {
        private readonly List<int> correctRecipeOrder = new List<int> { 1, 2, 3, 4, 5 };
        private List<BurgerIngredient> ingredients = new List<BurgerIngredient>();
        private BurgerIngredient selectedIngredient;
        private Point offset;

        // Dictionary to map ingredient types to numbers
        private Dictionary<string, int> ingredientNumbers = new Dictionary<string, int>
        {
            { "Auksa Maizes", 1 },
            { "Gala", 2 },
            // Add more mappings for each ingredient type
        };

        public MainWindow()
        {
            InitializeComponent();
            SetUpApp();
        }

        private void SetUpApp()
        {
            //C:\Users\nazis\Desktop\kursa\atteli
            string imageDirectoryPath = @"C:\Users\nazis\Desktop\atteli";
            string[] imagePaths = Directory.GetFiles(imageDirectoryPath, "*.png");
            foreach (string imagePath in imagePaths)
            {
                string imageName = Path.GetFileNameWithoutExtension(imagePath);
                int ingredientNumber = GetIngredientNumber(imageName);
                if (ingredientNumber != -1)
                {
                    BurgerIngredient ingredient = new BurgerIngredient(imagePath, 0, 0, ingredientNumber);
                    ingredients.Add(ingredient);
                    burgerCanvas.Children.Add(ingredient.Image);
                }
            }

            StackIngredients();

            burgerCanvas.MouseDown += BurgerCanvas_MouseDown;
            burgerCanvas.MouseMove += BurgerCanvas_MouseMove;
            burgerCanvas.MouseUp += BurgerCanvas_MouseUp;

            // Add event handlers for the buttons


        }

        private void BurgerCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(burgerCanvas);

            foreach (BurgerIngredient ingredient in ingredients)
            {
                if (ingredient.Rect.Contains(mousePosition))
                {
                    selectedIngredient = ingredient;
                    offset = new Point(selectedIngredient.Position.X - mousePosition.X, selectedIngredient.Position.Y - mousePosition.Y);
                    ingredient.Image.Opacity = 0.7;
                    Panel.SetZIndex(ingredient.Image, 1); // Bring to foreground
                    break;
                }
            }
        }

        private void BurgerCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedIngredient != null)
            {
                Point mousePosition = e.GetPosition(burgerCanvas);
                Point newPosition = new Point(mousePosition.X + offset.X, mousePosition.Y + offset.Y);

                selectedIngredient.Position = newPosition;
                selectedIngredient.Image.Margin = new Thickness(newPosition.X, newPosition.Y, 0, 0);
            }
        }

        private void BurgerCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (selectedIngredient != null)
            {
                selectedIngredient.IsActive = true;
                StackIngredients();

                selectedIngredient.Image.Opacity = 1.0;
                Panel.SetZIndex(selectedIngredient.Image, 0); // Reset Z-index
                selectedIngredient = null;
            }
        }

        private void CheckRecipeOrder()
        {
            // Example check - customize as per your recipe logic
            for (int i = 0; i < Math.Min(ingredients.Count, correctRecipeOrder.Count); i++)
            {
                if (ingredients[i].IngredientNumber != correctRecipeOrder[i])
                {
                    MessageBox.Show("Incorrect recipe order!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            MessageBox.Show("Correct recipe order!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

       private void McDonaldsMenu_Click(object sender, RoutedEventArgs e)
{
    IngredientsWindow ingredientsWindow = new IngredientsWindow();
    ingredientsWindow.OnWindowClosed += OpenTimeWindow;// Set up an event handler to re-show MainWindow when IngredientsWindow is closed
            this.Hide(); // Hide the main window
            ingredientsWindow.Show(); // Show the ingredients window
        }

        private void CompleteBurger_Click(object sender, RoutedEventArgs e)
        {
            // Logic for Complete Burger button click
            // This can call the CheckRecipeOrder method or other game completion logic
            CheckRecipeOrder();
        }
        private void StackIngredients()
        {
            double currentYPosition = 300;
            foreach (var ingredient in ingredients)
            {
                Canvas.SetTop(ingredient.Image, currentYPosition - ingredient.Image.Height);
                currentYPosition -= ingredient.Image.Height;
            }
        }

        private void IngredientButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                string ingredientType = button.Content.ToString();
                string imagePath = GetImagePathForIngredient(ingredientType);
                int ingredientNumber = GetIngredientNumber(ingredientType);

                if (ingredientNumber != -1)
                {
                    BurgerIngredient newIngredient = new BurgerIngredient(imagePath, 0, 0, ingredientNumber);
                    ingredients.Add(newIngredient);
                    burgerCanvas.Children.Add(newIngredient.Image);
                    StackIngredients();
                }
            }
        }

        private string GetImagePathForIngredient(string ingredientType)
        {
            //C:\Users\nazis\Desktop\kursa\atteli
            string imageDirectoryPath = @"C:\Users\nazis\Desktop\atteli";
            string imagePath = Path.Combine(imageDirectoryPath, ingredientType + ".png");
            if (File.Exists(imagePath))
            {
                return imagePath;
            }
            return null;


        }

        private int GetIngredientNumber(string ingredientType)
        {
            if (ingredientNumbers.TryGetValue(ingredientType, out int number))
            {
                return number;
            }
            return -1;
        }

        private void GoToTime_Click(object sender, RoutedEventArgs e)
        {
            //open time window
            timeMainWindow timeWindow = new timeMainWindow();
            timeWindow.Show();
            this.Close();

        }


        private void OpenTimeWindow()
        {
            timeMainWindow timeWindow = new timeMainWindow();
            timeWindow.Show();
            this.Close();
        }

        //Sound
        private void PlaySound(string soundName)
        {
            string soundPath = Path.Combine(@"C:\Users\nazis\Desktop\skana", soundName + ".wav");
            if (File.Exists(soundPath))
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundPath);
                player.Play();
            }


        }
    }
}




