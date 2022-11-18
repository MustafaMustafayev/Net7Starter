using FluentValidation;

namespace DTO.Role.Validators;

public class AddDtoValidator : AbstractValidator<RoleToAddDto>
{
    public AddDtoValidator()
    {
        RuleFor(p => p.RoleId).NotNull();
        RuleFor(p => p.Name).NotNull();
    }
}