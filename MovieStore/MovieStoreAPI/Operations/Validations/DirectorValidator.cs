using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MovieStoreAPI.Data.Context;
using MovieStoreAPI.Data.Entities;
using MovieStoreAPI.Models.DTO;

namespace MovieStoreAPI.Operations.Validations
{
    public class DirectorValidator : AbstractValidator<DirectorDTO>
    {
        public DirectorValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Name).MinimumLength(2).WithMessage("Name length min value is 2.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is required.");
            RuleFor(x => x.Surname).MinimumLength(2).WithMessage("Surname length min value is 2.");
        }
    }
}
