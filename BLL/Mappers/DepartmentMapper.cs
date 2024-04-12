using AutoMapper;
using DTO.Department;
using ENTITIES.Entities.Structures;

namespace BLL.Mappers;

public class DepartmentMapper : Profile
{
    public DepartmentMapper()
    {
        CreateMap<Department, DepartmentResponseDto>();
        CreateMap<Department, DepartmentByIdResponseDto>();
        CreateMap<DepartmentCreateRequestDto, Department>();
        CreateMap<DepartmentUpdateRequestDto, Department>();
    }
}
