using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Memory
{
    public class Tile
    {
        public Rectangle rect;
        public BitmapImage image;
        public bool isUp;
        public byte pair;
        private Game theGame;

        public Tile(int size, int margin, Game theGame)
        {
            rect = new Rectangle() { Height = size, Width = size, Margin = new Thickness(margin), Stroke = Brushes.Black, Fill = Brushes.Gray };
            isUp = false;
            this.theGame = theGame;
            this.rect.MouseLeftButtonUp += rect_MouseLeftButtonUp;
        }

        void rect_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ChangeUp();
            theGame.TileClicked(this);
        }


        public void SetImage(BitmapImage image, byte p)
        {
            this.image = image;
            this.pair = p;
        }

        public void ChangeUp()
        {
            if (isUp)
            {
                rect.Fill = Brushes.Gray;
            }
            else
            {                
                rect.Fill = new ImageBrush(image);
            }

            isUp = !isUp;
        }
    }
}
