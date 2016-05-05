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
        private const int _tileMargin = 5;
        private const int _maxTileCount = 36;

        private WrapPanel panel;
        private Random rand;

        private Tile[] tiles;
        private Tile[] shuffledTiles;
        private DispatcherTimer timer;
        private List<Tile> _selectedTiles;
        private int tilesLeft;

        private bool _isStarted;
        public bool isStarted
        {
            get { return _isStarted; }
            set 
            {
                _isStarted = value;
                if (value == true)
                {
                    _startTime = DateTime.Now;
                    timer.Start();
                }
                else
                {
                    timer.Stop();
                }
            }
        }

        private DateTime _startTime;

        private int _tileCount; // = 36
        public int tileCount
        {
            get { return _tileCount; }
            set
            {
                _tileCount = value;
                NotifyPropertyChanged("tileCount");
            }
        }

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

        public Game(WrapPanel panel, int size)
        {
            this.panel = panel;
            this._tileCount = size;
            tiles = new Tile[_maxTileCount];
            rand = new Random();
            timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (sender, args) =>
                {
                    time = DateTime.Now - _startTime;
                };
            GenerateTiles();
        }

        public void NewGame(int size)
        {
            _selectedTiles = new List<Tile>();
            tileCount = size;
            shuffledTiles = ShuffleTiles(tiles);
            SetBoard();            
            clicks = 0;
            tilesLeft = tileCount;
            time = new TimeSpan();
            isStarted = false;
            
        }

        public void GameOver()
        {
            isStarted = false;
        }

        public void TileClicked(Tile t)
        {
            if (isStarted == false)
            {
                isStarted = true;
            }            

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
                panel.Children.Add(shuffledTiles[i].rect);                               
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
        }

        private Tile[] ShuffleTiles(Tile[] t)
        {
            List<Tile> source = new List<Tile>();
            List<Tile> result = new List<Tile>();

            for (int i = 0; i < tileCount; i++)
            {
                source.Add(new Tile(t[i]));
            }

            while (source.Count > 0)
            {
                int index = rand.Next(source.Count - 1);
                result.Add(source[index]);
                source.RemoveAt(index);
            }

            return result.ToArray();
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
