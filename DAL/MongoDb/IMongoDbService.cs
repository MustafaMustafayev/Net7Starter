using MongoDB.Driver;
using System.Linq.Expressions;

namespace DAL.MongoDb;

public interface IMongoDbService
{
    void ChangeDatabase(string database);
    IMongoCollection<T> GetCollection<T>(string collectionName);
    Task<IEnumerable<T>> GetAll<T>(string collectionName);
    Task<T> GetById<T>(string collectionName, string id);
    Task<IEnumerable<T>> GetByFilter<T>(string collectionName, Expression<Func<T, bool>> filterExpression);
    Task Insert<T>(string collectionName, T document);
    Task Update<T>(string collectionName, string id, T document);
    Task Delete<T>(string collectionName, string id);
}