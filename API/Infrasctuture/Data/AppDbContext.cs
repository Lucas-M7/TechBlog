using API.Domain.Models.Post;
using API.Domain.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Infrasctuture.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext(options)
{
    public new DbSet<UserModel> Users { get; set; }
    public DbSet<PostModel> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<PostModel>()
            .HasOne(m => m.User)
            .WithMany().HasForeignKey(m => m.UserId);
    }
}