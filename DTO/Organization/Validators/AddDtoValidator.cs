using FluentValidation;

namespace DTO.Organization.Validators;

public class AddDtoValidator : AbstractValidator<OrganizationToAddDto>
{
    public AddDtoValidator()
    {
        RuleFor(p => p.FullName).NotNull();
        RuleFor(p => p.Tin).NotNull(); //.Length(10);
    }
}