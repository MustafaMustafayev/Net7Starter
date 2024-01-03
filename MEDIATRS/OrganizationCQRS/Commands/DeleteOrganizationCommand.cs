using DTO.Responses;
using MediatR;

namespace MEDIATRS.OrganizationCQRS.Commands;

public record DeleteOrganizationCommand(Guid Id) : IRequest<IResult>;