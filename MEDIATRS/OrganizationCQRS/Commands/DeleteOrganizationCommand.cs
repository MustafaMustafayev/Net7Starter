using DTO.Responses;
using MediatR;

namespace MEDIATRS.OrganizationCQRS.Commands;

public record DeleteOrganizationCommand(int Id) : IRequest<IResult>;