using ProductAPI.Domain.Entities;

namespace ProductAPI.App.DTOs.Conversions
{
    public static class ProductConversions
    {
        public static Product DTOToEntity(ProductDTO productDTO) => new Product
        {
            Id = productDTO.Id,
            Name = productDTO.Name,
            Price = productDTO.Price,
            StockQuantity = productDTO.StockQuantity,
            Quantity = productDTO.Quantity
        };


        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product, IEnumerable<ProductDTO> products)
        {
            // return single product
            if(product is not null && products is null)
            {
                var singleProduct = new ProductDTO
                    (
                        product.Id,
                        product.Name,
                        product.Quantity,
                        product.StockQuantity,
                        product.Price
                    );

                return (singleProduct, null);
            }

            if(products is not null && product is null)
            {
                var _products = products.Select(p =>
                    new ProductDTO(p.Id, p.Name, p.Quantity, p.Price, p.StockQuantity));

                return (null, _products);
            }

            return (null, null);
        }

        public static Product ToEntity(ProductDTO product)
        {
            if (product == null)
                return null;

            return new Product
            {
                Id = product.Id,
                Name = product.Name,
                Quantity = product.Quantity,
                StockQuantity = product.StockQuantity,
                Price = product.Price
            };
        }
    }
}
