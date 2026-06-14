using Cafeteria_API.Data;
using Cafeteria_API.DTOs;
using Cafeteria_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cafeteria_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

      
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias()
        {
            var categorias = await _context.Categorias
                .Select(c => new CategoriaDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre
                })
                .ToListAsync();

            return Ok(categorias);
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return NotFound();

            return Ok(new CategoriaDTO
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            });
        }

        
        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> CrearCategoria(CrearCategoriaDTO dto)
        {
            var categoria = new Categoria
            {
                Nombre = dto.Nombre
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            var resultado = new CategoriaDTO
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, resultado);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCategoria(int id, CrearCategoriaDTO dto)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return NotFound();

            categoria.Nombre = dto.Nombre;
            await _context.SaveChangesAsync();

            return NoContent();
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return NotFound();

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
