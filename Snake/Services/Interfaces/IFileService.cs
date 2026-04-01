using System.Text;
using System.Windows;

namespace Snake.Services.Interfaces
{
    public interface IFileService
    {
        string ReadFile(string filePath);
        void WriteToFile(string filePath, string text, Encoding encoding);

    }
}