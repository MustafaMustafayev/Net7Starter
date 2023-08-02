using System.Linq.Expressions;
using Elasticsearch.Net;
using Nest;

namespace DAL.ElasticSearch;

public class ElasticSearchService<T> : IElasticSearchService<T> where T : class
{
    private readonly ElasticClient _client;

    public ElasticSearchService(string uri, string defaultIndex, string? username = null, string? password = null)
    {
        var pool = new SingleNodeConnectionPool(new Uri(uri));

        var settings = new ConnectionSettings(pool)
            .DefaultIndex(defaultIndex)
            .DefaultMappingFor<T>(m => m.IndexName(defaultIndex))
            .BasicAuthentication(username, password);

        _client = new ElasticClient(settings);
    }

    public async Task<bool> IndexExistsAsync(string indexName)
    {
        var response = await _client.Indices.ExistsAsync(indexName);
        return response.Exists;
    }

    public async Task<bool> CreateIndexAsync(string indexName)
    {
        var createIndexResponse = await _client.Indices.CreateAsync(indexName, c => c
            .Map<T>(m => m.AutoMap())
            .Settings(s => s
                .Analysis(a => a
                    .Analyzers(aa => aa
                        .Custom("my_analyzer", ca => ca
                            .Tokenizer("standard")
                            .CharFilters("html_strip")
                            .Filters("lowercase", "asciifolding")
                        )
                    )
                )
            )
        );

        return createIndexResponse.IsValid;
    }

    public async Task<bool> AddToIndexAsync(T document)
    {
        var indexResponse = await _client.IndexDocumentAsync(document);
        return indexResponse.IsValid;
    }

    public async Task<bool> AddRangeToIndexAsync(IEnumerable<T> documents)
    {
        var indexResponse = await _client.IndexManyAsync(documents);
        if (!indexResponse.IsValid) return false;
        return indexResponse.IsValid;
    }

    public async Task<bool> DeleteIndexAsync(string indexName)
    {
        var deleteIndexResponse = await _client.Indices.DeleteAsync(indexName);
        return deleteIndexResponse.IsValid;
    }


    /// <summary>
    ///     Example: var documents = await SearchDocumentsAsync(d => d.Text, "some query");
    /// </summary>
    public async Task<IEnumerable<T>> SearchDocumentsAsync(Expression<Func<T, object>> field, string query)
    {
        var searchResponse = await _client.SearchAsync<T>(s => s
            .Query(q => q
                .Match(m => m
                    .Field(field)
                    .Query(query)
                )
            )
        );
        return searchResponse.Documents;
    }
}