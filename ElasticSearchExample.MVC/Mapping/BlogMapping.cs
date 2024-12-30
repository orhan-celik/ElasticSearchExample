using AutoMapper;
using ElasticSearchExample.MVC.Models;
using ElasticSearchExample.MVC.ViewModels;

namespace ElasticSearchExample.MVC.Mapping
{
    public class BlogMapping : Profile
    {
        public BlogMapping()
        {
            // Blog modelini, BlogListViewModel modeline dönüştürüyoruz.
            CreateMap<Blog, BlogListViewModel>()
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created.ToShortDateString()))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => string.Join(",", src.Tags)));

            // BlogCreateViewModel modelini, Blog modeline dönüştürüyoruz.
            CreateMap<BlogCreateViewModel, Blog>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList()))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(_ => Guid.NewGuid()));
        }
    }
}
