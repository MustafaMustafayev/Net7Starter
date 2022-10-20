using FluentValidation;

namespace Project.DTO.Role.RoleValidators;

public class AddDtoValidator : AbstractValidator<RoleToAddDto>
{
    public AddDtoValidator()
    {
        RuleFor(p => p.RoleId).NotNull();
        RuleFor(p => p.Name).NotNull();
    }
}