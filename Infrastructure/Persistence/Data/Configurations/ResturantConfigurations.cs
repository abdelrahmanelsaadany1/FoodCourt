using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class ResturantConfigurations : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder
                .HasOne(r => r.Chef)              // Restaurant.Chef → navigation to User
                .WithMany()                       // No navigation in User back to Restaurant
                .HasForeignKey(r => r.ChefId)     // Use Restaurant.ChefId as FK
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}