using Microsoft.EntityFrameworkCore;
using GrobsJobsRazorPages.Model;

namespace GrobsJobsRazorPages.Data;

public partial class GrobsJobsRazorPagesDbContext : DbContext
{
    public GrobsJobsRazorPagesDbContext(DbContextOptions<GrobsJobsRazorPagesDbContext> options) : base(options) { }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Message> Messages { get; set; }
}