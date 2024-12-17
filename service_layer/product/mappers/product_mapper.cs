using DTOLayer.Product;
using RepositoryLayer.Repositories.Product.Entity;
using System;

namespace ServiceLayer.Product.Mappers
{
    public static class ProductMapper
    {
        public static ProductResponseDTO ToDTO(this ProductEntity entity) => new ProductResponseDTO()
        {
            CodigoProducto = entity?.CodigoProducto,
            Descripcion = entity?.Descripcion
        };

        public static ProductEntity ToEntity(this ProductRequestDTO entity) => new ProductEntity()
        {
            CodigoProducto = entity.CodigoProducto,
            Descripcion = entity.Descripcion,
            CodigoBarra= entity.CodigoBarra,
            Caja = string.Empty,
            Pieza = string.Empty,
            FormaVentaCB = string.Empty
        };
    }
}
