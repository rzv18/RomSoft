namespace Models
{
    public class ArchivingLogs
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public DateTime ArchiveStartTime { get; set; }
        public TimeSpan ArchiveTimeSpan { get; set; }
        public ArchiveStatus Status { get; set; }
    }

    public enum ArchiveStatus
    {
        Success = 1, Error = 2
    }
}
