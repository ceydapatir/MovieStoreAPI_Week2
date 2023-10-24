using Microsoft.EntityFrameworkCore;
using MovieStoreAPI.Data.Entities;

namespace MovieStoreAPI.Data.Context
{
    public interface IMovieStoreDBContext
    {
        public DbSet<ActorMovie> ActorMovies { get; set; }
        public DbSet<FavGenreCustomer> FavGenreCustomers { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        
    }
}