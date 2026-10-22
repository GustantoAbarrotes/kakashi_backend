namespace RepositoryLayer.Repositories.Product.Entity
{

    public class BarcodeEntity
    {
        public string CodigoProducto { get; set; }
        public string CodigoBarra { get; set; }
        public string FormaVentaCB { get; set; }
        public string SubCodigoPadre { get; set; }
        public string SubCodigoHijo { get; set; }
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
        public System.DateTime CreadoFecha { get; set; }
        public System.DateTime ModificadoFecha { get; set; }
    }
}
