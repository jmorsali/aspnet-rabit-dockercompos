using System;
using System.Data.Entity.Hierarchy;

namespace Consumer_Worker.Model;

public class MqLogMessageRepository : IMqLogMessageRepository
{
    private readonly MessagesStoreContext _context;

    public MqLogMessageRepository(MessagesStoreContext context)
    {
        _context = context;
    }
    public void SaveMessageToDb(string message)
    {
        var dbmMessage = new MQLogMessages { uId = Guid.NewGuid(), Message = message , CreateDateTime=DateTime.Now};
        _context.Add(dbmMessage);
        _context.SaveChanges();
    }
}