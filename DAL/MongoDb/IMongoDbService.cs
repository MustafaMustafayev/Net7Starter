using MongoDB.Driver;

namespace DAL.MongoDb;

public interface IMongoDbService
{
    void ChangeDatabase(string database);
    IMongoCollection<T> GetCollection<T>(string collectionName);
    Task<IEnumerable<T>> GetAll<T>(string collectionName);
    Task<T> GetById<T>(string collectionName, string id);
    Task Insert<T>(string collectionName, T document);
    Task Update<T>(string collectionName, string id, T document);
    Task Delete<T>(string collectionName, string id);
}