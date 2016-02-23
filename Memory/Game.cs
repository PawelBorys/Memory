using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Memory
{
    public class Game
    {
        private const int _tileSize = 60;
        private const int _tileRows = 4;
        private const int _tileCols = 4;
        private const int _tileMargin = 5;

        private WrapPanel panel;

        Tile[,] tiles;
        private Tile _selectedTile;

        public Game(WrapPanel panel)
        {
            this.panel = panel;
            tiles = new Tile[_tileRows, _tileCols];
        }

        public void NewGame()
        {
            _selectedTile = null;
            GenerateTiles();
            SetBoard();
        }

        public void TileClicked(Tile t)
        {
            if (_selectedTile == null)
            {
                _selectedTile = t;
            }
            else if (_selectedTile.pair == t.pair)
            {
                if (_selectedTile != t)
                {
                    PairFound(_selectedTile, t);
                }
                else
                {
                    _selectedTile = null;
                }
               
                
            }
        }

        private void PairFound(Tile t1, Tile t2)
        {
            panel.Width = 0;
        }

        private void SetBoard()
        {
            panel.Children.Clear();

            for (int i = 0; i < _tileRows; i++)
            {
                for (int j = 0; j < _tileCols; j++)
                {                    
                    panel.Children.Add(tiles[i,j].rect);
                }                
            }
            panel.Width = _tileCols * (_tileSize + _tileMargin + _tileMargin);
        }

        private void GenerateTiles()
        {
            // generate tiles
            for (int i = 0; i < _tileRows; i++)
            {
                for (int j = 0; j < _tileCols; j++)
                {
                    tiles[i, j] = new Tile(_tileSize, _tileMargin, this);
                }
            }

            // set images
            double index = 0;
            for (int i = 0; i < _tileRows; i++)
            {
                for (int j = 0; j < _tileCols; j++)
                {
                    byte imageName = (byte)Math.Floor(index / 2);

                    tiles[i,j].SetImage(new BitmapImage(new Uri("pack://application:,,,/Assets/" + imageName.ToString() + ".png")), imageName);

                    index++;
                }
            }
        }
    }
}
