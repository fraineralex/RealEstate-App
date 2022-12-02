using RealEstateApp.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Domain.Entities
{
    public class TypeOfPropery: AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int PropertyId { get; set; }
        public Property MyProperty { get; set; }
    }
}
