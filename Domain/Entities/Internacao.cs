using Hospisim.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hospisim.Domain.Entities
{
    public class Internacao
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();

        public Guid PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        public Guid AtendimentoId { get; set; }
        public Atendimento? Atendimento { get; set; }

        public DateTime DataEntrada { get; set; } = DateTime.UtcNow;
        public DateTime? PrevisaoAlta { get; set; }

        [Required] public string MotivoInternacao { get; set; } = default!;

        [Required] public string Leito { get; set; } = default!;
        [Required] public string Quarto { get; set; } = default!;
        [Required] public string Setor { get; set; } = default!;   // UTI, Clínica Geral…

        public string? PlanoSaudeUtilizado { get; set; }
        public string? ObservacoesClinicas { get; set; }

        public StatusInternacao StatusInternacao { get; set; } = StatusInternacao.Ativa;

        public AltaHospitalar? Alta { get; set; }
    }
}
