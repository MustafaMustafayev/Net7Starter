using FluentValidation;

namespace Project.DTO.User.UserValidators;

public class UserToUpdateDtoValidator : AbstractValidator<UserToUpdateDto>
{
    public UserToUpdateDtoValidator()
    {
        RuleFor(p => p.Username).NotNull();
    }
}