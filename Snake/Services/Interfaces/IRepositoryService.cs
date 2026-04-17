namespace Snake.Services.Interfaces
{
    public interface IRepositoryService
    {
        public List<PlayerRecord> Load();
        public void Save(PlayerRecord record);
        public void Remove(int id);
    }
}
