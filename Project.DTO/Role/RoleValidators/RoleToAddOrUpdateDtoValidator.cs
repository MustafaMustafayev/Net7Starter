using FluentValidation;

namespace Project.DTO.Role.RoleValidators;

public class RoleToAddOrUpdateDtoValidator : AbstractValidator<RoleToAddOrUpdateDto>
{
    public RoleToAddOrUpdateDtoValidator()
    {
        RuleFor(p => p.Name).NotNull();
    }
}