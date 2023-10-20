using System.CodeDom.Compiler;
using Refit;
using StackExchange.Redis;

namespace BLL.External.Clients;

[GeneratedCode("Refitter", "0.8.2.0")]
public interface IPetStoreClient
{
    [Post("/store/order")]
    Task<Order> PlaceOrder([Body] Order body);

    [Get("/store/order/{orderId}")]
    Task<Order> GetOrderById(long orderId);

    [Delete("/store/order/{orderId}")]
    Task DeleteOrder(long orderId);
}