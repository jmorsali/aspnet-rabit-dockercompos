using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Consumer_Worker.Model.EntityConfiguration
{
    public class MQLogMessagesEntityConfiguration : EntityTypeConfiguration<MQLogMessages>
    {
        public MQLogMessagesEntityConfiguration()
        {

            ToTable("MQMessageStore");

            HasKey(s => s.Id);
            Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.uId)
                .HasColumnName("UniqeIdentity")
                .HasColumnOrder(2)
                .IsRequired();

            Property(p => p.Message).IsRequired()
                .HasMaxLength(500);

            Property(p => p.CreateDateTime).IsRequired();


        }
    }
}
