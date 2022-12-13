using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Properties.Commands.CreateProperties
{
    public class CreatePropertiesCommand
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El codigo es requerido.")]
        public string Code { get; set; }
        [Required(ErrorMessage = "El precio es requerido.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "El Tamaño del terreno en metro es requerido.")]
        public int LandSize { get; set; }
        [Required(ErrorMessage = "La Cantidad de habitaciones es requerida.")]
        public int NumberOfRooms { get; set; }
        [Required(ErrorMessage = "La Cantidad de baños es requerida.")]
        public int NumberOfBathrooms { get; set; }
        [Required(ErrorMessage = "La Descripción es requerida.")]
        public string Description { get; set; }
        public string ImagePathOne { get; set; }
        public string? ImagePathTwo { get; set; }
        public string? ImagePathThree { get; set; }
        public string? ImagePathFour { get; set; }
        [Required(ErrorMessage = "El listado de mejoras es requerido.")]
        public List<int> Improvements { get; set; }
        [Required(ErrorMessage = "El tipo de propiedad es requerida.")]
        public int TypeOfPropertyId { get; set; }
        [Required(ErrorMessage = "El tipo de venta es requerida.")]
        public int TypeOfSaleId { get; set; }
    }
}
