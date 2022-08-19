using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer_Worker.Model
{
    public class MQLogMessages
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }
}
