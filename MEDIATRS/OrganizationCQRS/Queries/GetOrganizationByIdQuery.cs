using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace MEDIATRS.OrganizationCQRS.Queries;

public record GetOrganizationByIdQuery(int Id) : IRequest<IDataResult<OrganizationToListDto>>;