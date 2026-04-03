using System.Windows.Controls;

namespace Snake.Model
{
    public class Food
    {
        public int GridX { get; }
        public int GridY { get; }
        public double CanvasX { get; }
        public double CanvasY { get; }

        public Food(int gridX, int gridY, double cellSize)
        {
            GridX = gridX;
            GridY = gridY;
            CanvasX = gridX * cellSize;
            CanvasY = gridY * cellSize;
        }

    }
}
