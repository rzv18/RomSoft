using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext():base()
		{

		}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {            

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //this can be moved to a appsetting, but will leave it here for now
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=testinterview.colorful.hr; Initial Catalog=DB_Razvan_I; User Id=candidat; Password=NkvDPYVk8Q27EjdT;Trusted_Connection=True;TrustServerCertificate=True;Integrated Security=false;", builder => builder.EnableRetryOnFailure());
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<ArchivingLogs>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Archivin__3214EC0712D70AE8");

                entity.Property(e => e.ArchiveStartTime).HasColumnType("datetime");
            });
        }

		public virtual DbSet<ArchivingLogs> ArchivingLogs { get; set; }
	}
}
