using BLL.External.Dtos;
using RestEase;

namespace BLL.External.Clients;

[Header("Content-Type", "application/json")]
public interface IStudentClient
{
    [Get("/api/student")]
    Task<IEnumerable<StudentToListDto>> GetAsync(
        [Header("Authorization")] string token,
        [Query] bool isActive);

    [Get("/api/student/{id}")]
    Task<StudentToListDto> GetAsync([Path] int id);

    [Post("/api/student")]
    Task CreateAsync([Body] StudentToAddDto dto);

    [Put("/api/student/{id}")]
    Task UpdateAsync([Path] int id, [Body] StudentToUpdateDto dto);

    [Delete("/api/student/{id}")]
    Task DeleteAsync([Path] int id);
}