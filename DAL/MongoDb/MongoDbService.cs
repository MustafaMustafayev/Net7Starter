using MongoDB.Driver;

namespace DAL.MongoDb;

public class MongoDbService : IMongoDbService
{
    private readonly IMongoClient _client;
    private IMongoDatabase _database;

    public MongoDbService(string connection, string database)
    {
        _client = new MongoClient(connection);
        _database = _client.GetDatabase(database);
    }

    public void ChangeDatabase(string database)
    {
        _database = _client.GetDatabase(database);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }

    public async Task<IEnumerable<T>> GetAll<T>(string collectionName)
    {
        var collection = GetCollection<T>(collectionName);
        var documents = await collection.Find(_ => true).ToListAsync();
        return documents;
    }

    public async Task<T> GetById<T>(string collectionName, string id)
    {
        var collection = GetCollection<T>(collectionName);
        var filter = Builders<T>.Filter.Eq("_id", id);
        var document = await collection.Find(filter).FirstOrDefaultAsync();
        return document;
    }

    public async Task Insert<T>(string collectionName, T document)
    {
        var collection = GetCollection<T>(collectionName);
        await collection.InsertOneAsync(document);
    }

    public async Task Update<T>(string collectionName, string id, T document)
    {
        var collection = GetCollection<T>(collectionName);
        var filter = Builders<T>.Filter.Eq("_id", id);
        await collection.ReplaceOneAsync(filter, document);
    }

    public async Task Delete<T>(string collectionName, string id)
    {
        var collection = GetCollection<T>(collectionName);
        var filter = Builders<T>.Filter.Eq("_id", id);
        await collection.DeleteOneAsync(filter);
    }
}