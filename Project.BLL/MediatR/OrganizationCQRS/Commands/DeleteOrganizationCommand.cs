using MediatR;
using Project.DTO.Responses;

namespace Project.BLL.MediatR.OrganizationCQRS.Commands;

public record DeleteOrganizationCommand(int Id) : IRequest<IResult>;