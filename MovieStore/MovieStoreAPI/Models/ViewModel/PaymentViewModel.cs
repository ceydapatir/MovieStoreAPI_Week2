
using MovieStoreAPI.Base.Data;

namespace MovieStoreAPI.Models.ViewModel
{
    public class PaymentViewModel : BaseData
    {
        public string CustomerFullName { get; set; }
        public string MovieName { get; set; }
        public double Price { get; set; }
        public string PaymentDate { get; set; }
    }
}