﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iruka.EF.Model
{
    public class Product : BaseEntity<Guid>
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Picture { get; set; }
        public bool isActive { get; set; }
        public int Priority { get; set; }
        public DateTime ScheduleDate { get; set; }
    }
}
