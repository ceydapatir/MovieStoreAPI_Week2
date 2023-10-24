using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStoreAPI.Base.Data;

namespace MovieStoreAPI.Data.Entities
{
    public class Actor : BaseData
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public List<ActorMovie> ActorMovies { get; set; }
    }

    public class ActorConfigruration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(50);

            
            builder.HasMany(x => x.ActorMovies);
        }
    }
}