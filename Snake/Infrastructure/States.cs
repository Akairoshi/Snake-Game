using System.Windows.Media;

namespace Snake.Infrastructure
{
    public enum GameColor { Snake, Food, Background }

    public static class GameColors
    {
        public static readonly Dictionary<GameColor, SolidColorBrush> Colors = new()
        {
            [GameColor.Snake] = new(Color.FromRgb(76, 175, 80)),
            [GameColor.Food] = new(Color.FromRgb(244, 67, 54)),
            [GameColor.Background] = new(Color.FromRgb(30, 30, 30)),
        };
    }

    public enum Direction { Up, Down, Left, Right }
    public record Cell(int X, int Y);
}