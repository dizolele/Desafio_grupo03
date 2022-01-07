using System.ComponentModel.DataAnnotations;

namespace Meetups.API.Domain.DTO
{
    public class EventoCreateRequest
    {
        [Required]
        [MaxLength(250, ErrorMessage = "Digite no máximo 250 caracteres")]
        public string Nome { get; set; }
        [Required]
        [MaxLength(250, ErrorMessage = "Digite no máximo 250 caracteres")]
        public string Local { get; set; }
        [Required]
        [MaxLength(250, ErrorMessage = "Digite no máximo 250 caracteres")]
        public string Descricao { get; set; }
        [Required]
        public int LimiteVagas { get; set; }
        [Required]
        [Range(1, 9)]
        public int IdCategoriaEvento { get; set; }
        [Required]
        public DateTime DataHoraInicio { get; set; }
        [Required]
        public DateTime DataHoraFim { get; set; }

    }
}
