using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using RomSoft.Models;
using System.Diagnostics;
using Business.Services.Contracts;
using DataAccess;
using Models;
using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.SignalR;
using RomSoft.Hubs;

namespace RomSoft.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArchiveService _archiveService;
        private readonly IHubContext<FileUploadedHub> _hubContext;
        private HubConnection _connection;
        private static string _connectionId;
        private static Dictionary<string, ArchivingLogs> _archiveLogs = new Dictionary<string, ArchivingLogs>();

        public HomeController(IArchiveService archiveService, IHubContext<FileUploadedHub> hubContext)
        {
            _archiveService = archiveService;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            InitSignalR();
            return View();
        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> Index(List<IFormFile> files)
        {

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName(); //using Temp file name just for the example
                    filePaths.Add(filePath);
                    using var stream = new MemoryStream();
                    await formFile.CopyToAsync(stream);

                    var success = await _archiveService.Archive(stream.ToArray(), _connectionId, formFile.FileName);
                    DoAfterArchiveTasks(success);
                }
            }

            // process uploaded files
            ViewBag.toast = "File sent to archiver successfully!";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> GetArchive()
        {
            var byteArray = await _archiveService.GetByConnectionId(_connectionId);
            return new FileContentResult(byteArray.ToArray(), "application/zip") { FileDownloadName = "Filename.zip" };
        }
        #region SignalR

        //should move all this in another class if there's more time
        private async void InitSignalR()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7024/ArchiveHub")
                .Build();

            _connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _connection.StartAsync();
            };

            await ConnectSignalR();
        }

        private async Task ConnectSignalR()
        {
            _connection.On<string>("Whisper", MessageHandler);

            try
            {
                await _connection.StartAsync();
                _connectionId = _connection.ConnectionId;
            }
            catch (Exception ex)
            {

            }
        }

        #endregion



        private void MessageHandler(string message)
        {
            _archiveLogs[_connectionId].Status = ArchiveStatus.Success;
            _archiveLogs[_connectionId].ArchiveStartTime = DateTime.Parse(message.Split(";")[0].Replace("StartDate:", "")); ;
            _archiveLogs[_connectionId].ArchiveTimeSpan = TimeSpan.Parse(message.Split(";")[1].Replace("Duration:", ""));

            _archiveService.SaveArchiveLog(_archiveLogs[_connectionId]);

            //send notification to clients
            Task.Run(async ()=> await _hubContext.Clients.All.SendAsync("FileArchived", $"Archive is completed and can be downloaded!"));
        }
        private void DoAfterArchiveTasks(bool started)
        {
            var log = _archiveService.CreateArchiveLog(_connectionId,
                started ? ArchiveStatus.Success : ArchiveStatus.Error);
            if (!_archiveLogs.ContainsKey(_connectionId))
            {
                _archiveLogs.Add(_connectionId, log);
            }
            else
            {
                _archiveLogs[_connectionId] = log;
            }
        }
    }
}