using DTOLayer.Product;
using RepositoryLayer.Repositories.Product.Entity;
using System;

namespace ServiceLayer.Product.Mappers
{
    public static class BarcodeMapper
    {
        public static BarcodeEntity ToEntity(this BarcodeCreateRequestDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new BarcodeEntity
            {
                CodigoProducto = dto.CodigoProducto,
                CodigoBarra = dto.CodigoBarra,
                SubCodigoHijo = dto.SubCodigoHijo,
                SubCodigoPadre = dto.SubCodigoPadre,
                FormaVentaCB = dto.FormaVenta,
                CreadoPor = dto.CreadoPor,
                ModificadoPor = dto.ModificadoPor
            };
        }
    }
}
