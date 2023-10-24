using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MovieStoreAPI.Data.Context;
using MovieStoreAPI.Data.Entities;
using MovieStoreAPI.Models.DTO;

namespace MovieStoreAPI.Operations.Validations
{
    public class IdValidator : AbstractValidator<int>
    {
        public IdValidator()
        {
            RuleFor(x => x).NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x).GreaterThan(0).WithMessage("Id must be greater than zero");
        }
    }
}
