namespace WebSocket;

public interface INotificationClient
{
    Task ReceiveMessage(string message);
}