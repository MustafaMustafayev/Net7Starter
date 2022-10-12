using MediatR;
using Project.DTO.Organization;
using Project.DTO.Responses;

namespace Project.BLL.MediatR.Commands;

public record AddOrganizationCommand
    (OrganizationToAddOrUpdateDto Organization) : IRequest<IDataResult<OrganizationToListDto>>;