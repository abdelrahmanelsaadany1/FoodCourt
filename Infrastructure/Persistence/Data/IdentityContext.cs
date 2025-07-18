using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class IdentityContext:IdentityDbContext<User>
    {

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Fix: Configure Address to use the existing migration table name
            builder.Entity<Address>().ToTable("Addresses"); // This will be the correct name after migration

            builder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId);

         
        }
        #region DbSets
        // public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
      
        #endregion
    }
}
//edit file to make sure main work correctly