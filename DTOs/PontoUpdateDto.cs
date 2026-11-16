
using System.ComponentModel.DataAnnotations;

namespace Refugio.API.DTOs;

public class PontoUpdateDto
{
    [Required]
    [StringLength(110)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Bairro { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string HorarioTexto { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(ATIVO|INATIVO)$")]
    public string Status { get; set; } = "ATIVO";
}
