using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearchExample.MVC.Models;

namespace ElasticSearchExample.MVC.Repositories
{
    public class BlogRepository
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        private const string IndexName = "blog";

        public BlogRepository(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        public async Task<List<Blog>> GetAllAsync()
        {
            // MatchAll sorgusunu oluştur ve ElasticSearch'e gönder
            var response = await _elasticsearchClient.SearchAsync<Blog>(s => s
                .Index(IndexName)
                .Query(q => q.MatchAll(m => new MatchAllQuery()))
            );

            // Sonuçları kontrol et ve listede döndür
            if (!response.IsValidResponse || response.Hits == null)
            {
                return new List<Blog>();
            }

            return response.Hits.Select(hit => hit.Source).ToList();
        }

        public async Task<Blog?> SaveAsync(Blog newBlog)
        {
            newBlog.Created = DateTime.Now;
            var response = await _elasticsearchClient.IndexAsync(newBlog, x => x.Index(IndexName));
            if (!response.IsValidResponse) return null;
            newBlog.Id = response.Id;
            return newBlog;

        }
    }
}
