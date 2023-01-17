using System.Net.Http.Headers;
using System.Net.Http.Json;
using Business.Services.Contracts;
using Models;

namespace Business.Services
{
	public class ArchiveService : IArchiveService
    {
        private readonly IArchivingLogsService _archivingLogsService;
        //This can be moved in a appsettings key, but hardcoded for now
        private const string ArchiverUri = "https://localhost:7024/";
        private const string ArchiverPath = "api/Values";
        
        public ArchiveService(IArchivingLogsService archivingLogsService)
        {
            _archivingLogsService = archivingLogsService;
        }

        public async Task<bool> Archive(byte[] stream, string connectionId, string filename)
        {
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ArchiverUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var uploadModel = new ArchiveUploadModel { ConnectionId = connectionId, ContenStream = stream, Filename = filename};
                var response = await client.PostAsJsonAsync(ArchiverPath, uploadModel);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine(httpEx);
                return false;
                // determine error here by inspecting httpEx.Message         
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<byte[]> GetByConnectionId(string connectionId)
        {
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ArchiverUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var response = await client.GetAsync($"{ArchiverPath}?connectionId={connectionId}");
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string? result = null;
                    result = response.Content.ReadAsStringAsync().Result.Replace("\"", string.Empty);
                    return Convert.FromBase64String(result);
                }
            }
            catch (HttpRequestException httpEx)
            {
                // determine error here by inspecting httpEx.Message     
                Console.WriteLine(httpEx);
                return new byte[]{};    
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return new byte[]{};
        }

        public void SaveArchiveLog(ArchivingLogs model)
        {
            _archivingLogsService.AddLog(model);
        }
        public ArchivingLogs CreateArchiveLog(string connectionId, ArchiveStatus status)
        {
            var log = new ArchivingLogs
            {
                ArchiveStartTime = DateTime.Now,
                Status = status,
                Filename = connectionId
            };

            //if failed, should add the log here
            if (log.Status == ArchiveStatus.Error)
            {
                _archivingLogsService.AddLog(log);
            }

            return log;
        }
    }
}
