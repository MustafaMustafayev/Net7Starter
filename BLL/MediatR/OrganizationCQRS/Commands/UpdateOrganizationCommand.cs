using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace BLL.MediatR.OrganizationCQRS.Commands;

public record UpdateOrganizationCommand(OrganizationToUpdateDto Organization) : IRequest<IResult>;