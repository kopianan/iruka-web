﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iruka.Models
{
    public class BaseEntityDto
    {
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }
}