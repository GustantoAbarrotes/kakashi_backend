using System;

namespace DTOLayer.Supplier
{
    public class SupplierRequestDTO
    {
        public decimal CalcFechaVenc { get; set; }
        public decimal CveProveedor { get; set; }
        public string Nombre { get; set; }
        public string Colonia { get; set; }
        public string Telefono { get; set; }
        public string Rfc { get; set; }
        public string Ciudad { get; set; }
        public decimal DiasCred { get; set; }
        public decimal DiasEnt { get; set; }
        public string Estado { get; set; }
        public string NombreComercial { get; set; }
        public string Calle { get; set; }
        public string NumeroExterior { get; set; }
        public string NumeroInterior { get; set; }
        public string Referencia { get; set; }
        public string Municipio { get; set; }
        public string Pais { get; set; }
        public string Activo { get; set; }
        public decimal CodigoPostal { get; set; }
        public string Email { get; set; }
        public string NombreContacto { get; set; }
        public string TelefonoContacto { get; set; }
        public string EmailContacto { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoFecha { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime ModificadoFecha { get; set; }
        public string InformacionAdicional { get; set; }
        public decimal TipoProv { get; set; }
        public string NomGrupoProv { get; set; }
    }
}