using JobTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

public class JobDbContext: DbContext
{
    public JobDbContext(DbContextOptions<JobDbContext> options) : base(options)
    {
    }

    public DbSet<Job> Jobs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Connection string for Postgres when running locally
        optionsBuilder.UseNpgsql("Host=localhost;Database=jobdb;Username=postgres;Password=password");
    }
}