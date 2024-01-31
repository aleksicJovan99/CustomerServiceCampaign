using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities;
public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
        new IdentityRole 
        {
            Name = "Agent",
            NormalizedName = "AGENT" 
        },
        new IdentityRole 
        {
            Name = "SysAdmin",
            NormalizedName = "SYSADMIN" 
        });
    }
}
