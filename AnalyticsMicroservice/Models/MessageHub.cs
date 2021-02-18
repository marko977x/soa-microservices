using Microsoft.AspNetCore.SignalR;

namespace AnalyticsMicroservice.Models
{
    public class MessageHub : Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}