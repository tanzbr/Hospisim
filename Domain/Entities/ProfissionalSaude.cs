using Hospisim.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hospisim.Domain.Entities
{
    public class ProfissionalSaude
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(120)]
        public string NomeCompleto { get; set; } = default!;

        [Required, RegularExpression(@"\d{11}")]
        public string CPF { get; set; } = default!;

        [EmailAddress] public string? Email { get; set; }

        [Phone] public string? Telefone { get; set; }

        [Required] public string RegistroConselho { get; set; } = default!;
        public TipoRegistro TipoRegistro { get; set; }

        // — FK e navegação —
        public Guid EspecialidadeId { get; set; }
        public Especialidade Especialidade { get; set; } = default!;

        public DateTime DataAdmissao { get; set; }
        public int CargaHorariaSemanal { get; set; }
        public Turno Turno { get; set; }
        public bool Ativo { get; set; } = true;

        public ICollection<Atendimento> Atendimentos { get; set; } = new List<Atendimento>();
        public ICollection<Prescricao> Prescricoes { get; set; } = new List<Prescricao>();
    }
}
