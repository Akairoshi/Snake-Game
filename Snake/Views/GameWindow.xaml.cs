using Snake.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Snake.Views
{
    public partial class GameWindow : Window
    {
        private GameViewModel _vm;
        public GameWindow()
        {
            InitializeComponent();
        }
        public void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            _vm = DataContext as GameViewModel;
            _vm?.RequestClose += () => Close();
            _vm?.ScoreChanged += OnScoreChanged;
            _vm?.SetGameCanvasSize(GameCanvas.ActualWidth, GameCanvas.ActualHeight);
            Window_Title.Text = this.Title;
        }
        private void OnScoreChanged()
        {
            var transform = new TranslateTransform();
            ScoreText.RenderTransform = transform;
            var anim = new DoubleAnimation(0, -15, TimeSpan.FromMilliseconds(300))
            {
                AutoReverse = true,
                EasingFunction = new CubicEase()
            };
            transform.BeginAnimation(TranslateTransform.YProperty, anim);
        }

    }
}
