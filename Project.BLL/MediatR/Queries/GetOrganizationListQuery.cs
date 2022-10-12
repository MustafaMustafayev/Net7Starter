using MediatR;
using Project.DTO.Organization;
using Project.DTO.Responses;

namespace Project.BLL.MediatR.Queries;

public record GetOrganizationListQuery : IRequest<IDataResult<List<OrganizationToListDto>>>;