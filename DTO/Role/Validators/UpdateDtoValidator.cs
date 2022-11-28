using FluentValidation;

namespace DTO.Role.Validators;

public class UpdateDtoValidator : AbstractValidator<RoleToAddDto>
{
    public UpdateDtoValidator()
    {
        RuleFor(p => p.Name).NotNull();
    }
}