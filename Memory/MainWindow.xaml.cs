﻿using System;
using System.Windows;

namespace Memory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Game theGame;
        StatsWindow sw;
                    
        public MainWindow()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
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

        private void Button_Click_2(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sw == null || !sw.IsVisible)
            {
                sw = new StatsWindow();
                sw.Show();
            }
            
            
        }
    }
}
