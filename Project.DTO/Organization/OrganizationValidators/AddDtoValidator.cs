using FluentValidation;

namespace Project.DTO.Organization.OrganizationValidators;

public class AddDtoValidator : AbstractValidator<OrganizationToAddDto>
{
    public AddDtoValidator()
    {
        RuleFor(p => p.FullName).NotNull();
        RuleFor(p => p.Tin).NotNull(); //.Length(10);
    }
}