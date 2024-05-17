using CORE.Enums;
using FluentValidation;

namespace DTO.File.Validators;

public class FileRemoveRequestDtoValidator : AbstractValidator<FileDeleteRequestDto>
{
    public FileRemoveRequestDtoValidator()
    {
        RuleFor(p => p.HashName).NotNull().NotEmpty();
        RuleFor(p => p.Type).NotNull().NotEmpty();
        RuleFor(p => p.UserId).NotNull().When(p => p.Type == EFileType.UserImages);
        RuleFor(p => p.OrganizationId).NotNull().When(p => p.Type == EFileType.OrganizationLogo);
    }
}