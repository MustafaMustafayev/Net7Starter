using Project.DTO.DTOs.AuthDTOs;
using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.UserDTOs;

namespace Project.BLL.Abstract;

public interface IAuthService
{
    Task<string> GetUserSaltAsync(string pin);

    Task<IDataResult<UserToListDTO>> LoginAsync(LoginDTO loginDto);
}