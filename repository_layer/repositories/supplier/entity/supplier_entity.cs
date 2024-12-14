using System;

namespace RepositoryLayer.Repositories.Supplier.Entity
{
    public class SupplierEntity
    {
        public decimal calcFechaVenc { get; set; }
        public decimal cveProveedor { get; set; }
        public string nombre { get; set; }
        public string colonia { get; set; }
        public string telefono { get; set; }

        public string rfc { get; set; }
        public string ciudad { get; set; }
        public decimal diasCred { get; set; }
        public decimal diasEnt { get; set; }
        public string estado { get; set; }

        public string nombreComercial { get; set; }
        public string calle { get; set; }
        public string numeroExterior { get; set; }
        public string numeroInterior { get; set; }
        public string referencia { get; set; }

        public string municipio { get; set; }
        public string pais { get; set; }
        public string activo { get; set; }
        public decimal codigoPostal { get; set; }
        public string email { get; set; }

        public string nombreContacto { get; set; }
        public string telefonoContacto { get; set; }
        public string emailContacto { get; set; }

        public string creadoPor { get; set; }
        public DateTime creadoFecha { get; set; }
        public string modificadoPor { get; set; }
        public DateTime modificadoFecha { get; set; }

        public string informacionAdicional { get; set; }
        public decimal tipoProv { get; set; }

        public string nomGrupoProv { get; set; }

        public SupplierEntity Clone()
        {
            SupplierEntity objClon = new SupplierEntity();
            objClon.nombreContacto = this.nombreContacto;
            objClon.calcFechaVenc = this.calcFechaVenc;
            objClon.cveProveedor = this.cveProveedor;
            objClon.nombre = this.nombre;
            objClon.colonia = this.colonia;
            objClon.telefono = this.telefono;
            objClon.rfc = this.rfc;
            objClon.ciudad = this.ciudad;
            objClon.diasCred = this.diasCred;
            objClon.diasEnt = this.diasEnt;
            objClon.estado = this.estado;
            objClon.nombreComercial = this.nombreComercial;
            objClon.calle = this.calle;
            objClon.numeroExterior = this.numeroExterior;
            objClon.numeroInterior = this.numeroInterior;
            objClon.referencia = this.referencia;
            objClon.municipio = this.municipio;
            objClon.pais = this.pais;
            objClon.activo = this.activo;
            objClon.codigoPostal = this.codigoPostal;
            objClon.email = this.email;
            objClon.telefonoContacto = this.telefonoContacto;
            objClon.emailContacto = this.emailContacto;
            objClon.creadoPor = this.creadoPor;
            objClon.creadoFecha = this.creadoFecha;
            objClon.modificadoPor = this.modificadoPor;
            objClon.modificadoFecha = this.modificadoFecha;
            objClon.informacionAdicional = this.informacionAdicional;
            objClon.tipoProv = this.tipoProv;
            return objClon;
        }
    }

}
