using Snake.Infrastructure;

namespace Snake.Model
{
    public class SnakeSegment : ObservableObject
    {
        public double CanvasX { get; }
        public double CanvasY { get; }

        public SnakeSegment(int gridX, int gridY, double cellSize) 
        {
            CanvasX = gridX * cellSize;
            CanvasY = gridY * cellSize;
        }
    }
}
