using DTOLayer.Supplier;
using RepositoryLayer.Repositories.Supplier.Entity;
using System;

namespace ServiceLayer.Supplier.Mappers
{
    public static class SupplierMapper
    {
        public static SupplierDTO ToDTO(this SupplierEntity entity) => new SupplierDTO()
        {
            Nombre = entity?.nombre,
            Email = entity?.email,
            Colonia = entity.colonia,
            Telefono = entity.telefono,
            Rfc = entity.rfc,
            Activo = entity.activo,
            CodigoPostal = entity.codigoPostal
        };

        public static SupplierEntity ToEntity(this SupplierRequestDTO entity) => new SupplierEntity()
        {
            calcFechaVenc = entity.CalcFechaVenc,
            cveProveedor = entity.CveProveedor,
            nombre = entity.Nombre,
            colonia = entity.Colonia,
            telefono = entity.Telefono,
            rfc = entity.Rfc,
            ciudad = entity.Ciudad,
            diasCred = entity.DiasCred,
            diasEnt = entity.DiasEnt,
            estado = entity.Estado,
            nombreComercial = entity.NombreComercial,
            calle = entity.Calle,
            numeroExterior = entity.NumeroExterior,
            numeroInterior = entity.NumeroInterior,
            referencia = entity.Referencia,
            municipio = entity.Municipio,
            pais = entity.Pais,
            activo = entity.Activo,
            codigoPostal = entity.CodigoPostal,
            email = entity.Email,
            nombreContacto = entity.NombreContacto,
            telefonoContacto = entity.TelefonoContacto,
            emailContacto = entity.EmailContacto,
            creadoPor = entity.CreadoPor,
            creadoFecha = entity.CreadoFecha,
            modificadoPor = entity.ModificadoPor,
            modificadoFecha = entity.ModificadoFecha,
            informacionAdicional = entity.InformacionAdicional,
            tipoProv = entity.TipoProv,
            nomGrupoProv = entity.NomGrupoProv
        };
    }
}
