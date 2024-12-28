using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ElasticSearchExample.MVC.Models
{
    public class BlogAdvanceSearchViewModel
    {
        [Display(Name = "Numara")]
        public string? Id { get; set; }

        [Display(Name = "Başlık")]
        public string? Title { get; set; }

        [Display(Name = "İçerik")]
        public string? Content { get; set; }

        [Display(Name = "Oluşturulma Tarihi Başlangıç")]
        [DataType(DataType.Date)]
        public DateTime? CreatedStart { get; set; }

        [Display(Name = "Oluşturulma Tarihi Bitiş")]
        [DataType(DataType.Date)]
        public DateTime? CreatedEnd { get; set; }
    }
}
