namespace worker.Model;

public interface IMqLogMessageRepository
{
    void SaveMessageToDb(string message);
}