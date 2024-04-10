using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace MEDIATRS.OrganizationCQRS.Queries;

public record GetOrganizationListQuery : IRequest<IDataResult<List<OrganizationResponseDto>>>;