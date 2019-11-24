using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iruka.EF.Model
{
    public class Transaction : BaseEntity<Guid>
    {
        public string ServiceType { get; set; }
        public string Description { get; set; }
        public int Point { get; set; }
    }
}
