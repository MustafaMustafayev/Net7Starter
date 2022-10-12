using FluentValidation;

namespace Project.DTO.DTOs.UserDto.UserValidators;

public class UserToUpdateDtoValidator : AbstractValidator<UserToUpdateDto>
{
    public UserToUpdateDtoValidator()
    {
        RuleFor(p => p.Username).NotNull();
    }
}