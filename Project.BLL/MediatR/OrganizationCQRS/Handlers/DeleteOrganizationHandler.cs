using AutoMapper;
using MediatR;
using Project.BLL.MediatR.OrganizationCQRS.Commands;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.Responses;

namespace Project.BLL.MediatR.OrganizationCQRS.Handlers;

public class DeleteOrganizationHandler : IRequestHandler<DeleteOrganizationCommand, IResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrganizationHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResult> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
    {
        var data = await _unitOfWork.OrganizationRepository.GetAsync(e => e.OrganizationId == request.Id);
        _unitOfWork.OrganizationRepository.Delete(data!);

        return new SuccessResult();
    }
}