using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "68a2bc1b-4b5c-4714-b7be-afdcac09caac";
            var writerRoleId = "c0df36c3-193e-45d9-b157-0e37fdd96521";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id =readerRoleId,
                    ConcurrencyStamp =  readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                },
                new IdentityRole
                {
                    Id =writerRoleId,
                    ConcurrencyStamp =  writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
