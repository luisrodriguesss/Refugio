
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
}
