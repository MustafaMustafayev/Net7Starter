using FluentValidation;

namespace Project.DTO.DTOs.UserDto.UserValidators;

public class UserToAddDtoValidator : AbstractValidator<UserToAddDto>
{
    public UserToAddDtoValidator()
    {
        RuleFor(p => p.Username).NotNull();
        RuleFor(p => p.Password).NotNull();
        RuleFor(p => p.Password).Equal(p => p.PasswordConfirmation);
    }
}