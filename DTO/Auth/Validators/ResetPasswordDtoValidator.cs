using FluentValidation;

namespace DTO.Auth.Validators;

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordDtoValidator()
    {
        RuleFor(p => p.Email).NotNull();
        RuleFor(p => p.Password).NotNull();
        //RuleFor(x => x.Age).InclusiveBetween(18, 60).WithMessage("must be 18 and 60");
        RuleFor(p => p.Password).Equal(p => p.PasswordConfirmation);
    }
}