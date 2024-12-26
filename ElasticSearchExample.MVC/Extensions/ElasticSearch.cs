using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace ElasticSearchExample.MVC.Extensions
{
    public static class ElasticSearch
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var elasticUserName = configuration.GetSection("ElasticSearch")["Username"]!;
            var elasticPassword = configuration.GetSection("ElasticSearch")["Password"]!;
            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("ElasticSearch")["Url"]!))
                .Authentication(new BasicAuthentication(elasticUserName, elasticPassword));

            var client = new ElasticsearchClient(settings);

            services.AddSingleton(client);
        }
    }
}
