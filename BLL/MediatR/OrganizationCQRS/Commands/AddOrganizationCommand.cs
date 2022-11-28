using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace BLL.MediatR.OrganizationCQRS.Commands;

public record AddOrganizationCommand
    (OrganizationToAddDto Organization) : IRequest<IResult>;