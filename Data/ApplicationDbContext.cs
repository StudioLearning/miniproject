using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using miniproject.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<miniproject.Models.TheCourse> Course { get; set; }
    public DbSet<miniproject.Models.Order> Order { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        // builder.HasDefaultSchema("Identity");
        // builder.Entity<IdentityUser>(entity =>
        // {
        //     entity.ToTable(name: "User");
        // });
        // builder.Entity<IdentityRole>(entity =>
        // {
        //     entity.ToTable(name: "Role");
        // });
        // builder.Entity<IdentityUserRole<string>>(entity =>
        // {
        //     entity.ToTable("UserRoles");
        // });
        // builder.Entity<IdentityUserClaim<string>>(entity =>
        // {
        //     entity.ToTable("UserClaims");
        // });
        // builder.Entity<IdentityUserLogin<string>>(entity =>
        // {
        //     entity.ToTable("UserLogins");
        // });
        // builder.Entity<IdentityRoleClaim<string>>(entity =>
        // {
        //     entity.ToTable("RoleClaims");
        // });
        // builder.Entity<IdentityUserToken<string>>(entity =>
        // {
        //     entity.ToTable("UserTokens");
        // });
    }
}
