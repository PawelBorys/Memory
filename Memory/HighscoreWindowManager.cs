using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory
{
    static class HighscoreWindowManager
    {
        static StatsWindow _sw;

        public static void Show(bool isFour = true)
        {
            if (_sw == null || !_sw.IsVisible)        // don't open more than one highscore window, please
            {
                _sw = new StatsWindow();
                _sw.Show();                
            }
            else
            {
                _sw.Activate();
            }
            _sw.FoursTabItem.IsSelected = isFour;
            _sw.SixesTabItem.IsSelected = !isFour;
        }
    }
}
