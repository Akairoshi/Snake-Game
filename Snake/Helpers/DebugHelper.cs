#if DEBUG
using Snake.Model;

namespace Snake.Helpers
{
    public static class DebugHelper
    {
        public static void SetScore(Player player, int score)
        {
            typeof(Player)
                .GetProperty(nameof(player.Score))!
                .SetValue(player, score);
        }
        public static void SetSpeed(Player player, int speed)
        {
            typeof(Player)
                .GetProperty(nameof(player.Speed))!
                .SetValue(player, speed);
        }
    }
}
#endif