using FluentValidation;

namespace Project.DTO.DTOs.RoleDTOs.RoleValidators;

public class RoleToAddOrUpdateDtoValidator : AbstractValidator<RoleToAddOrUpdateDto>
{
    public RoleToAddOrUpdateDtoValidator()
    {
        RuleFor(p => p.Name).NotNull();
    }
}