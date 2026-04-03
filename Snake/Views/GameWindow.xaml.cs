using Snake.ViewModels;
using System.Windows;

namespace Snake.Views
{
    public partial class GameWindow : Window
    {
        private readonly GameViewModel _vm;
        public GameWindow(GameViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }
        public void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            _vm.SetGameCanvasSize(GameCanvas.ActualWidth, GameCanvas.ActualHeight);
        }
    }
}
