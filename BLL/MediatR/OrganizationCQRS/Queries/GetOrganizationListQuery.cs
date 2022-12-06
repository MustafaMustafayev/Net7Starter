using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace BLL.MediatR.OrganizationCQRS.Queries;

public record GetOrganizationListQuery : IRequest<IDataResult<List<OrganizationToListDto>>>;