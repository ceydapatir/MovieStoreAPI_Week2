using FluentValidation;
using MovieStoreAPI.Data.Context;
using MovieStoreAPI.Data.Entities;
using MovieStoreAPI.Models.DTO;

namespace MovieStoreAPI.Operations.Validations
{
    public class MovieValidator : AbstractValidator<MovieDTO>
    {
        private readonly MovieStoreDBContext _context;
        public MovieValidator(MovieStoreDBContext context)
        {
            _context = context;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Name).MinimumLength(2).WithMessage("Name length min value is 2.");
            RuleFor(x => x.PublishDate).NotEmpty().WithMessage("Publish date is required.");
            RuleFor(x => x.PublishDate).NotEmpty().WithMessage("Publish date is required.");
            RuleFor(x => x.GenreId).NotEmpty().WithMessage("Genre id is required.");
            RuleFor(x => x.GenreId).Must(i => IsValidId(_context,i,"genre")).WithMessage("Genre not found!");
            RuleFor(x => x.DirectorId).NotEmpty().WithMessage("Director id is required.");
            RuleFor(x => x.DirectorId).Must(i => IsValidId(_context,i,"director")).WithMessage("Director not found.");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            
            RuleFor(x => x.ActorIds).Must(i => IsValidId(_context,i)).WithMessage("Genre not found");
        }
        
        private bool IsValidId(MovieStoreDBContext context,int id, string entityname)
        {
            if(entityname == "genre")
                return context.Set<Genre>().FirstOrDefault(i => i.Id == id) is not null;
            return context.Set<Director>().FirstOrDefault(i => i.Id == id) is not null;
        }
        
        private bool IsValidId(MovieStoreDBContext context,List<int> ids)
        {
            foreach (var id in ids)
            {
                if(context.Set<Actor>().FirstOrDefault(i => i.Id == id) is null)
                    return false;
            }
            return true;
        }
    }
}
