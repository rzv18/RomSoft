using Business.Repositories.Contracts;
using Models;

namespace DataAccess.Repositories
{
	public class ArchivingLogsRepository : GenericRepository<ArchivingLogs>, IArchivingLogsRepository
    {
        public ArchivingLogsRepository() : base()
        { }
    }
}
