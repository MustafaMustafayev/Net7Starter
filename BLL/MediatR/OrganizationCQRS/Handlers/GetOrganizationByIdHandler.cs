using AutoMapper;
using BLL.MediatR.OrganizationCQRS.Queries;
using CORE.Localization;
using DAL.UnitOfWorks.Abstract;
using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace BLL.MediatR.OrganizationCQRS.Handlers;

public class GetOrganizationByIdHandler : IRequestHandler<GetOrganizationByIdQuery,
    IDataResult<OrganizationToListDto>>
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
        var data =
            await _unitOfWork.OrganizationRepository.GetAsync(e => e.OrganizationId == request.Id);
        var result = _mapper.Map<OrganizationToListDto>(data);

        return new SuccessDataResult<OrganizationToListDto>(result, Messages.Success.Translate());
    }
}