
    using Microsoft.AspNetCore.SignalR;

    namespace ArchiverWebApi.Hubs
    {
        public interface IArchiveHubClient
        {
            Task Whisper(string message);
        }

        public class ArchiveHub : Hub<IArchiveHubClient>
        {
            public async Task Whisper(string connectionIdTarget, string message)
            {
                await Clients.Client(connectionIdTarget).Whisper(message);
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

