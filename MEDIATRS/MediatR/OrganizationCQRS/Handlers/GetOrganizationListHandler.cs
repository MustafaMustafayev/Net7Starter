using AutoMapper;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DTO.Organization;
using DTO.Responses;
using MediatR;
using MEDIATRS.MediatR.OrganizationCQRS.Queries;

namespace MEDIATRS.MediatR.OrganizationCQRS.Handlers;

public class
    GetOrganizationListHandler : IRequestHandler<GetOrganizationListQuery,
        IDataResult<List<OrganizationToListDto>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetOrganizationListHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<List<OrganizationToListDto>>> Handle(
        GetOrganizationListQuery request,
        CancellationToken cancellationToken)
    {
        var data = await _unitOfWork.OrganizationRepository.GetListAsync();
        var result = _mapper.Map<List<OrganizationToListDto>>(data);

        return new SuccessDataResult<List<OrganizationToListDto>>(result, Messages.Success.Translate());
    }
}