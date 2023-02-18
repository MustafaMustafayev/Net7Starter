using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace MEDIATRS.MediatR.OrganizationCQRS.Commands;

public record AddOrganizationCommand
    (OrganizationToAddDto Organization) : IRequest<IResult>;