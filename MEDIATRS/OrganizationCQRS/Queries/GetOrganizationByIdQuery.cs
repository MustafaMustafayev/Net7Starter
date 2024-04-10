using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace MEDIATRS.OrganizationCQRS.Queries;

public record GetOrganizationByIdQuery(Guid Id) : IRequest<IDataResult<OrganizationResponseDto>>;