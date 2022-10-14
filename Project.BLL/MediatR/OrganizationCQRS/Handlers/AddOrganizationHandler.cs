using MapsterMapper;
using MediatR;
using Project.BLL.MediatR.OrganizationCQRS.Commands;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.Responses;
using Project.Entity.Entities;

namespace Project.BLL.MediatR.OrganizationCQRS.Handlers;

public class AddOrganizationHandler : IRequestHandler<AddOrganizationCommand, IResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AddOrganizationHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResult> Handle(AddOrganizationCommand request, CancellationToken cancellationToken)
    {
        var mapped = _mapper.Map<Organization>(request.Organization);
        await _unitOfWork.OrganizationRepository.AddAsync(mapped);
        // var result = _mapper.Map<OrganizationToListDto>(data);

        return new SuccessResult();
    }
}