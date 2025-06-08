using Hospisim.Domain.Entities;

namespace Hospisim.Models
{
    public class PacienteVM
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; } = default!;
        public string CPF { get; set; } = default!;
        public DateTime DataNascimento { get; set; }

        public static PacienteVM FromEntity(Paciente p) => new()
        {
            Id = p.Id,
            NomeCompleto = p.NomeCompleto,
            CPF = p.CPF,
            DataNascimento = p.DataNascimento
        };

        public void UpdateEntity(Paciente p)
        {
            p.NomeCompleto = NomeCompleto;
            p.CPF = CPF;
            p.DataNascimento = DataNascimento;
        }
    }

}
