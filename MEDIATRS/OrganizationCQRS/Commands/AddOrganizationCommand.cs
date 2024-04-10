using DTO.Organization;
using DTO.Responses;
using MediatR;

namespace MEDIATRS.OrganizationCQRS.Commands;

public record AddOrganizationCommand
    (OrganizationCreateRequestDto Organization) : IRequest<IResult>;