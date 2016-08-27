using System;
using System.Windows;

namespace Memory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Game theGame;
        
                    
        public MainWindow()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());

            // initialize database connection now, because it takes some time
            /*StatsContext sc = new StatsContext();
            sc.highscores.ToString();
            sc.Dispose();*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            theGame.NewGame(GetSize());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            theGame = new Game(BoardWrapPanel, GetSize());
            this.DataContext = theGame;
            theGame.NewGame(GetSize());
        }

        /// <summary>
        /// Get number of cards on the board
        /// </summary>
        private int GetSize()
        {
            if ((bool)Size4RB.IsChecked)
            {
                return 16;
            }
            else
            {
                return 36;
            }
        }

        private void Button_Click_2(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            HighscoreWindowManager.Show(Size4RB.IsChecked == true);
        }
    }
}
