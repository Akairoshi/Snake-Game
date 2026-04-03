using Snake.Infrastructure;

namespace Snake.Model
{
    public class Player
    {
        public int Score { get; private set; }
        public double Speed { get; private set; }
        public Direction CurrentDirection { get; set; } = Direction.Left;

        public event Action? Died;
        public event Action? ScoreChanged;

        public Player(double speed)
        {
            Speed = speed;
        }

        public void InreaseSpeed(double increment)
        {
            Speed += increment;
        }
        public void DecreaseSpeed(double decrement)
        {
            Speed -= decrement;
        }
        public void AddScore(int score)
        {
            Score += score;
            ScoreChanged?.Invoke();
        }
        public void ChangeDirection(Direction moveDirection)
        {
            CurrentDirection = moveDirection;
        }
        public void Move()
        {
            //if ()
            //{
            //    ScoreChanged?.Invoke(Score);
            //}
            //if ()
            //{
            //    Died?.Invoke();
            //}

        }
        public void Reset()
        {
            Score = 0;
            CurrentDirection = Direction.Left;
        }
    }
}
