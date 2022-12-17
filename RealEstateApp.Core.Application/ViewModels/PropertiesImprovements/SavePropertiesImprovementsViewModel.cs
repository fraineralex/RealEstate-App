using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.PropertiesImprovements
{
    public class SavePropertiesImprovementsViewModel
    {
        public int PropertyId { get; set; }
        public int ImprovementId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
