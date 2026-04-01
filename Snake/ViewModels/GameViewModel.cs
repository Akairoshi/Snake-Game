using Snake.Services.Interfaces;

namespace Snake.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        private readonly ILogService _logService;
        private readonly IDialogService _dialogService;
        public GameViewModel(IDialogService dialogService, ILogService logService)
        {
            _logService = logService;
            _dialogService = dialogService;

            _logService.LogInfo("GameViewModel initialized");
        }
    }
}
