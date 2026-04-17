using Snake.ViewModels;
using System.Diagnostics;
using System.Windows;

namespace Snake.Views
{
    public partial class LobbyView : Window
    {
        private readonly LobbyViewModel _vm;
        public LobbyView(LobbyViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
            _vm.RequestClose += () => Close();
            Window_Title.Text = this.Title;
        }
    }
}
