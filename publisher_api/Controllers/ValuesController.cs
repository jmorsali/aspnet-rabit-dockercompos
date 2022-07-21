using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using publisher_api.Services;

namespace publisher_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public ValuesController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "queue-test" };
        }

        [HttpPost]
        public void Post([FromBody] string payload)
        {
            Console.WriteLine("received a Post: " + payload);
            _messageService.Enqueue(payload);
        }

    }
}
