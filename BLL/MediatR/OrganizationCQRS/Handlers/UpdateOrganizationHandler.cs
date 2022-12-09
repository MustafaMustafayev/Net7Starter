using AutoMapper;
using BLL.MediatR.OrganizationCQRS.Commands;
using CORE.Localization;
using DAL.UnitOfWorks.Abstract;
using DTO.Responses;
using ENTITIES.Entities;
using MediatR;

namespace BLL.MediatR.OrganizationCQRS.Handlers;

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
        var mapped = _mapper.Map<Organization>(request.Organization);
        mapped.OrganizationId = request.organizationId;

        _unitOfWork.OrganizationRepository.Update(mapped);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}