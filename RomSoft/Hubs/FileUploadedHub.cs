using Microsoft.AspNetCore.SignalR;

namespace RomSoft.Hubs
{
    public class FileUploadedHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("FileArchived", message);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
