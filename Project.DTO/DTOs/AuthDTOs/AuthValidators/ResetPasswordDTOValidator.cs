using FluentValidation;

namespace Project.DTO.DTOs.AuthDTOs.AuthValidators
{
	public class ResetPasswordDTOValidator : AbstractValidator<ResetPasswordDTO>
	{
        public ResetPasswordDTOValidator()
        {
            RuleFor(p => p.UserId).NotNull();
            RuleFor(p => p.Password).NotNull();
            RuleFor(x => x.Age).InclusiveBetween(18, 60).WithMessage("must be 18 and 60");
            RuleFor(p => p.Password).Equal(p => p.PasswordConfirmation);
        }
	}
}

