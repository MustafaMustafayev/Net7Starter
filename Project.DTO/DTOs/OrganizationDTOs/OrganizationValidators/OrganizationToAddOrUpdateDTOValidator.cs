using FluentValidation;

namespace Project.DTO.DTOs.OrganizationDTOs.OrganizationValidators;

public class OrganizationToAddOrUpdateDtoValidator : AbstractValidator<OrganizationToAddOrUpdateDto>
{
    public OrganizationToAddOrUpdateDtoValidator()
    {
        RuleFor(p => p.FullName).NotNull();
        RuleFor(p => p.Tin).NotNull().Length(10);
    }
}