﻿using AutoMapper;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DTO.Organization;
using DTO.Responses;
using MediatR;
using MEDIATRS.OrganizationCQRS.Queries;

namespace MEDIATRS.OrganizationCQRS.Handlers;

public class GetOrganizationByIdHandler : IRequestHandler<GetOrganizationByIdQuery,
    IDataResult<OrganizationResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetOrganizationByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<OrganizationResponseDto>> Handle(GetOrganizationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var data =
            await _unitOfWork.OrganizationRepository.GetAsync(e => e.Id == request.Id);
        var result = _mapper.Map<OrganizationResponseDto>(data);

        return new SuccessDataResult<OrganizationResponseDto>(result, Messages.Success.Translate());
    }
}