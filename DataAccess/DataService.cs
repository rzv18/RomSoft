namespace DataAccess
{
	public class DataService
	{
		public DataService(ApplicationDbContext dbContext)
		{
			DbContext = dbContext;
		}
		private ApplicationDbContext DbContext { get; }
	}
}
