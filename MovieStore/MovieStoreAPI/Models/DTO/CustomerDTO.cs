
namespace MovieStoreAPI.Models.DTO
{
    public class CustomerDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public List<int> FavGenreIds { get; set; }
    
    }
}