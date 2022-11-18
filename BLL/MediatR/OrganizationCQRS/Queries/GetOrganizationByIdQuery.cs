using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace BLL.MediatR.OrganizationCQRS.Queries;

public record GetOrganizationByIdQuery(int Id) : IRequest<IDataResult<OrganizationToListDto>>;