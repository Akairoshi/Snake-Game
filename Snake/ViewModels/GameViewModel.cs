using Snake.Commands;
using Snake.Helpers;
using Snake.Infrastructure;
using Snake.Model;
using Snake.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
namespace Snake.ViewModels
{
    public class GameViewModel : ViewModelBase, IDisposable
    {
        private const int GridSize = 20;
        private double _cellSize;
        private int _gridX, _gridY;

        public double CellSize
        {
            get => _cellSize;
            set { _cellSize = value; OnPropertyChanged(); }
        }

        private readonly ILogService _logService;
        private readonly IDialogService _dialogService;

        private bool _isGameRunning = false;
        private Player _player = new(1.0);

        public ObservableCollection<SnakeSegment> Snake { get; } = new();
        public ObservableCollection<Food> Foods { get; } = new();

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
            _player.Died += OnPlayerDied;
            _player.ScoreChanged += OnScoreChanged;
            _logService.LogInfo("GameViewModel initialized");
            MoveCommand = new RelayCommand(ChangeDirection);
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
#if DEBUG
            Foods.Add(new(1, 1, CellSize));
            DebugHelper.SetScore(_player, 30);
            _player.AddScore(5);
#endif
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
        private void OnPlayerDied()
        {
            _isGameRunning = false;
        }
        private void OnScoreChanged()
        {
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Score));
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
        public void Dispose()
        {
            _player.Died -= OnPlayerDied;
            _player.ScoreChanged -= OnScoreChanged;
        }
    }
}
