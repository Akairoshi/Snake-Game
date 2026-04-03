using Snake.ViewModels;
using System.Windows;
using System.Windows.Input;

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
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                _vm.SetBoost(true);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                _vm.SetBoost(false);
        }
    }
}
