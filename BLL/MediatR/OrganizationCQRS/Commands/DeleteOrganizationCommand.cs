using DTO.Responses;
using MediatR;

namespace BLL.MediatR.OrganizationCQRS.Commands;

public record DeleteOrganizationCommand(int Id) : IRequest<IResult>;