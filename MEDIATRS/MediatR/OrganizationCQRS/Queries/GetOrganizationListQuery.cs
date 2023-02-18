using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace MEDIATRS.MediatR.OrganizationCQRS.Queries;

public record GetOrganizationListQuery : IRequest<IDataResult<List<OrganizationToListDto>>>;