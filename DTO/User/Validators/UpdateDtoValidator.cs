using FluentValidation;

namespace DTO.User.Validators;

public class UpdateDtoValidator : AbstractValidator<UserToUpdateDto>
{
    public UpdateDtoValidator()
    {
        RuleFor(p => p.Username).NotNull();
    }
}