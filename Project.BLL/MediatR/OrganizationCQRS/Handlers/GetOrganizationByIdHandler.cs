using MapsterMapper;
using MediatR;
using Project.BLL.MediatR.OrganizationCQRS.Queries;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.Organization;
using Project.DTO.Responses;

namespace Project.BLL.MediatR.OrganizationCQRS.Handlers;

public class GetOrganizationByIdHandler : IRequestHandler<GetOrganizationByIdQuery, IDataResult<OrganizationToListDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetOrganizationByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<OrganizationToListDto>> Handle(GetOrganizationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var data = await _unitOfWork.OrganizationRepository.GetAsync(e => e.OrganizationId == request.Id);
        var result = _mapper.Map<OrganizationToListDto>(data);

        return new SuccessDataResult<OrganizationToListDto>(result);
    }
}