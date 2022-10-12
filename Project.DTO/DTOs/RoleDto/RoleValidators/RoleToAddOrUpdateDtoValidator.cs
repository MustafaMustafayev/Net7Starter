using FluentValidation;

namespace Project.DTO.DTOs.RoleDto.RoleValidators;

public class RoleToAddOrUpdateDtoValidator : AbstractValidator<RoleToAddOrUpdateDto>
{
    public RoleToAddOrUpdateDtoValidator()
    {
        RuleFor(p => p.Name).NotNull();
    }
}