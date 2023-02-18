using AutoMapper;
using CORE.Localization;
using DAL.UnitOfWorks.Abstract;
using DTO.Responses;
using MediatR;
using MEDIATRS.MediatR.OrganizationCQRS.Commands;

namespace MEDIATRS.MediatR.OrganizationCQRS.Handlers;

public class DeleteOrganizationHandler : IRequestHandler<DeleteOrganizationCommand, IResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrganizationHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResult> Handle(DeleteOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var data =
            await _unitOfWork.OrganizationRepository.GetAsync(e => e.OrganizationId == request.Id);
        _unitOfWork.OrganizationRepository.SoftDelete(data!);

        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}