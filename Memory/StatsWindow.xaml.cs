using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Memory
{
    /// <summary>
    /// Interaction logic for StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : Window
    {
        List<Stat> stats4;
        List<Stat> stats6;

        public StatsWindow()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            LoadHighscores();
        }

        void LoadHighscores()
        {
            using (StatsContext context = new StatsContext())
            {
                stats4 = (from h in context.highscores
                          where h.isFour == true
                          select h).Take(10).OrderBy(x => x.clicks).ToList<Stat>();

                stats6 = (from h in context.highscores
                          where h.isFour == false
                          select h).Take(10).OrderBy(x => x.clicks).ToList<Stat>();
            }
            FoursGrid.ItemsSource = stats4;
            SixesGrid.ItemsSource = stats6;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Do you want do erase all highscores?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialogResult == MessageBoxResult.Yes)
            {
                using (StatsContext context = new StatsContext())
                {
                    context.Database.ExecuteSqlCommand("delete from Stats");
                    this.Close();      
                }
            }
        }
    }
}
