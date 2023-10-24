using FluentValidation;
using MovieStoreAPI.Data.Context;
using MovieStoreAPI.Data.Entities;
using MovieStoreAPI.Models.DTO;

namespace MovieStoreAPI.Operations.Validations
{
    public class PaymentValidator : AbstractValidator<PaymentDTO>
    {
        private readonly MovieStoreDBContext _context;
        public PaymentValidator(MovieStoreDBContext context)
        {
            _context = context;
            RuleFor(x => x.CustomerId).Must(i => IsValidId(_context,i,"customer")).WithMessage("Genre not found!");
            RuleFor(x => x.MovieId).Must(i => IsValidId(_context,i,"movie")).WithMessage("Director not found.");
        }
        private bool IsValidId(MovieStoreDBContext context,int id, string entityname)
        {
            if(entityname == "customer")
                return context.Set<Customer>().FirstOrDefault(i => i.Id == id) is not null;
            return context.Set<Movie>().FirstOrDefault(i => i.Id == id) is not null;
        }
    }
}
