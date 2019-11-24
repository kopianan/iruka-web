using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iruka.EF.Model
{
    public class Event : BaseEntity<Guid>
    {
        public string EventName { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Picture { get; set; }
        public int? Priority { get; set; }
        public DateTime ScheduleDate { get; set; }
        public EventStatus EventStatus { get; set; }
    }

    public enum EventStatus
    {
        Pending,
        OnGoing,
        Finished
    }
}
