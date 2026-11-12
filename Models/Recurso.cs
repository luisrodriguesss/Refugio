
using System.ComponentModel.DataAnnotations;

namespace Refugio.API.Models;

public class Recurso
{
    public int IdRecurso { get; set; }

    [Required]
    public int IdPonto { get; set; }

    [Required]
    [StringLength(20)]
    [RegularExpression("^(AGUA|SOMBRA|ASSENTO|ACESSIBILIDADE)$")]
    public string Tipo { get; set; } = string.Empty;

    public bool Disponivel { get; set; }

    public Ponto? Ponto { get; set; }
}
