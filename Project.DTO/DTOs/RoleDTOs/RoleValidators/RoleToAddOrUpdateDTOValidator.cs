using FluentValidation;

namespace Project.DTO.DTOs.RoleDTOs.RoleValidators
{
	public class RoleToAddOrUpdateDTOValidator : AbstractValidator<RoleToAddOrUpdateDTO>
	{
		public RoleToAddOrUpdateDTOValidator()
		{
			RuleFor(p => p.Name).NotNull();
		}
	}
}