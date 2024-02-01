using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities;
public class RepositoryContext : IdentityDbContext<User>
{
    public RepositoryContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new RoleConfiguration());

        modelBuilder.Entity<LoyaltyCustomer>()
        .HasKey(lc => new { lc.AgentId, lc.CustomerId });
    }

    public DbSet<Agent> Agents { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<LoyaltyCustomer> LoyaltyCustomers { get; set; }
    
}
