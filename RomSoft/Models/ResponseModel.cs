namespace RomSoft.Models
{
    public class ResponseModel
    {
        public string ConnectionId { get; set; }
        public Exception Error { get; set; }
        public bool Success { get; set; }
    }
}
