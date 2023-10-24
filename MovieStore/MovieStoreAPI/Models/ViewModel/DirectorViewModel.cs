
using MovieStoreAPI.Base.Data;
using MovieStoreAPI.Data.Entities;

namespace MovieStoreAPI.Models.ViewModel
{
    public class DirectorViewModel : BaseData
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public List<Movie> Movies { get; set; }
    }
}