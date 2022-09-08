using FluentValidation;

namespace Project.DTO.DTOs.UserDTOs.UserValidators
{
	public class UserToAddDTOValidator : AbstractValidator<UserToAddDTO>
	{
		public UserToAddDTOValidator()
		{
			RuleFor(p => p.Name).NotNull();
			RuleFor(p => p.Password).NotNull();
			RuleFor(p => p.Password).Equal(p => p.PasswordConfirmation);
		}
	}
}