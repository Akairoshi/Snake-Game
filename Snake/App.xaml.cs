using Snake.Services;
using Snake.Services.Interfaces;
using Snake.ViewModels;
using Snake.Views;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace Snake
{
    public partial class App : Application
    {
        private readonly ILogService _logService = new LogService();
        private readonly IRepositoryService _repositoryService = new RepositoryService();

        public App()
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnTaskException;
            _logService.LogInfo("Application started");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IDialogService dialogService = new DialogService(_logService);
            var vm = new LobbyViewModel(_repositoryService, _logService, dialogService);
            var window = new LobbyView(vm);
            window.Show();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _logService.LogError("Unhandled UI exception", e.Exception);
            _logService.ShowDialog("UI Exception");
            e.Handled = true;
            Environment.Exit(1);
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            _logService.LogError("Unhandled exception", ex ?? new Exception("Unknown"));
            _logService.ShowDialog("AppDomain Exception");
            Environment.Exit(1);
        }

        private void OnTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            _logService.LogError("Unobserved task exception", e.Exception);
            _logService.ShowDialog("Task Exception");
            e.SetObserved();
            Environment.Exit(1);
        }
    }
}