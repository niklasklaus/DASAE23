using Login_Page_7_0.Tables;
using Microsoft.EntityFrameworkCore;

namespace Login_Page_7_0;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> Users2 { get; set; }
}