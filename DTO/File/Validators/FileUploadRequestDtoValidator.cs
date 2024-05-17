using CORE.Enums;
using FluentValidation;

namespace DTO.File.Validators;

public class FileUploadRequestDtoValidator : AbstractValidator<FileUploadRequestDto>
{
    public FileUploadRequestDtoValidator()
    {
        RuleFor(p => p.File).NotNull().Must(p => p.Length < 5242880); // ~ 5 mb
        RuleFor(p => p.Type).NotNull().NotEmpty();
        RuleFor(p => p.UserId).NotNull().When(p => p.Type == EFileType.UserImages);
        RuleFor(p => p.OrganizationId).NotNull().When(p => p.Type == EFileType.OrganizationLogo);
    }
}