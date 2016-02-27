using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Memory 
{
    public class Game : INotifyPropertyChanged
    {
        private const int _tileSize = 60;
        private const int _tileCount = 16;
        private const int _tileMargin = 5;

        private WrapPanel panel;
        private Random rand;

        private Tile[] tiles;
        private DispatcherTimer timer;
        private List<Tile> _selectedTiles;
        private int tilesLeft;

        private DateTime _startTime;

        private TimeSpan _time;
        public TimeSpan time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
                NotifyPropertyChanged("time");
            }
        }

        private int _clicks;
        public int clicks
        {
            get
            {
                return _clicks;
            }
            set
            {
                _clicks = value;
                NotifyPropertyChanged("clicks");
            }
        }

        public Game(WrapPanel panel)
        {
            this.panel = panel;
            tiles = new Tile[_tileCount];
            rand = new Random();
            timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (sender, args) =>
                {
                    time = DateTime.Now - _startTime;
                };
        }

        public void NewGame()
        {
            _selectedTiles = new List<Tile>();
            GenerateTiles();
            SetBoard();
            _startTime = DateTime.Now;
            clicks = 0;
            timer.Start();
        }

        public void GameOver()
        {
            timer.Stop();
        }

        public void TileClicked(Tile t)
        {
            clicks++;
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
                    if (tilesLeft == 0)
                    {
                        GameOver();
                    }
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

            tilesLeft -= 2;
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

            ShuffleTiles(tiles);
            tilesLeft = _tileCount;
        }

        private void ShuffleTiles(Tile[] t, int n = -1)
        {
            if (n == -1)
            {
                n = t.Length;
            }
            if (n > 0)
            {
                ShuffleTiles(t, n - 1);
                int index = rand.Next(n - 1);
                Tile tmp = t[index];
                t[index] = t[n - 1];
                t[n - 1] = tmp;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
