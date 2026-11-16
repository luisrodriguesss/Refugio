
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Refugio.API.Data;
using Refugio.API.DTOs;
using Refugio.API.Models;

namespace Refugio.API.Controllers;

[ApiController]
[Route("api/recursos")]
public class RecursosController : ControllerBase
{
    private readonly RefugioContext _context;

    public RecursosController(RefugioContext context)
    {
        _context = context;
    }

    // GET: api/recursos
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var recursos = await _context.Recursos
            .AsNoTracking()
            .Select(r => new
            {
                r.IdRecurso,
                r.IdPonto,
                r.Tipo,
                r.Disponivel
            })
            .ToListAsync();

        return Ok(recursos);
    }

    // GET: api/recursos/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var recurso = await _context.Recursos
            .AsNoTracking()
            .Where(r => r.IdRecurso == id)
            .Select(r => new
            {
                r.IdRecurso,
                r.IdPonto,
                r.Tipo,
                r.Disponivel
            })
            .FirstOrDefaultAsync();

        if (recurso == null)
        {
            return NotFound(new
            {
                mensagem = "Recurso não encontrado."
            });
        }

        return Ok(recurso);
    }

    // POST: api/recursos
    [HttpPost]
    public async Task<IActionResult> Cadastrar(
        [FromBody] RecursoCreateDto dados)
    {
        var pontoExiste = await _context.Pontos
            .AnyAsync(p => p.IdPonto == dados.IdPonto);

        if (!pontoExiste)
        {
            return NotFound(new
            {
                mensagem = "O ponto informado não existe."
            });
        }

        var recursoDuplicado = await _context.Recursos
            .AnyAsync(r =>
                r.IdPonto == dados.IdPonto &&
                r.Tipo == dados.Tipo);

        if (recursoDuplicado)
        {
            return Conflict(new
            {
                mensagem = "Esse recurso já está cadastrado neste ponto."
            });
        }

        var recurso = new Recurso
        {
            IdPonto = dados.IdPonto,
            Tipo = dados.Tipo,
            Disponivel = dados.Disponivel
        };

        _context.Recursos.Add(recurso);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = recurso.IdRecurso },
            new
            {
                recurso.IdRecurso,
                recurso.IdPonto,
                recurso.Tipo,
                recurso.Disponivel
            }
        );
    }

    // PUT: api/recursos/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(
        int id,
        [FromBody] Recurso dados)
    {
        var recurso = await _context.Recursos.FindAsync(id);

        if (recurso == null)
        {
            return NotFound(new
            {
                mensagem = "Recurso não encontrado."
            });
        }

        var pontoExiste = await _context.Pontos
            .AnyAsync(p => p.IdPonto == dados.IdPonto);

        if (!pontoExiste)
        {
            return NotFound(new
            {
                mensagem = "O ponto informado não existe."
            });
        }

        var duplicado = await _context.Recursos
            .AnyAsync(r =>
                r.IdPonto == dados.IdPonto &&
                r.Tipo == dados.Tipo &&
                r.IdRecurso != id);

        if (duplicado)
        {
            return Conflict(new
            {
                mensagem = "Esse recurso já existe neste ponto."
            });
        }

        recurso.IdPonto = dados.IdPonto;
        recurso.Tipo = dados.Tipo;
        recurso.Disponivel = dados.Disponivel;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/recursos/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var recurso = await _context.Recursos.FindAsync(id);

        if (recurso == null)
        {
            return NotFound(new
            {
                mensagem = "Recurso não encontrado."
            });
        }

        _context.Recursos.Remove(recurso);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
