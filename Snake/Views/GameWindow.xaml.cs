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
    }
}
