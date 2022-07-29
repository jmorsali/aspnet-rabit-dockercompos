using System;

namespace worker.Model;

public class MqLogMessageRepository : IMqLogMessageRepository
{
    private readonly MessagesStoreContext _context;

    public MqLogMessageRepository(MessagesStoreContext context)
    {
        _context = context;
    }
    public void SaveMessageToDb(string message)
    {
        var dbmMessage = new MQLogMessages { Id = Guid.NewGuid(), Message = message };
        _context.Add(dbmMessage);
        _context.SaveChanges();
    }
}