using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Models
{
    public class BaseEntity
    {
        public DateTime CreationTime { get; set; }
        public string CreationUser { get; set; }
        public DateTime? ModificationTime { get; set; }
        public string ModificationUser { get; set; }
    }
}
