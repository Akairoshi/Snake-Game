using Snake.Infrastructure;

namespace Snake.Model
{
    public class Segment
    {
        public int GridX { get; }
        public int GridY { get; }

        public Segment(int gridX, int gridY) 
        {
            GridX = gridX;
            GridY = gridY;
        }
    }
}
