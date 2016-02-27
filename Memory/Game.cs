using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Memory
{
    public class Game
    {
        private const int _tileSize = 60;
        private const int _tileCount = 16;
        private const int _tileMargin = 5;

        private WrapPanel panel;

        Tile[] tiles;
        
        private List<Tile> _selectedTiles;

        public Game(WrapPanel panel)
        {
            this.panel = panel;
            tiles = new Tile[_tileCount];
        }

        public void NewGame()
        {
            _selectedTiles = new List<Tile>();
            GenerateTiles();
            SetBoard();
        }

        public void TileClicked(Tile t)
        {
            // if tile is disabled
            //if (t.enabled == false)
            //{
            //    return;
            //}

            // if tile eas already selected
            if (_selectedTiles.Contains(t))
            {
                _selectedTiles.Remove(t);
                t.ChangeUp();
                return;
            }

            if (_selectedTiles.Count == 2)
            {
                return;
            }

            // if no tile was selected ealier
            if (_selectedTiles.Count == 0)
            {
                _selectedTiles.Add(t);
                t.ChangeUp();
                return;
            }

            // if one tile was selected
            if (_selectedTiles.Count == 1)
            {
                _selectedTiles.Add(t);
                t.ChangeUp();

                if (_selectedTiles[0].pair == _selectedTiles[1].pair)
                {
                    PairFound();
                    _selectedTiles.Clear();
                    return;
                }

                DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };

                timer.Start();
                timer.Tick += (sender, args) =>
                    {
                        timer.Stop();
                        _selectedTiles[0].ChangeUp();
                        _selectedTiles[1].ChangeUp();
                        _selectedTiles.Clear();
                    };                
            }
        }

        private void PairFound()
        {
            _selectedTiles[0].Hide();
            _selectedTiles[1].Hide();
        }

        private void SetBoard()
        {
            panel.Children.Clear();

            for (int i = 0; i < _tileCount; i++)
            {
                panel.Children.Add(tiles[i].rect);                               
            }
            panel.Width = Math.Floor(Math.Sqrt(_tileCount)) * (_tileSize + _tileMargin + _tileMargin);
        }

        private void GenerateTiles()
        {
            // generate tiles
            for (int i = 0; i < _tileCount; i++)
            {
                tiles[i] = new Tile(_tileSize, _tileMargin, this);
            }     

            // set images
            double index = 0;
            for (int i = 0; i < _tileCount; i++)
            {
                byte imageName = (byte)Math.Floor(index / 2);
                tiles[i].SetImage(new BitmapImage(new Uri("pack://application:,,,/Assets/" + imageName.ToString() + ".png")), imageName);
                index++;                
            }

            // TODO: shuffle tiles
            
        }
    }
}
