using Cafeteria_API.Data;
using Cafeteria_API.DTOs;
using Cafeteria_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cafeteria_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetProductos()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Select(p => new ProductoDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria != null ? p.Categoria.Nombre : string.Empty
                })
                .ToListAsync();

            return Ok(productos);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDTO>> GetProducto(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                return NotFound();

            return Ok(new ProductoDTO
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = producto.Categoria != null ? producto.Categoria.Nombre : string.Empty
            });
        }

        
        [HttpPost]
        public async Task<ActionResult<ProductoDTO>> CrearProducto(CrearProductoDTO dto)
        {
            
            var categoria = await _context.Categorias.FindAsync(dto.CategoriaId);
            if (categoria == null)
                return BadRequest($"No existe una categoría con Id {dto.CategoriaId}.");

            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Precio = dto.Precio,
                CategoriaId = dto.CategoriaId
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            var resultado = new ProductoDTO
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = categoria.Nombre
            };

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, resultado);
        }

     
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, CrearProductoDTO dto)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            var categoria = await _context.Categorias.FindAsync(dto.CategoriaId);
            if (categoria == null)
                return BadRequest($"No existe una categoría con Id {dto.CategoriaId}.");

            producto.Nombre = dto.Nombre;
            producto.Precio = dto.Precio;
            producto.CategoriaId = dto.CategoriaId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
