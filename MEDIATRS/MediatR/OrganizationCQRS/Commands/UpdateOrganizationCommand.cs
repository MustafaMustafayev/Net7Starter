using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace MEDIATRS.MediatR.OrganizationCQRS.Commands;

public record UpdateOrganizationCommand(int OrganizationId, OrganizationToUpdateDto Organization) : IRequest<IResult>;