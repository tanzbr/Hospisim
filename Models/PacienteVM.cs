using Hospisim.Domain.Entities;
using Hospisim.Domain.Enums;

namespace Hospisim.Models
{
    public class PacienteVM
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; } = default!;
        public string CPF { get; set; } = default!;
        public DateTime DataNascimento { get; set; }

        public Sexo Sexo { get; set; }
        public TipoSanguineo TipoSanguineo { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? EnderecoCompleto { get; set; }
        public string? NumeroCartaoSUS { get; set; }
        public EstadoCivil? EstadoCivil { get; set; }
        public bool PossuiPlanoSaude { get; set; }

        public static PacienteVM FromEntity(Paciente p) => new()
        {
            Id = p.Id,
            NomeCompleto = p.NomeCompleto,
            CPF = p.CPF,
            DataNascimento = p.DataNascimento,
            Sexo = p.Sexo,
            TipoSanguineo = p.TipoSanguineo,
            Telefone = p.Telefone,
            Email = p.Email,
            EnderecoCompleto = p.EnderecoCompleto,
            NumeroCartaoSUS = p.NumeroCartaoSUS,
            EstadoCivil = p.EstadoCivil,
            PossuiPlanoSaude = p.PossuiPlanoSaude
        };

        public void UpdateEntity(Paciente p)
        {
            p.NomeCompleto = NomeCompleto;
            p.CPF = CPF;
            p.DataNascimento = DataNascimento;
            p.Sexo = Sexo;
            p.TipoSanguineo = TipoSanguineo;
            p.Telefone = Telefone;
            p.Email = Email;
            p.EnderecoCompleto = EnderecoCompleto;
            p.NumeroCartaoSUS = NumeroCartaoSUS;
            p.EstadoCivil = EstadoCivil;
            p.PossuiPlanoSaude = PossuiPlanoSaude;
        }
    }

}
