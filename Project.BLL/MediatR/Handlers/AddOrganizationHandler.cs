using MapsterMapper;
using MediatR;
using Project.BLL.MediatR.Commands;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.Organization;
using Project.DTO.Responses;
using Project.Entity.Entities;

namespace Project.BLL.MediatR.Handlers;

public class AddOrganizationHandler : IRequestHandler<AddOrganizationCommand, IDataResult<OrganizationToListDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AddOrganizationHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<OrganizationToListDto>> Handle(AddOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var mapped = _mapper.Map<Organization>(request.Organization);
        var data = await _unitOfWork.OrganizationRepository.AddAsync(mapped);
        var result = _mapper.Map<OrganizationToListDto>(data);

        return new SuccessDataResult<OrganizationToListDto>(result);
    }
}