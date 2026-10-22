using System.ComponentModel.DataAnnotations;

namespace DTOLayer.Product
{
    public class ProductResponseDTO
    {
        [Required]
        public string CodigoProducto { get; set; } = string.Empty;
        [Required]
        public string SubcodigoPadre { get; set; } = string.Empty;
        [Required]
        public string SubCodigoHijo { get; set; } = string.Empty;
        [Required]
        public string CodigoBarra { get; set; } = string.Empty;
        [Required]
        public string FormaVenta { get; set; } = string.Empty;
        [Required]
        public string Descripcion { get; set; } = string.Empty;
        [Required]
        public double Precio { get; set; }
    }
}
