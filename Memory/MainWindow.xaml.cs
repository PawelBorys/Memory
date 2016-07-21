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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (StatsContext db = new StatsContext())
            {

            }
        }

        private void Button_Click_2(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StatsWindow sw = new StatsWindow();
            sw.Show();
        }
    }
}
