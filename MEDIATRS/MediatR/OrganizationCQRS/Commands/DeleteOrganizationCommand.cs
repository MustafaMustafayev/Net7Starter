using DTO.Responses;
using MediatR;

namespace MEDIATRS.MediatR.OrganizationCQRS.Commands;

public record DeleteOrganizationCommand(int Id) : IRequest<IResult>;