using Snake.Commands;
using Snake.Infrastructure;
using Snake.Model;
using Snake.Services;
using Snake.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
namespace Snake.ViewModels
{
    public class GameViewModel : ViewModelBase, IDisposable
    {
        private readonly List<SoundPlayer> _eatSounds = new()
        {
            new SoundPlayer(Application.GetResourceStream(new Uri("Assets/Sounds/1.wav", UriKind.Relative)).Stream),
            new SoundPlayer(Application.GetResourceStream(new Uri("Assets/Sounds/2.wav", UriKind.Relative)).Stream),
            new SoundPlayer(Application.GetResourceStream(new Uri("Assets/Sounds/3.wav", UriKind.Relative)).Stream),
        };
        private readonly SoundPlayer _death = new SoundPlayer(Application.GetResourceStream(new Uri("Assets/Sounds/death.wav", UriKind.Relative)).Stream);


        private int _gridSize;
        private double _cellSize;
        private int _tickRate;
        private string _nickname;
        private readonly DispatcherTimer _timer = new();


        private readonly ILogService _logService;
        private readonly IRepositoryService _repository;

        private readonly Random _random = new();

        private bool _isGameRunning;
        public bool IsGameRunning
        {
            get => _isGameRunning;
            set
            {
                if (_isGameRunning != value)
                {
                    _isGameRunning = value;

                    GameOverTextVisibility = !_isGameRunning;

                    OnPropertyChanged();
                }
            }
        }
        private bool _gameOverTextVisibility;
        public bool GameOverTextVisibility
        {
            get => _gameOverTextVisibility;
            set
            {
                if (_gameOverTextVisibility != value)
                {
                    _gameOverTextVisibility = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _backgroundFade;
        public bool BackgroundFade
        {
            get => _backgroundFade;
            set
            {
                if (_backgroundFade != value)
                {
                    _backgroundFade = value;
                    OnPropertyChanged();
                }
            }
        }


        private bool _isGamePaused = false;

        private Player _player;
        public ObservableCollection<SegmentViewModel> Apples { get; } = new();
        public ObservableCollection<SegmentViewModel> Snake { get; } = new();

        public ICommand MoveCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand CloseCommand { get; }

        public GameViewModel(IRepositoryService repositoryService, ILogService logService, int gridsize, int speed, string nickname)
        {
            _isGameRunning = true;
            _logService = logService;
            _repository = repositoryService;
            _logService.LogInfo("GameViewModel initialized");

            _tickRate = speed;
            _gridSize = gridsize;
            _nickname = nickname;


            SpawnPlayer();

            foreach (var s in _eatSounds)
                s.LoadAsync();
            MoveCommand = new RelayCommand(ChangeDirection);
            CloseCommand = new RelayCommand(_ => CloseWindow());
            PauseCommand = new RelayCommand(_ => Pause());

            _timer.Interval = TimeSpan.FromMilliseconds(_tickRate);
            _timer.Tick += (s, e) => _player.Move(Apples, _gridSize);
            _timer.Start();
        }
        public string Title
        {
            get
            {
                if (_isGameRunning == false || _player.Score < 30)
                    return "Snake";
                else if (_player.Score >= 30 && _player.Score < 50)
                    return "MEGA Snake";
                else if (_player.Score >= 50 && _player.Score < 80)
                    return "ULTRA SNAKE!!!!";
                else
                    return "--- SUPERMEGAULTRAPROFISNAKE ---";
            }
        }
        public void SpawnPlayer()
        {
            _player = new Player();
            _player.FoodEaten += OnFoodEated;
            _player.Died += OnPlayerDied;
            _player.ScoreChanged += OnScoreChanged;
            _player.Snake.CollectionChanged += Snake_CollectionChanged;
            _player.Spawn(_gridSize / 2, _gridSize / 2);
        }
        private void Snake_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            _logService.LogInfo($"Snake collection changed: Action={e.Action}, NewItems={e.NewItems?.Count}, OldItems={e.OldItems?.Count}");
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var segment = (Segment)e.NewItems[0];
                    Snake.Insert(e.NewStartingIndex, new SegmentViewModel(segment.GridX, segment.GridY, _cellSize));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (Snake.Count > 0)
                        Snake.RemoveAt(Snake.Count - 1);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Snake.Clear();
                    break;
            }
        }
        public void SetGameCanvasSize(double width, double height)
        {
            _cellSize = width / _gridSize;
            SpawnApple();
        }
        private void ChangeDirection(object? parameter)
        {
            if (!_isGamePaused)
            {
                _logService.LogInfo("ChangeDirection command executed with parameter: " + parameter);
                if (parameter is not string directionStr || !Enum.TryParse(directionStr, out Direction direction))
                {
                    _logService.LogWarning("Invalid direction parameter: " + parameter);
                    return;
                }
                _player.ChangeDirection(direction);
                OnPropertyChanged(nameof(MoveDirection));
            }
        }
        private void Pause()
        {
            if (_isGameRunning)
            {
                if (!_isGamePaused)
                {
                    _isGamePaused = true;
                    BackgroundFade = true;
                    _timer.Stop();
                    _logService.LogInfo("Game paused");
                }
                else
                {
                    _isGamePaused = false;
                    BackgroundFade = false;
                    _timer.Start();
                    _logService.LogInfo("Game resumed");
                }
            }
        }
        private void OnFoodEated(int gridX, int gridY)
        {
            var food = Apples.FirstOrDefault(f => f.GridX == gridX && f.GridY == gridY);
            if (food != null)
            {
                Apples.Remove(food);
                SpawnApple();
                _player.AddScore(1);
                _tickRate = Math.Max(50, _tickRate - 2);
                _logService.LogInfo($"Food eated [x: {gridX}; y: {gridY}");
                _eatSounds[_random.Next(_eatSounds.Count)].Play();
            }
        }
        private void OnPlayerDied()
        {
            _death.Play();
            _timer.Stop();
            IsGameRunning = false;
            BackgroundFade = true;

            if (Score > 0)
            {
                _repository.Save(new PlayerRecord(_player.Score, _nickname, _gridSize, _tickRate));
            }
            _logService.LogInfo("Player died with score: " + _player.Score);
            CloseWindowAfterDelay();
        }
        private void OnScoreChanged()
        {
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Score));
            ScoreChanged?.Invoke();
        }
        public void SpawnApple()
        {
            if(Apples.Count > 1)
                return;
            int count = _random.Next(1, 7);
            for (int i = 0; i < count; i++) 
            {
                if (Apples.Count >= (_gridSize * _gridSize) - _player.Snake.Count)
                    return;

                int foodX, foodY;
                do
                {
                    foodX = _random.Next(0, _gridSize);
                    foodY = _random.Next(0, _gridSize);
                } while (Apples.Any(f => f.GridX == foodX && f.GridY == foodY) ||
                            _player.Snake.Any(s => s.GridX == foodX && s.GridY == foodY));
                var food = new SegmentViewModel(foodX, foodY, _cellSize);
                Debug.WriteLine($"Spawning food at [x: {foodX}; y: {foodY}]");
                Apples.Add(food);
                _logService.LogInfo($"Food spawned at [x: {foodX}; y: {foodY}");
            }

        }
        public int Score
        {
            get => _player.Score;
        }

        public string MoveDirection
        {
            get
            {
                return _player.CurrentDirection switch
                {
                    Direction.Up => @"⇧",
                    Direction.Down => @"⇩",
                    Direction.Left => @"⇦",
                    Direction.Right => @"⇨",
                    _ => "Unknown"
                };
            }
        }



        public void Dispose()
        {
            _player.FoodEaten -= OnFoodEated;
            _player.ScoreChanged -= OnScoreChanged;
            _player.Died -= OnPlayerDied;
        }
        public event Action? RequestClose;
        public event Action? ScoreChanged;
        private async void CloseWindowAfterDelay()
        {
            _isGameRunning = false;
            _timer.Stop();
            await Task.Delay(3000);
            CloseWindow();
        }
        private void CloseWindow()
        {
            Dispose();
            RequestClose?.Invoke();
        }
    }
}