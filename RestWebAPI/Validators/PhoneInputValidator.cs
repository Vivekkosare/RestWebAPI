using FluentValidation;
using RestWebAPI.Entities;

namespace RestWebAPI.Validators
{
    public class PhoneInputValidator : AbstractValidator<PhoneInput>
    {
        public PhoneInputValidator()
        {
            RuleFor(ph => ph.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .Length(1, 50)
                .WithMessage("Name must be between 1 and 50 characters long.");
        }
    }
}
