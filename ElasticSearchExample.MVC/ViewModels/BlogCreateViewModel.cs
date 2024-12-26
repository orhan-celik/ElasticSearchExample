using System.ComponentModel.DataAnnotations;

namespace ElasticSearchExample.MVC.ViewModels
{
    public class BlogCreateViewModel
    {
        [Display(Name = "Blog Başlık")]
        [Required]
        public string Title { get; set; } = null!;

        [Display(Name = "Blog İçerik")]
        [Required]
        public string Content { get; set; } = null!;

        [Display(Name = "Blog Tagları")]
        [Required]
        public string Tags { get; set; } = null!;
    }
}
