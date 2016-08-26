using System;
using System.Linq;
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

        private WrapPanel _panel;
        private Random _rand;

        private Tile[] _tiles;
        private Tile[] _shuffledTiles;
        private DispatcherTimer _timer;
        DispatcherTimer _tileShowTimer;
        private List<Tile> _selectedTiles;
        private int _tilesLeft;

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
                    _timer.Start();
                }
                else
                {
                    _timer.Stop();
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
            this._panel = panel;
            this._tileCount = size;
            _tiles = new Tile[_maxTileCount];
            _rand = new Random();
            _timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (sender, args) =>
                {
                    time = DateTime.Now - _startTime;
                };

            _tileShowTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            _tileShowTimer.Tick += (sender, args) =>
            {
                _tileShowTimer.Stop();
                _selectedTiles[0].ChangeUp();
                _selectedTiles[1].ChangeUp();
                _selectedTiles.Clear();
            }; 
            GenerateTiles();
        }

        public void NewGame(int size)
        {
            _selectedTiles = new List<Tile>();
            tileCount = size;
            _shuffledTiles = ShuffleTiles(_tiles);
            SetBoard();            
            clicks = 0;
            _tilesLeft = tileCount;
            time = new TimeSpan();
            isStarted = false;
            _tileShowTimer.Stop();
        }

        public void GameOver()
        {
            isStarted = false;
            SaveHighscore();
        }

        private void SaveHighscore()
        {
            using (StatsContext context = new StatsContext())
            {
                bool isSizeFour = Math.Sqrt(tileCount) == 4;

                var scores = (from h in context.highscores
                              where h.isFour == isSizeFour
                              select h.clicks).ToArray();

                int maxScore = scores.Max();

                if (clicks < maxScore || scores.Count() < 10)       // if the score is better than the worst score in the highscores or there is less than 10 scores saved
                {
                    NamePrompt np = new NamePrompt();
                    np.ShowDialog();

                    if (np.DialogResult == true)        // if player provided their name
                    {                        
                        string playerName = np.playerName;

                        context.highscores.Add(new Stat() { name = playerName, clicks = this.clicks, time = this.time, isFour = isSizeFour });
                        context.SaveChanges();

                        HighscoreWindowManager.Show(isSizeFour);
                    }                    
                }
            }
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
                    if (_tilesLeft == 0)
                    {
                        GameOver();
                    }
                    return;
                }

                _tileShowTimer.Start();
                               
            }
        }

        private void PairFound()
        {
            _selectedTiles[0].Hide();
            _selectedTiles[1].Hide();

            _tilesLeft -= 2;
        }

        private void SetBoard()
        {
            _panel.Children.Clear();

            for (int i = 0; i < _tileCount; i++)
            {
                _panel.Children.Add(_shuffledTiles[i].rect);                               
            }
            _panel.Width = Math.Floor(Math.Sqrt(_tileCount)) * (_tileSize + _tileMargin + _tileMargin);
        }

        private void GenerateTiles()
        {
            // generate tiles
            for (int i = 0; i < _tileCount; i++)
            {
                _tiles[i] = new Tile(_tileSize, _tileMargin, this);
            }     

            // set images
            double index = 0;
            for (int i = 0; i < _tileCount; i++)
            {
                byte imageName = (byte)Math.Floor(index / 2);
                _tiles[i].SetImage(new BitmapImage(new Uri("pack://application:,,,/Assets/" + imageName.ToString() + ".png")), imageName);
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
                int index = _rand.Next(source.Count);
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
