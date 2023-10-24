using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStoreAPI.Base.Data;

namespace MovieStoreAPI.Data.Entities
{
    public class Payment : BaseData
    {
        public int CustomerId { get; set; }
        public int MovieId { get; set; }
        public DateTime PaymentDate { get; set; }
        
        public Customer Customer { get; set; }
        public Movie Movie { get; set; }
    }

    public class PaymentConfigruration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

            builder.Property(x => x.PaymentDate).IsRequired();

            builder.Property(x => x.MovieId).IsRequired(true);
            builder.Property(x => x.CustomerId).IsRequired(true);

            builder.HasIndex(x => x.MovieId);
            builder.HasIndex(x => x.CustomerId);
        }
    }
}