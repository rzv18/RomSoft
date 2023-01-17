using ArchiverWebApi.Hubs;
using ArchiverWebApi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArchiverWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHubContext<ArchiveHub, IArchiveHubClient> _hubContext;
        private readonly IArchiverService _archiverService;
        private static readonly Dictionary<string, byte[]> FilesByConnectionId = new();

        public ValuesController(IHubContext<ArchiveHub, IArchiveHubClient> hubContext, IArchiverService archiverService)
        {
            _hubContext = hubContext;
            _archiverService = archiverService;
        }
        
        [HttpGet]
        public IActionResult Get(string connectionId)
        {
            if (!string.IsNullOrWhiteSpace(connectionId) && FilesByConnectionId.ContainsKey(connectionId))
            {
                //for now support only one file in the archive so using directly 
                return Ok(FilesByConnectionId[connectionId]);
            }

            return NotFound(new byte[] { });
        }

        [HttpPost]
        public IActionResult Post([FromBody] ArchiveUploadModel model)
        {
            //Basic validation
            if (string.IsNullOrWhiteSpace(model.ConnectionId) || model?.ContenStream == null)
            {
                return Ok(false);
            }

            //This can start running asap. No need to wait for its answer as we will use signalR after
            Task.Run(async () =>
            {
                var startTime = DateTime.Now;
                var success = _archiverService.TryArchive(model.ContenStream, out byte[] archiveZipped, model.ConnectionId, model.Filename);
                if (success)
                {
                    if (FilesByConnectionId.ContainsKey(model.ConnectionId))
                    {
                        FilesByConnectionId[model.ConnectionId] = archiveZipped;
                    }
                    else
                    {
                        FilesByConnectionId.Add(model.ConnectionId, archiveZipped);
                    }

                    //Warn that the file is archived
                    await _hubContext.Clients.Client(model.ConnectionId).Whisper($"StartDate:{startTime};Duration:{DateTime.Now - startTime}");
                }

            });

            return Ok(true);
        }

    }
}
