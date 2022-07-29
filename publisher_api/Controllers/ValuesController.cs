using System;
using System.Collections.Generic;
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

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "queue-test" };
        }

        [HttpPost]
        public ActionResult<bool> Post([FromBody] string payload)
        {
            Console.WriteLine("received a Post: " + payload);
            var result = _messageService.Enqueue(payload);
            return Ok(result);
        }

    }
}
