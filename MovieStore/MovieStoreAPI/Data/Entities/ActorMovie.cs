using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStoreAPI.Base.Data;

namespace MovieStoreAPI.Data.Entities
{
    public class ActorMovie : BaseData
    {
        public int ActorId { get; set; }
        public int MovieId { get; set; }
        
        public Actor Actor { get; set; }
        public Movie Movie { get; set; }
    }

    public class ActorMovieConfigruration : IEntityTypeConfiguration<ActorMovie>
    {
        public void Configure(EntityTypeBuilder<ActorMovie> builder)
        {
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
            
            builder.Property(x => x.MovieId).IsRequired(true);
            builder.Property(x => x.ActorId).IsRequired(true);

            builder.HasIndex(x => x.MovieId);
            builder.HasIndex(x => x.ActorId);
        }
    }
}