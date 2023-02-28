using System.Linq.Expressions;

namespace CORE.ElasticSearch;

public interface IElasticSearchService<TIndexModel> where TIndexModel : class
{
    Task<bool> IndexExistsAsync(string indexName);
    Task<bool> CreateIndexAsync(string indexName);
    Task<bool> DeleteIndexAsync(string indexName);
    Task<bool> AddToIndexAsync(TIndexModel document);
    Task<bool> AddRangeToIndexAsync(IEnumerable<TIndexModel> documents);
    Task<IEnumerable<TIndexModel>> SearchDocumentsAsync(Expression<Func<TIndexModel, object>> field, string query);
}