using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStoreAPI.Base.Data;

namespace MovieStoreAPI.Data.Entities
{
    public class Director : BaseData
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public List<Movie> Movies { get; set; }
    }

    public class DirectorConfigruration : IEntityTypeConfiguration<Director>
    {
        public void Configure(EntityTypeBuilder<Director> builder)
        {
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(50);

            builder.HasMany(x => x.Movies).WithOne(x => x.Director).HasForeignKey(x => x.DirectorId);
        }
    }
}