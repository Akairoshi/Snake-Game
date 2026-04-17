using System.Windows;

namespace Snake.Services.Interfaces
{
    public interface IDialogService
    {
        void Show<TWindow>() where TWindow : Window, new();
        void ShowDialog<TWindow>() where TWindow : Window, new();
        void Show<TWindow, TViewModel>(TViewModel viewModel)
            where TWindow : Window, new();
        bool? ShowDialog<TWindow, TViewModel>(TViewModel viewModel)
            where TWindow : Window, new();
        MessageBoxResult Confirm(string message, string title,
            MessageBoxButton buttons = MessageBoxButton.YesNo,
            MessageBoxImage icon = MessageBoxImage.Question);
    }
}
