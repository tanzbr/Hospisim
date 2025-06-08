using Hospisim.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hospisim.Domain.Entities
{
    public class Paciente
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(120)]
        public string NomeCompleto { get; set; } = default!;

        [Required, RegularExpression(@"\d{11}")]
        public string CPF { get; set; } = default!;

        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        public Sexo Sexo { get; set; }
        public TipoSanguineo TipoSanguineo { get; set; }

        [Phone] public string? Telefone { get; set; }
        [EmailAddress] public string? Email { get; set; }
        public string? EnderecoCompleto { get; set; }
        public string? NumeroCartaoSUS { get; set; }
        public EstadoCivil? EstadoCivil { get; set; }
        public bool PossuiPlanoSaude { get; set; }

        // Navegação
        public ICollection<Prontuario> Prontuarios { get; set; } = new List<Prontuario>();
    }
}
