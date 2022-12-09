using FluentValidation;

namespace DTO.User.Validators;

public class UpdateDtoValidator : AbstractValidator<UserToUpdateDto>
{
    public UpdateDtoValidator()
    {
        RuleFor(p => p.ContactNumber).NotNull();
        RuleFor(p => p.RoleId).NotNull();
        RuleFor(p => p.Email).NotNull().EmailAddress();
        RuleFor(p => p.Username).NotNull();
    }
}