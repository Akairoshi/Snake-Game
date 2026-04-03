using Snake.Commands;
using Snake.Infrastructure;
using Snake.Model;
using Snake.Services.Interfaces;
using System.Collections.ObjectModel;
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
            new SoundPlayer(Application.GetResourceStream(new Uri("Sounds/1.wav", UriKind.Relative)).Stream),
            new SoundPlayer(Application.GetResourceStream(new Uri("Sounds/2.wav", UriKind.Relative)).Stream),
            new SoundPlayer(Application.GetResourceStream(new Uri("Sounds/3.wav", UriKind.Relative)).Stream),
        };
        private readonly SoundPlayer _death = new SoundPlayer(Application.GetResourceStream(new Uri("Sounds/death.wav", UriKind.Relative)).Stream);


        private const int GridSize = 20;
        private double _cellSize;
        private int _gridX, _gridY;
        private int tickRate = 70;
        private readonly DispatcherTimer _timer = new();

        public double CellSize
        {
            get => _cellSize;
            set { _cellSize = value; OnPropertyChanged(); }
        }

        private readonly ILogService _logService;
        private readonly IDialogService _dialogService;
        private readonly Random _random = new();

        private bool _isGameRunning = false;
        private Player _player = new Player();
        public ObservableCollection<Food> Foods { get; } = new();
        public ObservableCollection<SnakeSegment> Snake => _player.Snake;

        public ICommand MoveCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand ResumeCommand { get; }
        public ICommand RestartCommand { get; }
        public ICommand IncreaseSpeed { get; }
        public ICommand DecreaseSpeed { get; }

        public GameViewModel(IDialogService dialogService, ILogService logService)
        {
            _isGameRunning = true;
            _logService = logService;
            _dialogService = dialogService;
            _player.FoodEaten += OnFoodEated;
            _player.ScoreChanged += OnScoreChanged;
            _player.Died += OnPlayerDied;
            _logService.LogInfo("GameViewModel initialized");

            foreach (var s in _eatSounds)
                s.LoadAsync();

            MoveCommand = new RelayCommand(ChangeDirection);
            RestartCommand = new RelayCommand(_ => Restart());

            _timer.Interval = TimeSpan.FromMilliseconds(tickRate);
            _timer.Tick += (s, e) => _player.Move(Foods, GridSize, CellSize);
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
        public void SetGameCanvasSize(double width, double height)
        {
            CellSize = width / 20;
            _player.Spawn(GridSize / 2, GridSize / 2, CellSize);
            SpawnFood();
        }
        private void ChangeDirection(object? parameter)
        {
            _logService.LogInfo("ChangeDirection command executed with parameter: " + parameter);
            Debug.WriteLine("ChangeDirection command executed with parameter: " + parameter);
            if (parameter is not string directionStr || !Enum.TryParse(directionStr, out Direction direction))
            {
                _logService.LogWarning("Invalid direction parameter: " + parameter);
                return;
            }
            _player.ChangeDirection(direction);
            OnPropertyChanged(nameof(MoveDirection));
        }
        private void OnFoodEated(int gridX, int gridY)
        {
            var food = Foods.FirstOrDefault(f => f.GridX == gridX && f.GridY == gridY);
            if (food != null)
            {
                Foods.Remove(food);
                SpawnFood();
                _player.AddScore(1);
                tickRate = Math.Max(50, tickRate - 2);
                Debug.WriteLine("Playing sound");
                _eatSounds[_random.Next(_eatSounds.Count)].Play();
            }
        }
        private void Restart()
        {
            _death.Stop();
            Foods.Clear();
            _player.Reset();
            _player.Spawn(GridSize / 2, GridSize / 2, CellSize);
            SpawnFood();
            _isGameRunning = true;
            _timer.Start();
        }
        private void OnPlayerDied()
        {
            _death.Play();
            _timer.Stop();
            _isGameRunning = false;
        }
        private void OnScoreChanged()
        {
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Score));
        }
        public void SpawnFood()
        {
            if(Foods.Count > 1)
                return;
            int count = _random.Next(1, 7);
            for (int i = 0; i < count; i++) 
            {
                if (Foods.Count >= (GridSize * GridSize) - _player.Snake.Count)
                    return;

                int foodX, foodY;
                do
                {
                    foodX = _random.Next(0, GridSize);
                    foodY = _random.Next(0, GridSize);
                } while (Foods.Any(f => f.GridX == foodX && f.GridY == foodY) ||
                            _player.Snake.Any(s => s.GridX == foodX && s.GridY == foodY));
                var food = new Food(foodX, foodY, CellSize);
                Foods.Add(food);
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
                    Direction.Up => @"⬆️",
                    Direction.Down => @"⬇️",
                    Direction.Left => @"⬅️",
                    Direction.Right => @"➡",
                    _ => "Unknown"
                };
            }
        }
        public void SetBoost(bool active)
        {
            _timer.Interval = active
                ? TimeSpan.FromMilliseconds(Math.Max(50, tickRate - 80))
                : TimeSpan.FromMilliseconds(Math.Max(50, tickRate + 80));
        }
        public void Dispose()
        {
            _player.FoodEaten -= OnFoodEated;
            _player.ScoreChanged -= OnScoreChanged;
            _player.Died -= OnPlayerDied;
        }
    }
}
