using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly EstoqueContext _context;
    public CategoriaController(EstoqueContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    // GET: api/Categoria
    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> GetCategoria()
    {
        try
        {
            var Categoria = _context.Categoria.ToList();
            return Ok(Categoria);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor: " + ex.Message);
        }
    }
    // HEAD: api/Categoria
    [HttpHead]
    public IActionResult HeadCategoria()
    {
        try
        {
            var Categoria = _context.Categoria.ToList();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor: " + ex.Message);
        }
    }
    // POST: api/Categoria
    [HttpPost]
    public async Task<ActionResult<Categoria>> PostCategoria([FromBody] Categoria Categoria)
    {
        if (Categoria == null)
        {
            return BadRequest("Categoria não pode ser nulo");
        }
        try
        {
            _context.Categoria.Add(Categoria);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategoria), new { id = Categoria.Id }, Categoria);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor: " + ex.Message);
        }
    }
    // PUT: api/Categoria/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategoria(int id, [FromBody] Categoria Categoria)
    {
        if (Categoria == null || id != Categoria.Id)
        {
            return BadRequest("Categoria inválido ou ID não corresponde");
        }
        try
        {
            _context.Entry(Categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent(); 
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Categoria.Any(p => p.Id == id))
            {
                return NotFound("Categoria não encontrado");
            }
            else
            {
                throw;
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor: " + ex.Message);
        }
    }
    // PATCH: api/Categoria/{id}
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchCategoria(int id, [FromBody] Categoria Categoria)
    {
        if (Categoria == null || id != Categoria.Id)
        {
            return BadRequest("Categoria inválido ou ID não corresponde");
        }
        try
        {
            var CategoriaExistente = await _context.Categoria.FindAsync(id);
            if (CategoriaExistente == null)
            {
                return NotFound("Categoria não encontrado");
            }         
            CategoriaExistente.Nome = Categoria.Nome ?? CategoriaExistente.Nome;
            await _context.SaveChangesAsync();
            return NoContent(); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor: " + ex.Message);
        }
    }
    // DELETE: api/Categoria/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoria(int id)
    {
        try
        {
            var Categoria = await _context.Categoria.FindAsync(id);
            if (Categoria == null)
            {
                return NotFound("Categoria não encontrado");
            }
            _context.Categoria.Remove(Categoria);
            await _context.SaveChangesAsync();
            return NoContent(); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor: " + ex.Message);
        }
    }
    // OPTIONS: api/Categoria
    [HttpOptions]
    public IActionResult OptionsCategoria()
    {
        Response.Headers.Append("Allow", "GET, POST, PUT, PATCH, DELETE, OPTIONS");
        return Ok();
    }
}