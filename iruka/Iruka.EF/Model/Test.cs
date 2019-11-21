using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iruka.EF.Model
{
    public class Test : BaseEntity<Guid>
    {
        public string Value { get; set; }
    }
}
