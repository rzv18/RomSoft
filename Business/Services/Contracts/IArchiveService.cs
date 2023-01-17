using Models;

namespace Business.Services.Contracts
{
	public interface IArchiveService
	{
		Task<bool> Archive(byte[] content, string connectionId);
        Task<byte[]> GetByConnectionId(string connectionId);
        void SaveArchiveLog(ArchivingLogs model);

        ArchivingLogs CreateArchiveLog(string connectionId, ArchiveStatus status);
    }
}
