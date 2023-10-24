
using MovieStoreAPI.Base.Data;

namespace MovieStoreAPI.Models.ViewModel
{
    public class MovieViewModel : BaseData
    {
        public string Name { get; set; }
        public string PublishDate { get; set; }
        public double Price { get; set; }

        public List<string> Actors { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
    }
}