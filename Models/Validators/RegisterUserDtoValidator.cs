using FluentValidation;
using RentItAPI.Entities;
using System.Linq;

namespace RentItAPI.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(AppDbContext dbContext)
        {
            RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress();
            RuleFor(x => x.Password).MinimumLength(6);
            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);

            RuleFor(x => x.Email).Custom((value, context) =>
            {
                var emailInUse = dbContext.Users.Any(u => u.Email == value);
                if (emailInUse)
                {
                    context.AddFailure("Email", "This email is already taken.");
                }
            });
        }
    }
}