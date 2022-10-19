using FluentValidation;

namespace Project.DTO.User.UserValidators;

public class UpdateDtoValidator : AbstractValidator<UserToUpdateDto>
{
    public UpdateDtoValidator()
    {
        RuleFor(p => p.Username).NotNull();
    }
}