using MapsterMapper;
using MediatR;
using Project.BLL.MediatR.OrganizationCQRS.Queries;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.Organization;
using Project.DTO.Responses;

namespace Project.BLL.MediatR.OrganizationCQRS.Handlers;

public class
    GetOrganizationListHandler : IRequestHandler<GetOrganizationListQuery, IDataResult<List<OrganizationToListDto>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetOrganizationListHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<List<OrganizationToListDto>>> Handle(GetOrganizationListQuery request,
        CancellationToken cancellationToken)
    {
        var data = await _unitOfWork.OrganizationRepository.GetListAsync();
        var result = _mapper.Map<List<OrganizationToListDto>>(data);

        return new SuccessDataResult<List<OrganizationToListDto>>(result);
    }
}