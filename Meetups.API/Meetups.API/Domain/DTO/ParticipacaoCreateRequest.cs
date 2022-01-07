using System.ComponentModel.DataAnnotations;

namespace Meetups.API.Domain.DTO
{
    public class ParticipacaoCreateRequest
    {
        [Required]
        public int IdEvento { get; set; }
        [Required]
        [MaxLength(250, ErrorMessage = "Respeite o limite máximo de 250 caracteres")]
        public string LoginParticipante { get; set; } = null!;
    }
}
