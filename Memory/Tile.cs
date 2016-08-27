using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        private bool _isEnabled;
        private Game _theGame;

        private Brush _cardBack = new SolidColorBrush(Color.FromRgb(115, 183, 117));

        public Tile(int size, int margin, Game theGame)
        {
            rect = new Rectangle() { Height = size, Width = size, Margin = new Thickness(margin), Stroke = Brushes.Black, Fill = _cardBack };
            isUp = false;
            this._theGame = theGame;
            this.rect.MouseLeftButtonUp += rect_MouseLeftButtonUp;
            _isEnabled = true;
        }

        public Tile(Tile t)
        {
            this.rect = new Rectangle() { Height = t.rect.Height, Width = t.rect.Width, Margin = new Thickness(t.rect.Margin.Bottom), Stroke = Brushes.Black, Fill = _cardBack };
            this.isUp = false;
            this._theGame = t._theGame;
            this.rect.MouseLeftButtonUp += rect_MouseLeftButtonUp;
            this._isEnabled = true;
            this.image = t.image;
            this.pair = t.pair;
        }

        void rect_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_isEnabled)
            {
                _theGame.TileClicked(this);
            }            
        }

        public void Hide()
        {
            _isEnabled = false;

            var anim = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300)
            };

            this.rect.BeginAnimation(System.Windows.Shapes.Rectangle.OpacityProperty, anim);
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
                rect.Fill = _cardBack;
            }
            else
            {
                rect.Fill = new ImageBrush(image) { Stretch = Stretch.Uniform };                
            }

            isUp = !isUp;
        }
    }
}
