using Chat_Endpoint.Services;
using Microsoft.AspNetCore.SignalR;

namespace Chat_Endpoint.Controllers
{
    public class MessageController
    {
        IHubContext<SignalRHub> hub;

        public MessageController(IHubContext<SignalRHub> hub)
        {
            this.hub = hub;
        }
    }
}
