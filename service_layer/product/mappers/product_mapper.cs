using DTOLayer.Product;
using RepositoryLayer.Repositories.Product.Entity;
using System;

namespace ServiceLayer.Product.Mappers
{
    public static class ProductMapper
    {
        public static ProductResponseDTO ToResponseDTO(this ProductEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new ProductResponseDTO
            {
                CodigoProducto = entity.CodigoProducto,
                Descripcion = entity.Descripcion,
                CodigoBarra = entity.CodigoBarra,
                SubCodigoHijo = entity.SubCodigoHijo,
                SubcodigoPadre = entity.SubCodigoPadre,
                FormaVenta = entity.FormaVentaCB,
                Precio = entity.Precio
            };
        }

        public static ProductEntity ToEntity(this ProductRequestDTO dto)
        {
            if (dto == null)   
                throw new ArgumentNullException(nameof(dto));
            
            return new ProductEntity
            {
                CodigoProducto = dto.CodigoProducto,
                Descripcion = dto.Descripcion,
                CodigoBarra = dto.CodigoBarra,
                SubCodigoHijo = dto.SubCodigoHijo,
                SubCodigoPadre = dto.SubCodigoPadre,
                FormaVentaCB = dto.FormaVenta
            };
        }
    }
}
