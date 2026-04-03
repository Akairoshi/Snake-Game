namespace Snake.Services.Interfaces
{
    public interface ILogService
    {
        void LogError(string message, Exception ex);
        void LogInfo(string message);
        void LogWarning(string message, Exception? ex = null);
        void ShowDialog(string title);
    }
}
