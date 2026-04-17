using Snake.Data;
using Snake.Services.Interfaces;

public class RepositoryService : IRepositoryService
{
    public List<PlayerRecord> Load()
    {
        using var context = new AppDbContext();
        return context.Players.ToList();
    }

    public void Save(PlayerRecord record)
    {
        using var context = new AppDbContext();
        context.Players.Add(record);
        context.SaveChanges();
    }
    public void Remove(int id)
    {
        using var context = new AppDbContext();
        var record = context.Players.Find(id);
        if (record != null)
        {
            context.Players.Remove(record);
            context.SaveChanges();
        }
    }
}