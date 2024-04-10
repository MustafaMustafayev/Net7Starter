using AutoMapper;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DTO.Organization;
using DTO.Responses;
using MediatR;
using MEDIATRS.OrganizationCQRS.Queries;

namespace MEDIATRS.OrganizationCQRS.Handlers;

public class
    GetOrganizationListHandler : IRequestHandler<GetOrganizationListQuery,
        IDataResult<List<OrganizationResponseDto>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetOrganizationListHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<List<OrganizationResponseDto>>> Handle(
        GetOrganizationListQuery request,
        CancellationToken cancellationToken)
    {
        var data = await _unitOfWork.OrganizationRepository.GetListAsync();
        var result = _mapper.Map<List<OrganizationResponseDto>>(data);

        return new SuccessDataResult<List<OrganizationResponseDto>>(result, Messages.Success.Translate());
    }
}