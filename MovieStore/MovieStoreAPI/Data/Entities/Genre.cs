using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStoreAPI.Base.Data;
namespace MovieStoreAPI.Data.Entities
{
    public class Genre : BaseData
    {
        public string Name { get; set; }
        public List<FavGenreCustomer> FavGenreCustomers { get; set;}
    }

    public class GenreConfigruration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            
            builder.HasMany(x => x.FavGenreCustomers);
        }
    }
}