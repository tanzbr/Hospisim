using System.ComponentModel.DataAnnotations;

namespace Hospisim.Domain.Entities
{
    public class Exame
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(60)]
        public string Tipo { get; set; } = default!;   // sangue, imagem, etc.

        public DateTime DataSolicitacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataRealizacao { get; set; }

        public string? Resultado { get; set; }

        // — FK e navegação —
        public Guid AtendimentoId { get; set; }
        public Atendimento Atendimento { get; set; } = default!;
    }
}
