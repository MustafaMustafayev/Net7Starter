using AutoMapper;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DTO.Responses;
using ENTITIES.Entities;
using MediatR;
using MEDIATRS.OrganizationCQRS.Commands;

namespace MEDIATRS.OrganizationCQRS.Handlers;

public class UpdateOrganizationHandler : IRequestHandler<UpdateOrganizationCommand, IResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrganizationHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResult> Handle(UpdateOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var old = await _unitOfWork.OrganizationRepository.GetAsNoTrackingAsync(u => u.Id == request.OrganizationId);
        if (old is null) return new ErrorResult(Messages.DataNotFound.Translate());

        var mapped = _mapper.Map<Organization>(request.Organization);

        mapped.Id = request.OrganizationId;
        mapped.LogoFileId = old.LogoFileId;

        _unitOfWork.OrganizationRepository.Update(mapped);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}