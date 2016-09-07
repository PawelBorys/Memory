using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for NamePrompt.xaml
    /// </summary>
    public partial class NamePrompt : Window, INotifyPropertyChanged
    {
        private string _playerName;
        public string playerName 
        {
            get
            {
                return _playerName;
            }
            set
            {
                _playerName = value;
                RaisePropertyChanged("playerName");
            }
        }

        public NamePrompt()
        {
            InitializeComponent();
            this.DataContext = this;         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //playerName = "Paweł";

            this.DialogResult = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            NameTB.Focus();
        }

        private void NameTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.DialogResult = true;
                Close();
            }
            else if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
                Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
