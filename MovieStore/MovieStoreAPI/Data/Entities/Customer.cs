using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStoreAPI.Base.Data;

namespace MovieStoreAPI.Data.Entities
{
    public class Customer : BaseData
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }


        public List<FavGenreCustomer> FavGenreCustomers { get; set;}
        public List<Payment> Payments { get; set; }
    }

    public class CustomerConfigruration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Username).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Password).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Role).IsRequired().HasMaxLength(50);

            builder.HasMany(x => x.FavGenreCustomers);
            builder.HasMany(x => x.Payments).WithOne(x => x.Customer).HasForeignKey(x => x.CustomerId);
        }
    }
}