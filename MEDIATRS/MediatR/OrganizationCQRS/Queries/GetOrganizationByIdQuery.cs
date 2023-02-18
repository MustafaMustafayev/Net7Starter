using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace MEDIATRS.MediatR.OrganizationCQRS.Queries;

public record GetOrganizationByIdQuery(int Id) : IRequest<IDataResult<OrganizationToListDto>>;