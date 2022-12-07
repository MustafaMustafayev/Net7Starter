using AutoMapper;
using BLL.MediatR.OrganizationCQRS.Commands;
using DAL.UnitOfWorks.Abstract;
using DTO.Responses;
using ENTITIES.Entities;
using MediatR;

namespace BLL.MediatR.OrganizationCQRS.Handlers;

public class AddOrganizationHandler : IRequestHandler<AddOrganizationCommand, IResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AddOrganizationHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResult> Handle(AddOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var mapped = _mapper.Map<Organization>(request.Organization);
        await _unitOfWork.OrganizationRepository.AddAsync(mapped);

        return new SuccessResult();
    }
}