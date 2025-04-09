using System;
using System.Data;
using FluentValidation;
using RestWebAPI.Entities;
namespace RestWebAPI.Validators
{
    public class PhoneValidator : AbstractValidator<Phone>
    {
        public PhoneValidator(bool isUpdateOperation = false)
        {
            RuleFor(ph => ph.Id)
                .NotEmpty()
                .WithMessage("Id is required.");
            

            RuleFor(ph => ph.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .Length(1, 50)
                .WithMessage("Name must be between 1 and 50 characters long.");

        }
    }
}