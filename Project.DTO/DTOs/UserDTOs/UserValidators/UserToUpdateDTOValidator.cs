using FluentValidation;

namespace Project.DTO.DTOs.UserDTOs.UserValidators
{
	public class UserToUpdateDTOValidator : AbstractValidator<UserToUpdateDTO>
	{
		public UserToUpdateDTOValidator()
		{
			RuleFor(p => p.Name).NotNull();
		}
	}
}

