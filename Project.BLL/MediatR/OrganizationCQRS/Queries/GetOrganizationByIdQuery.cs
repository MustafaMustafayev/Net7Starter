using MediatR;
using Project.DTO.Organization;
using Project.DTO.Responses;

namespace Project.BLL.MediatR.OrganizationCQRS.Queries;

public record GetOrganizationByIdQuery(int Id) : IRequest<IDataResult<OrganizationToListDto>>;