using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Kursa_Darbs_WBF_ES_DVDZ1
{
    public partial class timeMainWindow : Window
    {
        private Point lastMousePosition;
        private int score;
        private DispatcherTimer timer;
        private Random random = new Random();
        private int gameDuration = 3;
        private int elapsedTime = 0;

        public timeMainWindow()
        {
            InitializeComponent();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            score = 0;
            elapsedTime = 0;
            timer.Start();
            MoveObjectRandomly(movingRectangle);
            UpdateScore();



            //Šis palaiž skaņu kad piespiež pogu start un šo prikolu var izmantot visur

            MediaPlayer player = new MediaPlayer();
            player.Open(new Uri(@"C:\Users\nazis\Desktop\skana\mixkit-arcade-mechanical-bling-210.wav"));
            player.Play();


        }
        



        private void Timer_Tick(object sender, EventArgs e)
        {
            elapsedTime++;
            timerTextBlock.Text = "00:" + (gameDuration - elapsedTime).ToString("00");

            if (elapsedTime >= gameDuration)
            {
                timer.Stop();
                MessageBox.Show("Laiks beidzies");
               
                MessageBox.Show("Jūsu rezultāts ir " + score+" Vēlaties spelēt velreiz?") ;

                if (MessageBox.Show("Vēlaties spelēt velreiz?", "Spēle", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                   


                    score = 0;
                    elapsedTime = 0;
                    timer.Start();
                    MoveObjectRandomly(movingRectangle);
                    UpdateScore();

                }
                else
                {
                    this.Close();
                }

            }
        }
        //Poga Play Again
        private void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            score = 0;
            elapsedTime = 0;
            timer.Start();
            MoveObjectRandomly(movingRectangle);
            UpdateScore();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseOverObject(movingRectangle))
            {
                MoveObjectRandomly(movingRectangle);
                score += CalculatePoints();
                UpdateScore();
            }
        }

        private int CalculatePoints()
        {
            // Example scoring logic
            return Math.Max(10 - (elapsedTime / 3), 1);
        }

        private void UpdateScore()
        {
            Title = "Score: " + score;
        }

        private void MoveObjectRandomly(UIElement element)
        {
            double newX = random.NextDouble() * (canvas.ActualWidth - element.RenderSize.Width);
            double newY = random.NextDouble() * (canvas.ActualHeight - element.RenderSize.Height);

            Canvas.SetLeft(element, newX);
            Canvas.SetTop(element, newY);
        }

        private bool IsMouseOverObject(UIElement element)
        {
            Point mousePosition = Mouse.GetPosition(element);
            return new Rect(0, 0, element.RenderSize.Width, element.RenderSize.Height).Contains(mousePosition);
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lastMousePosition = e.GetPosition(canvas);
            Canvas.SetLeft(movingRectangle, lastMousePosition.X - movingRectangle.Width / 2);
            Canvas.SetTop(movingRectangle, lastMousePosition.Y - movingRectangle.Height / 2);
        }
        

    }
}