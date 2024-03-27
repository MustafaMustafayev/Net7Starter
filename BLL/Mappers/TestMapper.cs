using AutoMapper;
using DTO.Test;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class TestMapper : Profile
{
    public TestMapper()
    {
        CreateMap<Test, TestToListDto>();
        CreateMap<TestToAddDto, Test>();
        CreateMap<TestToUpdateDto, Test>();
    }
}
