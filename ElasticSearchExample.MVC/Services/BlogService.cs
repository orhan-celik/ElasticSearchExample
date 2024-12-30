using AutoMapper;
using ElasticSearchExample.MVC.Models;
using ElasticSearchExample.MVC.Repositories;
using ElasticSearchExample.MVC.ViewModels;

namespace ElasticSearchExample.MVC.Services
{
    public class BlogService
    {
        private readonly BlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public BlogService(BlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<List<BlogListViewModel>> GetAllAsync()
        {
            var result = await _blogRepository.GetAllAsync();
            var blogList = result.Select(blog => _mapper.Map<BlogListViewModel>(blog)).ToList();
            return blogList;
        }

        public async Task<bool> SaveAsync(BlogCreateViewModel model)
        {
            var newBlog = _mapper.Map<Blog>(model);
            var result = await _blogRepository.SaveAsync(newBlog);
            return result != null;
        }

        public async Task<List<BlogListViewModel>> SearchAsync(string searhText)
        {
            var result = await _blogRepository.SearchAsync(searhText);
            var viewResult = result.Select(x => _mapper.Map<BlogListViewModel>(x)).ToList();
            return viewResult;
        }

        public async Task<BlogListViewModel> GetById(string id)
        {
            var result = await _blogRepository.GetById(id);
            if (result == null) return null;
            var blogResult = _mapper.Map<BlogListViewModel>(result);
            return blogResult;
        }

        public async Task<bool?> DeleteAsync(string id)
        {
            var blog = await _blogRepository.GetById(id);
            if (blog is null) return null;
            return await _blogRepository.DeleteAsync(id);
        }

        public async Task<(List<BlogListViewModel> list, long totalCount, long pageLinkCount)> AdvanceSearchAsync(BlogAdvanceSearchViewModel searchModel, int page, int pageSize)
        {
            var (list, totalCount) = await _blogRepository.AdvanceSearchAsync(searchModel, page, pageSize);
            var pageLinkCount = (totalCount % pageSize) == 0 ? totalCount / pageSize : (totalCount / pageSize) + 1;
            var blogList = list.Select(blog => _mapper.Map<BlogListViewModel>(blog)).ToList();
            return (list: blogList, totalCount, pageLinkCount);
        }
    }
}