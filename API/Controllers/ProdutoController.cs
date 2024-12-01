using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly EstoqueContext _context;
    public ProdutoController(EstoqueContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    // GET: api/Produto
    [HttpGet]
    public ActionResult<IEnumerable<Produto>> GetProduto()
    {
        try
        {
            var Produto = _context.Produto.ToList();
            return Ok(Produto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor: " + ex.Message);
        }
    }
    // HEAD: api/Produto
    [HttpHead]
    public IActionResult HeadProduto()
    {
        try
        {
            var Produto = _context.Produto.ToList();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor: " + ex.Message);
        }
    }
    // POST: api/Produto
    [HttpPost]
    public async Task<ActionResult<Produto>> PostProduto([FromBody] Produto produto)
    {
        if (produto == null)
        {
            return BadRequest("Produto não pode ser nulo");
        }
        try
        {
            _context.Produto.Add(produto);
            await _context.SaveChangesAsync(); 
            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor: " + ex.Message);
        }
    }
    // PUT: api/Produto/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduto(int id, [FromBody] Produto produto)
    {
        if (produto == null || id != produto.Id)
        {
            return BadRequest("Produto inválido ou ID não corresponde");
        }
        try
        {
            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent(); 
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Produto.Any(p => p.Id == id))
            {
                return NotFound("Produto não encontrado");
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
    // PATCH: api/Produto/{id}
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchProduto(int id, [FromBody] Produto produto)
    {
        if (produto == null || id != produto.Id)
        {
            return BadRequest("Produto inválido ou ID não corresponde");
        }
        try
        {
            var produtoExistente = await _context.Produto.FindAsync(id);
            if (produtoExistente == null)
            {
                return NotFound("Produto não encontrado");
            }

            produtoExistente.Nome = produto.Nome ?? produtoExistente.Nome;
            produtoExistente.Descricao = produto.Descricao ?? produtoExistente.Descricao;
            produtoExistente.Quantidade = produto.Quantidade != 0 ? produto.Quantidade : produtoExistente.Quantidade;

            await _context.SaveChangesAsync();
            return NoContent(); // Retorna código 204 de sucesso
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor: " + ex.Message);
        }
    }
    // DELETE: api/Produto/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(int id)
    {
        try
        {
            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound("Produto não encontrado");
            }
            _context.Produto.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor: " + ex.Message);
        }
    }
    // OPTIONS: api/Produto
    [HttpOptions]
    public IActionResult OptionsProduto()
    {
        Response.Headers.Append("Allow", "GET, POST, PUT, PATCH, DELETE, OPTIONS");
        return Ok();
    }
}
