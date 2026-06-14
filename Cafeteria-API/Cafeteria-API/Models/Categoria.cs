using System.ComponentModel.DataAnnotations;

namespace Cafeteria_API.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        // Relación uno-a-muchos: una categoría tiene muchos productos
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
