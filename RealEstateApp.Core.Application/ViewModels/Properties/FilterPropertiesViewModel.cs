using RealEstateApp.Core.Application.ViewModels.Improvements;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperties;
using RealEstateApp.Core.Application.ViewModels.TypeOfSales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.Properties
{
    public class FilterPropertiesViewModel
    {
        public string? Code { get; set; }
        public List<int>? Ids { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
    }
}
