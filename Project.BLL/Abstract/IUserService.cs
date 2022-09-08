using Project.DTO.DTOs.AuthDTOs;
using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.UserDTOs;

namespace Project.BLL.Abstract;

public interface IUserService
{
    Task<IDataResult<List<UserToListDTO>>> GetAsync();

    Task<IDataResult<UserToListDTO>> GetAsync(int id);

    Task<IDataResult<Result>> AddAsync(UserToAddDTO dto);

    Task<IDataResult<Result>> UpdateAsync(int id, UserToUpdateDTO dto);

    Task DeleteAsync(int id);

    Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDto);
}