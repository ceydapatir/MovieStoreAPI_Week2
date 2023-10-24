
namespace MovieStoreAPI.Models.DTO
{
    public class MovieDTO
    {        
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public int GenreId { get; set; }
        public int DirectorId { get; set; }
        public double Price { get; set; }

        public List<int> ActorIds { get; set; }
    }
}