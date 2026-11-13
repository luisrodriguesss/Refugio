
using System.ComponentModel.DataAnnotations;

namespace Refugio.API.DTOs;

public class PontoCreateDto
{
    [Required]
    [StringLength(7)]
    [RegularExpression(@"^REF-\d{3}$")]
    public string CodigoPublico { get; set; } = string.Empty;

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
