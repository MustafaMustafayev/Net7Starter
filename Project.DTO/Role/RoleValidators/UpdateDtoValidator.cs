using FluentValidation;

namespace Project.DTO.Role.RoleValidators;

public class UpdateDtoValidator : AbstractValidator<RoleToAddDto>
{
    public UpdateDtoValidator()
    {
        RuleFor(p => p.Name).NotNull();
    }
}