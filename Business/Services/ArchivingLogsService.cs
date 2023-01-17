using Business.Repositories.Contracts;
using Business.Services.Contracts;
using Models;

namespace Business.Services
{
	public class ArchivingLogsService: IArchivingLogsService
	{
		private readonly IArchivingLogsRepository _archivingLogsRepository;

		public ArchivingLogsService(IArchivingLogsRepository archivingLogsRepository)
		{
			_archivingLogsRepository = archivingLogsRepository;
		}

		public void AddLog(ArchivingLogs log)
		{
			_archivingLogsRepository.Add(log);
		}


	}
}
