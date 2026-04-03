using System.Windows.Controls;

namespace Snake.Model
{
    public class Food
    {
        public double CanvasX { get; }
        public double CanvasY { get; }

        public Food(int gridX, int gridY, double cellSize)
        {
            CanvasX = gridX * cellSize;
            CanvasY = gridY * cellSize;
        }

    }
}
