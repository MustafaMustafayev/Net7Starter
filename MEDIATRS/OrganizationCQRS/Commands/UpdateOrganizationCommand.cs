using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace MEDIATRS.OrganizationCQRS.Commands;

public record UpdateOrganizationCommand(Guid OrganizationId, OrganizationUpdateRequestDto Organization) : IRequest<IResult>;