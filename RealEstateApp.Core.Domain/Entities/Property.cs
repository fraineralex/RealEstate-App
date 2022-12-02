using RealEstateApp.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Property: AuditableBaseEntity
    {
        public int Code { get; set; }
        public decimal Price { get; set; }
        public int LandSize { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public string Description { get; set; }
        public string AgentName { get; set; }
        public int AgentId { get; set; }
        public ICollection<Improvement> Improvements { get; set; }
        public TypeOfPropery TypeOfPropery { get; set; }
        public TypeOfSale TypeOfSale { get; set; }
    }
}
