using Microsoft.EntityFrameworkCore;
using MovieStoreAPI.Data.Entities;

namespace MovieStoreAPI.Base.Data.SeedData
{
    public class SeedData
    {
        private readonly ModelBuilder _modelBuilder;

        public SeedData(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            _modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Comedy", IsActive = true },
                new Genre { Id = 2, Name = "Horror", IsActive = true },
                new Genre { Id = 3, Name = "Anime", IsActive = true }
            );
            _modelBuilder.Entity<Movie>().HasData(
                new Movie { Id = 1, Name = "Murder Mystery 2", PublishDate = DateTime.Now.AddYears(-2), GenreId = 1, DirectorId = 1, Price = 18.99, IsActive = true },
                new Movie { Id = 2, Name = "Barbie", PublishDate = DateTime.Now.AddMonths(-5), GenreId = 1, DirectorId = 2, Price = 18.99, IsActive = true }
            );
            _modelBuilder.Entity<Director>().HasData(
                new Director { Id = 1, Name = "Jeremy", Surname = "Garelick", IsActive = true },
                new Director { Id = 2, Name = "Greta", Surname = "Gerwig", IsActive = true }
            );
            _modelBuilder.Entity<Actor>().HasData(
                new Actor { Id = 1, Name = "Jennifer", Surname = "Aniston", IsActive = true },
                new Actor { Id = 2, Name = "Adam", Surname = "Sandler", IsActive = true },
                new Actor { Id = 3, Name = "Margot", Surname = "Robbie", IsActive = true },
                new Actor { Id = 4, Name = "Ryan", Surname = "Gosling", IsActive = true }
            );
            _modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Ceyda", Surname = "Keklik", Username = "Ceyda1", Password = "5d41402abc4b2a76b9719d911017c592", Role = "admin", IsActive = true },
                new Customer { Id = 2, Name = "Ufuk", Surname = "Suat", Username = "Ufuk1", Password = "5d41402abc4b2a76b9719d911017c592", Role = "customer", IsActive = true }
            );
        }
    }
}