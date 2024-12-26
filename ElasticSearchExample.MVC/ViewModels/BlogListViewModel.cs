using System.ComponentModel.DataAnnotations;

namespace ElasticSearchExample.MVC.ViewModels
{
    public class BlogListViewModel
    {
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string Tags { get; set; } = null!;
        public string Created { get; set; } = null!;
    }
}
