using FluentValidation;

namespace Project.DTO.DTOs.OrganizationDTOs.OrganizationValidators
{
	public class OrganizationToAddOrUpdateDTOValidator : AbstractValidator<OrganizationToAddOrUpdateDTO>
	{
		public OrganizationToAddOrUpdateDTOValidator()
		{
			RuleFor(p => p.FullName).NotNull();
			RuleFor(p => p.TIN).NotNull().Length(10);
		}
	}
}