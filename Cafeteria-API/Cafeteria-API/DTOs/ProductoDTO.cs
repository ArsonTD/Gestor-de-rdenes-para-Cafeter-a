using System.ComponentModel.DataAnnotations;

namespace Cafeteria_API.DTOs
{
    // DTO de respuesta (lo que devuelve la API)
    public class ProductoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
    }

    // DTO para crear / actualizar un producto
    public class CrearProductoDTO
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Range(0, 999999)]
        public decimal Precio { get; set; }

        [Required]
        public int CategoriaId { get; set; }
    }
}
