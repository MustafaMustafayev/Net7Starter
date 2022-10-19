using FluentValidation;

namespace Project.DTO.User.UserValidators;

public class AddDtoValidator : AbstractValidator<UserToAddDto>
{
    public AddDtoValidator()
    {
        RuleFor(p => p.Username).NotNull();
        RuleFor(p => p.Password).NotNull();
        RuleFor(p => p.Password).Equal(p => p.PasswordConfirmation);
    }
}