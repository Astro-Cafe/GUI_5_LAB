using Chat_Endpoint.Services;
using Chat_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace Chat_Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        IHubContext<SignalRHub> hub;

        private static List<Message> messages = new List<Message>();

        public MessageController(IHubContext<SignalRHub> hub)
        {
            this.hub = hub;
        }

        [HttpPost]
        public void Create([FromBody] Message value)
        {
            messages.Add(value);
            this.hub.Clients.All.SendAsync("MessageCreated", value);
        }

        [HttpGet]
        public IEnumerable<Message> ReadAll()
        {
            return messages;
        }
    }
}
