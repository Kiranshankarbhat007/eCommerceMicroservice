using System.ComponentModel.DataAnnotations;

namespace ProductAPI.App.DTOs
{
    public record ProductDTO(
        long Id,
        [Required] string Name,
        [Required, Range(1, Int32.MaxValue)] int Quantity,
        [Required, DataType(DataType.Currency)] decimal Price,
        decimal StockQuantity
    );
}
