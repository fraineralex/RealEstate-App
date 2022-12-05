using RealEstateApp.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Properties: AuditableBaseEntity
    {
        public string Code { get; set; }
        public decimal Price { get; set; }
        public int LandSize { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public string Description { get; set; }
        public string ImagePathOne { get; set; }
        public string? ImagePathTwo { get; set; }
        public string? ImagePathThree { get; set; }
        public string? ImagePathFour { get; set; }
        public string ImprovementsId { get; set; }
        public string TypeOfPropertyId { get; set; }
        public string TypeOfSaleId { get; set; }
        public Improvements? Improvements { get; set; }
        public TypeOfProperties? TypeOfProperty { get; set; }
        public TypeOfSales? TypeOfSale { get; set; }
    }
}
