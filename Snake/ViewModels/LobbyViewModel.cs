
using Snake.Commands;
using Snake.Data;
using Snake.Services.Interfaces;
using Snake.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Snake.ViewModels
{
    public class LobbyViewModel : ViewModelBase
    {
        private readonly ILogService _logService;
        private readonly IDialogService _dialogService;
        private readonly IRepositoryService _repositoryService;

        public ICommand StartGameCommand { get; }
        public ICommand SetGridSizeCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand ItemRemoveCommand { get; }

        private ObservableCollection<PlayerRecord> _records;
        public ObservableCollection<PlayerRecord> Records
        {
            get => _records;
            set { _records = value; OnPropertyChanged(); }
        }

        public LobbyViewModel(IRepositoryService repositoryService, ILogService logService, IDialogService dialogService)
        {
            _logService = logService;
            _dialogService = dialogService;
            _repositoryService = repositoryService;
            _records = new ObservableCollection<PlayerRecord>(_repositoryService.Load());

            CloseCommand = new RelayCommand(_ => CloseWindow());
            StartGameCommand = new RelayCommand(_ => StartGame());
            SetGridSizeCommand = new RelayCommand(SetGridSize);
            ItemRemoveCommand = new RelayCommand(ItemRemove);
            _logService.LogInfo("LobbyViewModel initialized");
        }

        private void ItemRemove(object? sender)
        {
            int id = (int)sender!;
            _repositoryService.Remove(id);
            ReloadRecords();
        }

        public void SetGridSize(object? parameter)
        {
            if (parameter is string str && int.TryParse(str, out int size))
            {
                GridSize = size;
            }
            else 
            {
                _logService.LogWarning($"Incorrect grid size: {parameter}");
            }
                
        }

        private string _nickname = "No Name";
        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                OnPropertyChanged();
                IsStartEnabled = !string.IsNullOrWhiteSpace(value);
            }
        }
        private int _gridSize = 20;
        public int GridSize
        {
            get => _gridSize;
            set
            {
                _gridSize = value;
                OnPropertyChanged();
            }
        }
        private int _speed = 80;
        public int Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SpeedText));
            }
        }
        public string SpeedText
        {
            get
            {
                return $"{220 + 80 - Speed}";
            }
        }

        private bool _isStartEnabled = true;
        public bool IsStartEnabled
        {
            get => _isStartEnabled;
            set
            {
                _isStartEnabled = value;
                OnPropertyChanged();
            }
        }

        private void StartGame()
        {
            _logService.LogInfo($"Starting game with nickname: {Nickname}, grid size: {GridSize}, speed: {Speed}");

            var gameVm = new GameViewModel(_repositoryService, _logService, GridSize, Speed, Nickname);
            _dialogService.ShowDialog<GameWindow, GameViewModel>(gameVm);
            ReloadRecords();
        }
        private void ReloadRecords()
        {
            Records = new ObservableCollection<PlayerRecord>(_repositoryService.Load());
        }

        public event Action? RequestClose;

        private async void CloseWindow()
        {
            RequestClose?.Invoke();
        }
    }
}
