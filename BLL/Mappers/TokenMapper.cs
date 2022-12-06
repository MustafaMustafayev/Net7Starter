using AutoMapper;
using DTO.Auth;
using DTO.Token;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class TokenMapper : Profile
{
    public TokenMapper()
    {
        CreateMap<TokenToAddDto, Token>();
        CreateMap<Token, TokenToListDto>();
        CreateMap<LoginResponseDto, Token>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.UserId));
    }
}