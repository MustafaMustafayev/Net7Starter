using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace MEDIATRS.OrganizationCQRS.Commands;

public record UpdateOrganizationCommand(Guid OrganizationId, OrganizationToUpdateDto Organization) : IRequest<IResult>;