using Microsoft.AspNetCore.SignalR;

namespace message.Hubs;


public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ChatMessages", user, message);
    }
}
