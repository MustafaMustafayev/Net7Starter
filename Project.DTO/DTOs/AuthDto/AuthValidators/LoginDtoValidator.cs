using FluentValidation;

namespace Project.DTO.DTOs.AuthDto.AuthValidators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(p => p.Email).NotNull().Length(7);
        RuleFor(p => p.Password).NotNull();
    }
}