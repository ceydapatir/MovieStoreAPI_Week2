using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MovieStoreAPI.Data.Context;
using MovieStoreAPI.Data.Entities;
using MovieStoreAPI.Models.DTO;

namespace MovieStoreAPI.Operations.Validations
{
    public class CustomerValidator : AbstractValidator<CustomerDTO>
    {
        private readonly MovieStoreDBContext _context;
        public CustomerValidator(MovieStoreDBContext context)
        {
            _context = context;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Name).MinimumLength(2).WithMessage("Name length min value is 2.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is required.");
            RuleFor(x => x.Surname).MinimumLength(2).WithMessage("Surname length min value is 2.");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Username).MinimumLength(2).WithMessage("Username length min value is 2.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
            RuleFor(x => x.FavGenreIds).Must(i => IsValidId(_context,i)).WithMessage("Genre not found");
        }

        private bool IsValidId(MovieStoreDBContext context,List<int> ids)
        {
            foreach (var id in ids)
            {
                if(context.Set<Genre>().FirstOrDefault(i => i.Id == id) is null)
                    return false;
            }
            return true;
        }
    }
}
