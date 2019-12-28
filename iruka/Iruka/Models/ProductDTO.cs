using System;
using System.ComponentModel.DataAnnotations;

namespace Iruka.Models
{
    public class ProductDTO : BaseEntityDto
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Link")]
        public string Link { get; set; }

        [Required]
        [Display(Name = "Picture")]
        public string Picture { get; set; }

        [Display(Name = "Schedule Date")]
        public string ScheduleDate { get; set; }
        public int Priority { get; set; }
        public string Base64URL { get; set; }
    }
}