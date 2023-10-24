using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStoreAPI.Base.Data;

namespace MovieStoreAPI.Data.Entities
{
    public class Movie : BaseData
    {
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public int GenreId { get; set; }
        public int DirectorId { get; set; }
        public double Price { get; set; }

        public List<ActorMovie> ActorMovies { get; set; }
        public List<Payment> Payments { get; set; }
        public Director Director { get; set; }
        public Genre Genre { get; set; }
    }

    public class MovieConfigruration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.PublishDate).IsRequired();
            builder.Property(x => x.Price).IsRequired(true);

            builder.Property(x => x.GenreId).IsRequired(true);
            builder.Property(x => x.DirectorId).IsRequired(true);
            
            builder.HasIndex(x => x.GenreId);
            builder.HasIndex(x => x.DirectorId);
            builder.HasMany(x => x.ActorMovies);
            builder.HasMany(x => x.Payments);
        }
    }
}