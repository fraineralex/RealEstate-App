using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Domain.Entities
{
    public class PropertiesImprovements
    {
        public int PropertyId { get; set; }
        public int ImprovementId { get; set; }

        public Properties? Property { get; set; }
        public Improvements? Improvement { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
