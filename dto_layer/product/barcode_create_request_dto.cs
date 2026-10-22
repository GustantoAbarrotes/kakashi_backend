using System;
using System.ComponentModel.DataAnnotations;

namespace DTOLayer.Product
{
    public class BarcodeCreateRequestDTO
    {
        [Required]
        public string CodigoProducto { get; set; }
        [Required]
        public string CodigoBarra { get; set; }
        [Required]
        public string SubCodigoHijo { get; set; }
        [Required]
        public string SubCodigoPadre { get; set; }
        [Required]
        [RegularExpression("^[WC]$", ErrorMessage = "FormaVenta debe ser 'W' o 'C'.")]
        public string FormaVenta { get; set; }   // "W" o "C"
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
    }
}
