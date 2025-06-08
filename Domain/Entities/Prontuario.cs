using System.ComponentModel.DataAnnotations;

namespace Hospisim.Domain.Entities
{
    public class Prontuario
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(20)]
        public string Numero { get; set; } = default!;

        public DateTime DataAbertura { get; set; } = DateTime.UtcNow;

        public string? ObservacoesGerais { get; set; }

        // — FK e navegação —
        public Guid PacienteId { get; set; }
        public Paciente Paciente { get; set; } = default!;

        public ICollection<Atendimento> Atendimentos { get; set; } = new List<Atendimento>();
    }
}
