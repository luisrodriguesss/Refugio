
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Refugio.API.Data;
using Refugio.API.Models;

namespace Refugio.API.Controllers;

[ApiController]
[Route("api/pontos")]
public class PontosController : ControllerBase
{
    private readonly RefugioContext _context;

    public PontosController(RefugioContext context)
    {
        _context = context;
    }

    // GET: api/pontos
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var pontos = await _context.Pontos
            .AsNoTracking()
            .Select(p => new
            {
                p.IdPonto,
                p.CodigoPublico,
                p.Nome,
                p.Bairro,
                p.HorarioTexto,
                p.Status
            })
            .ToListAsync();

        return Ok(pontos);
    }

    // GET: api/pontos/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var ponto = await _context.Pontos
            .AsNoTracking()
            .Where(p => p.IdPonto == id)
            .Select(p => new
            {
                p.IdPonto,
                p.CodigoPublico,
                p.Nome,
                p.Bairro,
                p.HorarioTexto,
                p.Status
            })
            .FirstOrDefaultAsync();

        if (ponto == null)
        {
            return NotFound(new
            {
                mensagem = "Ponto não encontrado."
            });
        }

        return Ok(ponto);
    }

    // POST: api/pontos
    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] Ponto ponto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var codigoExistente = await _context.Pontos
            .AnyAsync(p => p.CodigoPublico == ponto.CodigoPublico);

        if (codigoExistente)
        {
            return Conflict(new
            {
                mensagem = "Já existe um ponto com esse código público."
            });
        }

        _context.Pontos.Add(ponto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = ponto.IdPonto },
            ponto
        );
    }

    // PUT: api/pontos/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(
        int id,
        [FromBody] Ponto dados)
    {
        var ponto = await _context.Pontos
            .FindAsync(id);

        if (ponto == null)
        {
            return NotFound(new
            {
                mensagem = "Ponto não encontrado."
            });
        }

        // O código público não pode ser alterado.
        if (dados.CodigoPublico != ponto.CodigoPublico)
        {
            return BadRequest(new
            {
                mensagem = "O código público não pode ser alterado."
            });
        }

        ponto.Nome = dados.Nome;
        ponto.Bairro = dados.Bairro;
        ponto.HorarioTexto = dados.HorarioTexto;
        ponto.Status = dados.Status;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/pontos/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var ponto = await _context.Pontos.FindAsync(id);

        if (ponto == null)
        {
            return NotFound(new
            {
                mensagem = "Ponto não encontrado."
            });
        }

        var possuiRecursos = await _context.Recursos
            .AnyAsync(r => r.IdPonto == id);

        if (possuiRecursos)
        {
            return Conflict(new
            {
                mensagem = "Não é possível excluir um ponto que possui recursos vinculados."
            });
        }

        _context.Pontos.Remove(ponto);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
