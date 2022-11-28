using FluentValidation;

namespace DTO.User.Validators;

public class AddDtoValidator : AbstractValidator<UserToAddDto>
{
    public AddDtoValidator()
    {
        RuleFor(p => p.Username).NotNull();
        RuleFor(p => p.Password).NotNull();
        RuleFor(p => p.Password).Equal(p => p.PasswordConfirmation);
    }
}