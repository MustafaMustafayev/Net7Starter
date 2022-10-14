using MapsterMapper;
using MediatR;
using Project.BLL.MediatR.OrganizationCQRS.Commands;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.Responses;
using Project.Entity.Entities;

namespace Project.BLL.MediatR.OrganizationCQRS.Handlers;

public class UpdateOrganizationHandler : IRequestHandler<UpdateOrganizationCommand, IResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrganizationHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<IResult> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var mapped = _mapper.Map<Organization>(request.Organization);
        _unitOfWork.OrganizationRepository.Update(mapped);
        // var result = _mapper.Map<OrganizationToListDto>(data);

        return Task.FromResult<IResult>(new SuccessResult());
    }
}