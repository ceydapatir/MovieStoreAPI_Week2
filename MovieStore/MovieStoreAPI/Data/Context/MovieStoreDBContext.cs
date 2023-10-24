using MovieStoreAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using MovieStoreAPI.Base.Data.SeedData;
using Microsoft.EntityFrameworkCore.Design;

namespace MovieStoreAPI.Data.Context
{
    public class MovieStoreDBContext : DbContext, IMovieStoreDBContext
    {
        public MovieStoreDBContext(DbContextOptions<MovieStoreDBContext> options) : base(options) {}

        public DbSet<ActorMovie> ActorMovies { get; set; }
        public DbSet<FavGenreCustomer> FavGenreCustomers { get; set; }
        public DbSet<Movie> Movies { get; set;}
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Payment> Payments { get; set; }

        //Unable to create an object of type 'MovieStoreDBContext'. For the different patterns supported at design time
        /*public class MovieStoreDBContextFactory : IDesignTimeDbContextFactory<MovieStoreDBContext>
        {
            public MovieStoreDBContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<MovieStoreDBContext>();
                optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=MovieStore;Trusted_Connection=true;TrustServerCertificate=True;MultipleActiveResultSets=true;");

                return new MovieStoreDBContext(optionsBuilder.Options);
            }
        }*/

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=MovieStore;Trusted_Connection=true;TrustServerCertificate=True;MultipleActiveResultSets=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FavGenreCustomerConfigruration());
            modelBuilder.ApplyConfiguration(new ActorMovieConfigruration());
            modelBuilder.ApplyConfiguration(new CustomerConfigruration());
            modelBuilder.ApplyConfiguration(new MovieConfigruration());
            modelBuilder.ApplyConfiguration(new GenreConfigruration());
            modelBuilder.ApplyConfiguration(new DirectorConfigruration());
            modelBuilder.ApplyConfiguration(new ActorConfigruration());
            modelBuilder.ApplyConfiguration(new PaymentConfigruration());

            new SeedData(modelBuilder).Seed();
            base.OnModelCreating(modelBuilder);
        }

    }
}