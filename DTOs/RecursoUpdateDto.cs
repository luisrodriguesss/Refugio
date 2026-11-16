
using System.ComponentModel.DataAnnotations;

namespace Refugio.API.DTOs;

public class RecursoUpdateDto
{
    [Range(1, int.MaxValue)]
    public int IdPonto { get; set; }

    [Required]
    [RegularExpression(
        "^(AGUA|SOMBRA|ASSENTO|ACESSIBILIDADE)$",
        ErrorMessage = "Tipo de recurso inválido."
    )]
    public string Tipo { get; set; } = string.Empty;

    public bool Disponivel { get; set; }
}
