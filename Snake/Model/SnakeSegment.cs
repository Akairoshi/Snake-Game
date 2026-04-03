using Snake.Infrastructure;

namespace Snake.Model
{
    public class SnakeSegment : ObservableObject
    {
        public int GridX { get; }
        public int GridY { get; }
        public double CanvasX { get; }
        public double CanvasY { get; }

        public SnakeSegment(int gridX, int gridY, double cellSize) 
        {
            GridX = gridX;
            GridY = gridY;
            CanvasX = gridX * cellSize;
            CanvasY = gridY * cellSize;
        }
    }
}
