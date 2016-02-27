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
            theGame.NewGame();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            theGame = new Game(BoardWrapPanel);
            this.DataContext = theGame;
            theGame.NewGame();
        }       
    }
}
