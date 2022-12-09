using FluentValidation;

namespace DTO.Organization.Validators;

public class UpdateDtoValidator : AbstractValidator<OrganizationToUpdateDto>
{
    public UpdateDtoValidator()
    {
        RuleFor(p => p.ShortName).NotNull();
        RuleFor(p => p.FullName).NotNull();
        RuleFor(p => p.Address).NotNull();
        RuleFor(p => p.Email).NotNull().EmailAddress();
        RuleFor(p => p.Rekvizit).NotNull();
        RuleFor(p => p.Tin).NotNull();
        RuleFor(p => p.PhoneNumber).NotNull();
    }
}