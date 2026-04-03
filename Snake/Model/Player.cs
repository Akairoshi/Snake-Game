using Snake.Infrastructure;
using System.Collections.ObjectModel;

namespace Snake.Model
{
    public class Player
    {
        public ObservableCollection<SnakeSegment> Snake { get; } = new();
        public int Score { get; private set; }
        public Direction CurrentDirection { get; set; } = Direction.Left;

        private bool _IsDirectionChanged = false;

        public event Action? Died;
        public event Action<int, int>? FoodEaten;
        public event Action? ScoreChanged;

        public Player()
        {
        }
        public void AddScore(int score)
        {
            Score += score;
            ScoreChanged?.Invoke();
        }
        public void ChangeDirection(Direction newDirection)
        {
            if (_IsDirectionChanged)
                return;
            bool isOpposite =
                (newDirection == Direction.Up && CurrentDirection == Direction.Down) ||
                (newDirection == Direction.Down && CurrentDirection == Direction.Up) ||
                (newDirection == Direction.Left && CurrentDirection == Direction.Right) ||
                (newDirection == Direction.Right && CurrentDirection == Direction.Left);

            if (!isOpposite)
                CurrentDirection = newDirection;
            _IsDirectionChanged = true;
        }
        public void Move(IEnumerable<Food> foods, int gridSize, double cellSize)
        {
            var head = Snake[0];
            int newX = head.GridX;
            int newY = head.GridY;

            switch(CurrentDirection)
            {
                case Direction.Up:
                    newY -= 1;
                    break;
                case Direction.Down:
                    newY += 1;
                    break;
                case Direction.Left:
                    newX -= 1;
                    break;
                case Direction.Right:
                    newX += 1;
                    break;
            }

            if(newX < 0 || newX >= gridSize || newY < 0 || newY >= gridSize)
            {
                Died?.Invoke();
                return;
            }

            if (Snake.Any(s => s.GridX == newX && s.GridY == newY))
            {
                Died?.Invoke();
                return;
            }

            var eaten = foods.FirstOrDefault(f => f.GridX == newX && f.GridY == newY);
            bool ateFood = eaten != null;
            if (ateFood)
                FoodEaten?.Invoke(eaten!.GridX, eaten.GridY);
            
            Snake.Insert(0, new SnakeSegment(newX, newY, cellSize));

            if (!ateFood)
                Snake.RemoveAt(Snake.Count - 1);

            _IsDirectionChanged = false;

        }
        public void Spawn(int gridX, int gridY, double cellSize)
        {
            Snake.Clear();
            Snake.Add(new SnakeSegment(gridX, gridY, cellSize));
            Snake.Add(new SnakeSegment(gridX + 1, gridY, cellSize));
        }
        public void Reset()
        {
            Score = 0;
            CurrentDirection = Direction.Left;
            Snake.Clear();
        }
    }
}
