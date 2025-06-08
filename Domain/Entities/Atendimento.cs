using Hospisim.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Hospisim.Domain.Entities
{
    public class Atendimento
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime DataHora { get; set; } = DateTime.UtcNow;
        public TipoAtendimento Tipo { get; set; }
        public StatusAtendimento Status { get; set; } = StatusAtendimento.EmAndamento;

        [StringLength(40)]
        public string? Local { get; set; }

        // — FKs e navegações —
        public Guid PacienteId { get; set; }
        public Paciente Paciente { get; set; } = default!;

        public Guid ProfissionalId { get; set; }
        public ProfissionalSaude Profissional { get; set; } = default!;

        public Guid ProntuarioId { get; set; }
        public Prontuario Prontuario { get; set; } = default!;

        public ICollection<Prescricao> Prescricoes { get; set; } = new List<Prescricao>();
        public ICollection<Exame> Exames { get; set; } = new List<Exame>();

        public Internacao? Internacao { get; set; }
    }
}
