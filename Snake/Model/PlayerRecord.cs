public class PlayerRecord
{
    public int Id { get; set; }
    public int Score { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public int GridSize { get; set; }
    public int Speed { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;

    public PlayerRecord() { }

    public PlayerRecord(int score, string nickname, int gridSize, int speed)
    {
        Score = score;
        Nickname = nickname;
        GridSize = gridSize;
        Speed = speed;
    }
}