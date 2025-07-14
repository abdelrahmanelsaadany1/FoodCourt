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
                .HasOne(r => r.Chef)
                .WithOne(u => u.Restaurant)
                .HasForeignKey<Restaurant>(r => r.ChefId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
