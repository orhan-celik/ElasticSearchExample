using ElasticSearchExample.MVC.Models;
using ElasticSearchExample.MVC.Repositories;
using ElasticSearchExample.MVC.ViewModels;

namespace ElasticSearchExample.MVC.Services
{
    public class BlogService
    {
        private readonly BlogRepository _blogRepository;

        public BlogService(BlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<List<BlogListViewModel>> GetAllAsync()
        {
            var result = await _blogRepository.GetAllAsync();
            var viewResult = result.Select(x => new BlogListViewModel
            {
                Content = x.Content,
                Title = x.Title,
                Created = x.Created.ToShortDateString() ?? "N/A", // Null kontrolü eklendi
                Tags = x.Tags != null ? string.Join(", ", x.Tags) : "No Tags" // Tags birleştirilerek dönüştürüldü
            }).ToList();
            return viewResult;
        }

        public async Task<bool> SaveAsync(BlogCreateViewModel model)
        {
            var newBlog = new Blog
            {
                Title = model.Title,
                Content = model.Content,
                Tags = model.Tags.Split(","),
                UserId = Guid.NewGuid()
            };
            var result = await _blogRepository.SaveAsync(newBlog);
            return result != null;
        }

        public async Task<List<BlogListViewModel>> SearchAsync(string searhText = "docker")
        {
            var result = await _blogRepository.SearchAsync(searhText);
            var viewResult = result.Select(x => new BlogListViewModel
            {
                Content = x.Content,
                Title = x.Title,
                Created = x.Created.ToShortDateString() ?? "N/A",
                Tags = x.Tags != null ? string.Join(", ", x.Tags) : "No Tags"
            }).ToList();
            return viewResult;
        }
    }
}
