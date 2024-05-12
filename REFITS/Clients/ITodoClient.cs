using Refit;
using REFITS.Dtos;

namespace REFITS.Clients;

public interface IToDoClient
{
    [Get("/todo")]
    Task<List<ToDoToListDto>> Get();

    [Get("/todo/{id}")]
    Task<ToDoToListDto> Get(int id);

    [Post("/todo")]
    Task<ToDoToListDto> Create([Body] ToDoToAddDto todo);

    [Put("/todo/{id}")]
    Task<ToDoToListDto> Update(int id, [Body] ToDoToUpdateDto todo);

    [Delete("/todo/{id}")]
    Task Delete(int id);
}