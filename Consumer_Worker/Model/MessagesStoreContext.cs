using Microsoft.EntityFrameworkCore;


namespace Consumer_Worker.Model;

public class MessagesStoreContext : DbContext
{
    public MessagesStoreContext()
    {
        
    }
    public MessagesStoreContext(DbContextOptions<MessagesStoreContext> options) : base(options)
    {
       
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);
    //}
    //entities
    public DbSet<MQLogMessages> MQMessageStore { get; set; }
}

