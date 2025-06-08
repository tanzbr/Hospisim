using System.ComponentModel.DataAnnotations;

namespace Hospisim.Domain.Entities
{
    public class AltaHospitalar
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime DataAlta { get; set; } = DateTime.UtcNow;

        [Required, StringLength(200)]
        public string CondicaoPaciente { get; set; } = default!;

        [Required]
        public string InstrucoesPosAlta { get; set; } = default!;

        // — FK e navegação —
        public Guid InternacaoId { get; set; }
        public Internacao Internacao { get; set; } = default!;
    }
}
