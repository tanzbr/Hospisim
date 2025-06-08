using Hospisim.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hospisim.Domain.Entities
{
    public class Prescricao
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();

        // — FKs e navegação —
        public Guid AtendimentoId { get; set; }
        public Atendimento Atendimento { get; set; } = default!;

        public Guid ProfissionalId { get; set; }
        public ProfissionalSaude Profissional { get; set; } = default!;

        [Required, StringLength(120)]
        public string Medicamento { get; set; } = default!;

        [Required, StringLength(30)]
        public string Dosagem { get; set; } = default!;

        [Required, StringLength(30)]
        public string Frequencia { get; set; } = default!;

        [Required, StringLength(30)]
        public string ViaAdministracao { get; set; } = default!;

        public DateTime DataInicio { get; set; } = DateTime.UtcNow;
        public DateTime? DataFim { get; set; }

        public string? Observacoes { get; set; }

        public StatusPrescricao StatusPrescricao { get; set; } = StatusPrescricao.Ativa;
        public string? ReacoesAdversas { get; set; }
    }
}
