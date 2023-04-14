
using AutoMapper;
using DTO.ITMIM;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class ITMIMMapper : Profile
{
    public ITMIMMapper()
    {
        CreateMap<ITMIM, ITMIMToListDto>();
        CreateMap<ITMIMToAddDto, ITMIM>();
        CreateMap<ITMIMToUpdateDto, ITMIM>();
    }
}
