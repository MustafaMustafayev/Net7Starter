using FluentValidation;

namespace Project.DTO.Organization.OrganizationValidators;

public class UpdateDtoValidator : AbstractValidator<OrganizationToUpdateDto>
{
    public UpdateDtoValidator()
    {
        RuleFor(p => p.OrganizationId).NotNull();
    }
}