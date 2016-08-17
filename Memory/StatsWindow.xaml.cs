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
        List<Stat> stats;

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
                stats = (from h in context.highscores
                         select h).OrderBy(x => x.clicks).ToList<Stat>();
            }
            FivesGrid.ItemsSource = stats;
        }
    }
}
