using AutoMapper;
using BLL.MediatR.OrganizationCQRS.Queries;
using CORE.Localization;
using DAL.UnitOfWorks.Abstract;
using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace BLL.MediatR.OrganizationCQRS.Handlers;

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