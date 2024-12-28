using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearchExample.MVC.Models;
using System.Drawing.Printing;

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

            foreach (var hit in response.Hits) hit.Source.Id = hit.Id;

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

        public async Task<List<Blog>> SearchAsync(string serchText)
        {
            var searchResult = await _elasticsearchClient.SearchAsync<Blog>(s => s
                .Index(IndexName)
                .Size(1000)
                .Query(q => q
                    .Bool(b => b
                        .Should(
                            s => s.Match(m => m.Field(f => f.Content).Query(serchText).Fuzziness(new Fuzziness(2))),
                            s => s.MatchBoolPrefix(p => p.Field(f => f.Title).Query(serchText).Fuzziness(new Fuzziness(2)))
                        )
                    )
                )
            );

            foreach (var hit in searchResult.Hits) hit.Source.Id = hit.Id;
            return searchResult.Documents.ToList();

        }

        public async Task<Blog?> GetById(string id)
        {
            var response = await _elasticsearchClient.GetAsync<Blog>(id, x => x.Index(IndexName));
            if (!response.Found || !response.IsValidResponse) return null;
            response.Source.Id = response.Id;
            return response.Source;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var response = await _elasticsearchClient.DeleteAsync<Blog>(id, x => x.Index(IndexName));
            return response.IsSuccess();
        }

        public async Task<(List<Blog> list, long totalCount)> AdvanceSearchAsync(BlogAdvanceSearchViewModel searchModel, int page, int pageSize)
        {
            List<Action<QueryDescriptor<Blog>>> listQuery = new();

            // Search Model Boş Gelirse
            if (searchModel is null)
            {
                Action<QueryDescriptor<Blog>> query = (q) => q.MatchAll(m => new MatchAllQuery());
                listQuery.Add(query);
                return await SearchResultDate(listQuery, page, pageSize);
            }

            // Id alanı dolu ise
            if (!String.IsNullOrEmpty(searchModel.Id))
            {
                Action<QueryDescriptor<Blog>> query = (q) => q.Term(t => t.Field(f => f.Id).Value(searchModel.Id));
                listQuery.Add(query);
            }

            // Title Alanı Dolu ise
            if (!String.IsNullOrEmpty(searchModel.Title))
            {
                Action<QueryDescriptor<Blog>> query = (q) => q
                    .Match(m => m
                    .Field(f => f.Title)
                    .Query(searchModel.Title)
                    .Fuzziness(new Fuzziness(2)));
                listQuery.Add(query);
            }

            // Content Alanı Dolu ise
            if (!String.IsNullOrEmpty(searchModel.Content))
            {
                Action<QueryDescriptor<Blog>> query = (q) => q
                    .Match(m => m
                    .Field(f => f.Content)
                    .Query(searchModel.Content)
                    .Fuzziness(new Fuzziness(2)));
                listQuery.Add(query);
            }

            // Oluşturulma tarihi başlangıcı dolu ise
            if (searchModel.CreatedStart.HasValue)
            {

                // Tarihi UTC formatına çevirmek için DateTime'ı UTC'ye dönüştürüyoruz
                string formattedDate = searchModel.CreatedStart.Value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");

                Action<QueryDescriptor<Blog>> query = (q) => q
                .Range(r => r
                    .DateRange(dr => dr
                        .Field(f => f.Created)
                        .Gte(formattedDate)
                    )
                );
                listQuery.Add(query);
            }

            // Oluşturulma tarihi bitişi dolu ise
            if (searchModel.CreatedEnd.HasValue)
            {

                // Tarihi UTC formatına çevirmek için DateTime'ı UTC'ye dönüştürüyoruz
                string formattedDate = searchModel.CreatedEnd.Value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");

                Action<QueryDescriptor<Blog>> query = (q) => q
                .Range(r => r
                    .DateRange(dr => dr
                        .Field(f => f.Created)
                        .Lte(formattedDate)
                    )
                );
                listQuery.Add(query);
            }

            // listQuery içerisinde hiç bir şey yok ise
            if (!listQuery.Any())
            {
                Action<QueryDescriptor<Blog>> query = (q) => q.MatchAll(m => new MatchAllQuery());
                listQuery.Add(query);
            }

            return await SearchResultDate(listQuery, page, pageSize);
        }

        private async Task<(List<Blog> list, long totalCount)> SearchResultDate(List<Action<QueryDescriptor<Blog>>> listQuery, int page, int pageSize)
        {
            // Sayfalama kaçtan başlayacak
            var pageFrom = (page - 1) * pageSize;

            var result = await _elasticsearchClient.SearchAsync<Blog>(s => s
                .Index(IndexName)
            .From(pageFrom)
                .Size(pageSize)
                .Query(q => q
                    .Bool(b => b
                        .Must(listQuery.ToArray()))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return (list: result.Documents.ToList(), totalCount: result.Total);
        }
    }
}
