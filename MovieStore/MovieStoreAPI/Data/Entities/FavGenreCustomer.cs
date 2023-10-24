using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStoreAPI.Base.Data;

namespace MovieStoreAPI.Data.Entities
{
    public class FavGenreCustomer : BaseData
    {
        public int CustomerId { get; set; }
        public int GenreId { get; set; }
        
        public Genre Genre { get; set; }
        public Customer Customer { get; set; }
    }

    public class FavGenreCustomerConfigruration : IEntityTypeConfiguration<FavGenreCustomer>
    {
        public void Configure(EntityTypeBuilder<FavGenreCustomer> builder)
        {
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
            
            builder.Property(x => x.CustomerId).IsRequired(true);
            builder.Property(x => x.GenreId).IsRequired(true);

            builder.HasIndex(x => x.CustomerId);
            builder.HasIndex(x => x.GenreId);
        }
    }
}