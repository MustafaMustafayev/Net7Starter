using FluentValidation;

namespace DTO.User.Validators;

public class AddDtoValidator : AbstractValidator<UserToAddDto>
{
    public AddDtoValidator()
    {
        RuleFor(p => p.Username).NotNull();
        RuleFor(p => p.RoleId).NotNull();
        RuleFor(p => p.ContactNumber).NotNull();
        RuleFor(p => p.Email).NotNull().EmailAddress();
        RuleFor(p => p.Password).NotNull().MinimumLength(5);
        RuleFor(p => p.PasswordConfirmation).Equal(p => p.Password);
    }
}