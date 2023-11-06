using Refit;
using REFITS.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REFITS.Clients
{
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
}
