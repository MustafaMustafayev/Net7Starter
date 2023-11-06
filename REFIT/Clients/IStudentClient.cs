// using BLL.External.Dtos;
// using Refit;

// namespace BLL.External.Clients;

// [Headers("Content-Type: application/json")]
// public partial interface IStudentClient
// {
//     [Get("/api/student")]
//     Task<IEnumerable<StudentToListDto>> GetAsync(
//         [Authorize] string token,
//         [Query] bool isActive,
//         [Header("x-mobile")] bool isMobile);

//     [Get("/api/student/{id}")]
//     Task<StudentToListDto> GetAsync(int id);

//     [Post("/api/student")]
//     Task CreateAsync([Body] StudentToAddDto dto);

//     [Put("/api/student/{id}")]
//     Task UpdateAsync(int id, [Body] StudentToUpdateDto dto);

//     [Delete("/api/student/{id}")]
//     Task DeleteAsync(int id);
// }

