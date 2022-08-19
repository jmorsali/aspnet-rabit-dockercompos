using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Hierarchy;

namespace Consumer_Worker.Model
{
    public class MQLogMessages
    {
        public int Id { get; set; }
        public Guid uId { get; set; }
        public string Message { get; set; }

        public DateTime CreateDateTime { get; set; }
        //public HierarchyId ConcurrencyToken { get; set; }
    }
}
