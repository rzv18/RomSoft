using Models;

namespace Business.Services.Contracts
{
	public interface IArchivingLogsService
	{
		void AddLog(ArchivingLogs log);
	}
}
