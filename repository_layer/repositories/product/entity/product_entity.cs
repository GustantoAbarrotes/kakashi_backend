namespace RepositoryLayer.Repositories.Product.Entity
{
    public class ProductEntity
    {
        public string CodigoProducto { get; set; }
        public string CodigoBarra { get; set; }
        public string FormaVentaCB { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public string SubCodigoPadre { get; set; }
        public string SubCodigoHijo { get; set; }
    }
}
