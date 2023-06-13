using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DTO.Responses;
using MediatR;
using MEDIATRS.MediatR.OrganizationCQRS.Commands;

namespace MEDIATRS.MediatR.OrganizationCQRS.Handlers;

public class DeleteOrganizationHandler : IRequestHandler<DeleteOrganizationCommand, IResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrganizationHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(DeleteOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var data =
            await _unitOfWork.OrganizationRepository.GetAsync(e => e.Id == request.Id);
        _unitOfWork.OrganizationRepository.SoftDelete(data!);

        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}