using ElasticSearchExample.MVC.ViewModels;

namespace ElasticSearchExample.MVC.Models
{
    public class BlogAdvanceSearchPageViewModel
    {
        public long TotalCount { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public long PageLinkCount { get; set; }

        public BlogAdvanceSearchViewModel? Search { get; set; }
        public List<BlogListViewModel>? List { get; set; }

        public string CreatePageUrl(HttpRequest httpRequest, int page, int pageSize)
        {
            var currentUrl = new Uri($"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.Path}{httpRequest.QueryString}").AbsoluteUri;

            if (currentUrl.Contains("page", StringComparison.OrdinalIgnoreCase))
            {
                currentUrl = currentUrl.Replace($"Page={Page}", $"Page={page}");
                currentUrl = currentUrl.Replace($"PageSize={PageSize}", $"PageSize={pageSize}");
            }
            else
            {
                // Eğer 'page' parametresi yoksa, URL'ye yeni parametreleri ekle
                if (currentUrl.Contains("?"))
                {
                    // URL'de zaten parametreler varsa '&' ile yeni parametreyi ekle
                    currentUrl = $"{currentUrl}&Page={page}&PageSize={pageSize}";
                }
                else
                {
                    // URL'de parametre yoksa '?' ile yeni parametreyi ekle
                    currentUrl = $"{currentUrl}?Page={page}&PageSize={pageSize}";
                }
            }

            return currentUrl;
        }
    }
}
