using FluentValidation;

namespace Project.DTO.DTOs.AuthDTOs.AuthValidators
{
	public class LoginDTOValidator : AbstractValidator<LoginDTO>
	{
		public LoginDTOValidator()
		{
			RuleFor(p => p.PIN).NotNull().Length(7);
			RuleFor(p => p.Password).NotNull();
		}
	}
}