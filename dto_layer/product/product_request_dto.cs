using System;

namespace DTOLayer.Product
{
    public class ProductRequestDTO
    {
        public string CodigoProducto { get; set; }
        public string Descripcion { get; set; }
        public string CodigoBarra { get; set; }
        public string SubCodigoHijo { get; set; }
        public string SubCodigoPadre { get; set; }
        public string FormaVenta { get; set; }
    }
}
