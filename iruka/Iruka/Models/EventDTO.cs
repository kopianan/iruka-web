using System;
using System.ComponentModel.DataAnnotations;

namespace Iruka.Models
{
    public class EventDTO : BaseEntityDto
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Link")]
        public string Link { get; set; }

        [Required]
        [Display(Name = "Picture")]
        public string Picture { get; set; }

        [Required]
        [Display(Name = "Schedule Date")]
        public string ScheduleDate { get; set; }
        public int Priority { get; set; }

        public string Base64URL { get; set; }
    }
}