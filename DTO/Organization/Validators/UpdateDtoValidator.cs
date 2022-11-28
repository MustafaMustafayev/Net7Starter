using FluentValidation;

namespace DTO.Organization.Validators;

public class UpdateDtoValidator : AbstractValidator<OrganizationToUpdateDto>
{
    public UpdateDtoValidator()
    {
        RuleFor(p => p.OrganizationId).NotNull();
    }
}