
using MovieStoreAPI.Base.Data;
using MovieStoreAPI.Data.Entities;

namespace MovieStoreAPI.Models.ViewModel
{
    public class CustomerViewModel : BaseData
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}