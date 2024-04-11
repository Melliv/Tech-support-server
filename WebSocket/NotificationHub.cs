using Microsoft.AspNetCore.SignalR;

namespace WebSocket;

public sealed class NotificationHub(IHubContext<NotificationHub, INotificationClient> context)
    : Hub<INotificationClient>
{
    public async Task SendMessage(NotificationMessage message)
    {
        await context.Clients.All.ReceiveMessage(message.ToString());
    }
}