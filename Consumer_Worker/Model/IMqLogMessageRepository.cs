namespace Consumer_Worker.Model;

public interface IMqLogMessageRepository
{
    void SaveMessageToDb(string message);
}