using API.Domain.Models.Post;
using API.Domain.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Data;

/// <summary>
/// Application database context class.
/// </summary>
public class AppDbContext : IdentityDbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>
    /// Gets or sets the Users table.
    /// </summary>b
    public new DbSet<UserModel> Users { get; set; }

    /// <summary>
    /// Gets or sets the Posts table.
    /// </summary>
    public DbSet<PostModel> Posts { get; set; }

    /// <summary>
    /// Configure the model relationships and other conventions.
    /// </summary>
    /// <param name="builder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure relationship between PostModel and UserModel
        builder.Entity<PostModel>()
            .HasOne(m => m.User)
            .WithMany().HasForeignKey(m => m.UserId);
    }
}