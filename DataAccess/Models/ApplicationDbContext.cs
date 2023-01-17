using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ArchivingLog> ArchivingLogs { get; set; }

    /// <summary>
    /// this is just a scaffold connection only for the purpose of the demo
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=testinterview.colorful.hr;Database=DB_Razvan_I;user id=candidat;password=NkvDPYVk8Q27EjdT;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ArchivingLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Archivin__3214EC0712D70AE8");

            entity.Property(e => e.ArchiveStartTime).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
