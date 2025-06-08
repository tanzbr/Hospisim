using Hospisim.Domain.Entities;
using Hospisim.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hospisim.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider services)
        {
            using var ctx = new AppDbContext(
                services.GetRequiredService<DbContextOptions<AppDbContext>>());

            // Evita duplicar se já existir seed
            if (ctx.Pacientes.Any()) return;

            // ---------- dados básicos ----------
            var especialidades = new[]
            {
            "Cardiologia","Pediatria","Ortopedia","Clínica Geral","Neurologia",
            "Ginecologia","Dermatologia","Oncologia","Endocrinologia","Psiquiatria"
        }
            .Select(n => new Especialidade { Id = Guid.NewGuid(), Nome = n })
            .ToList();
            ctx.Especialidades.AddRange(especialidades);

            // ---------- auxiliares ----------
            string GeraCpf(int i) => (10000000000 + i).ToString();               // 11 dígitos
            string GeraEmail(string baseName, int i) => $"{baseName}{i}@exemplo.com";
            string GeraFone(int i) => $"(11)90000-00{i:00}";
            DateTime RandDate(int startYear) => new DateTime(startYear, 1, 1)
                                                   .AddDays(Random.Shared.Next(0, 365 * 10));

            // ---------- Profissionais ----------
            var profs = new List<ProfissionalSaude>();
            for (int i = 1; i <= 10; i++)
            {
                var esp = especialidades[i % especialidades.Count];
                profs.Add(new ProfissionalSaude
                {
                    Id = Guid.NewGuid(),
                    NomeCompleto = $"Médico {i}",
                    CPF = GeraCpf(700 + i),
                    Email = GeraEmail("medico", i),
                    Telefone = GeraFone(i),
                    RegistroConselho = $"CRM-{1000 + i}",
                    TipoRegistro = TipoRegistro.CRM,
                    EspecialidadeId = esp.Id,
                    DataAdmissao = RandDate(2019),
                    CargaHorariaSemanal = 40,
                    Turno = (Turno)(i % 3),
                    Ativo = true
                });
            }
            ctx.Profissionais.AddRange(profs);

            // ---------- Pacientes ----------
            var pacientes = new List<Paciente>();
            for (int i = 1; i <= 10; i++)
            {
                pacientes.Add(new Paciente
                {
                    Id = Guid.NewGuid(),
                    NomeCompleto = $"Paciente {i}",
                    CPF = GeraCpf(100 + i),
                    DataNascimento = RandDate(1950),
                    Sexo = (Sexo)(i % 3),
                    TipoSanguineo = (TipoSanguineo)(i % 8),
                    Telefone = GeraFone(20 + i),
                    Email = GeraEmail("paciente", i),
                    EnderecoCompleto = $"Rua {i}, nº {i * 10}, Cidade",
                    NumeroCartaoSUS = (90000000000 + i).ToString(),
                    EstadoCivil = (EstadoCivil)(i % 5),
                    PossuiPlanoSaude = i % 2 == 0
                });
            }
            ctx.Pacientes.AddRange(pacientes);

            // ---------- Prontuários ----------
            var prontuarios = pacientes
                .Select(p => new Prontuario
                {
                    Id = Guid.NewGuid(),
                    Numero = $"PR-{p.CPF}",
                    DataAbertura = DateTime.UtcNow,
                    PacienteId = p.Id
                }).ToList();
            ctx.Prontuarios.AddRange(prontuarios);

            // ---------- Atendimentos ----------
            var atendimentos = new List<Atendimento>();
            for (int i = 0; i < 10; i++)
            {
                atendimentos.Add(new Atendimento
                {
                    Id = Guid.NewGuid(),
                    DataHora = DateTime.UtcNow.AddMinutes(-30 * i),
                    Tipo = (TipoAtendimento)(i % 3),
                    Status = StatusAtendimento.Realizado,
                    Local = $"Sala {i + 1:00}",
                    PacienteId = pacientes[i].Id,
                    ProfissionalId = profs[i].Id,
                    ProntuarioId = prontuarios[i].Id
                });
            }
            ctx.Atendimentos.AddRange(atendimentos);

            // ---------- Prescrições ----------
            var prescricoes = atendimentos.Select(a => new Prescricao
            {
                Id = Guid.NewGuid(),
                AtendimentoId = a.Id,
                ProfissionalId = a.ProfissionalId,
                Medicamento = "Paracetamol",
                Dosagem = "750mg",
                Frequencia = "8/8h",
                ViaAdministracao = "Oral",
                DataInicio = a.DataHora,
                StatusPrescricao = StatusPrescricao.Ativa
            }).ToList();
            ctx.Prescricoes.AddRange(prescricoes);

            // ---------- Exames ----------
            var exames = atendimentos.Select(a => new Exame
            {
                Id = Guid.NewGuid(),
                Tipo = "Hemograma",
                DataSolicitacao = a.DataHora,
                Resultado = "Normal",
                AtendimentoId = a.Id
            }).ToList();
            ctx.Exames.AddRange(exames);

            // ---------- Internações e Altas ----------
            var internacoes = new List<Internacao>();
            var altas = new List<AltaHospitalar>();

            for (int i = 0; i < 10; i++)
            {
                var intern = new Internacao
                {
                    Id = Guid.NewGuid(),
                    PacienteId = pacientes[i].Id,
                    AtendimentoId = atendimentos[i].Id,
                    DataEntrada = DateTime.UtcNow.AddDays(-i),
                    PrevisaoAlta = DateTime.UtcNow.AddDays(3 - i),
                    MotivoInternacao = "Observação clínica",
                    Leito = $"L-{101 + i}",
                    Quarto = $"Q-{1 + i / 2}",
                    Setor = "Clínica Geral",
                    PlanoSaudeUtilizado = pacientes[i].PossuiPlanoSaude ? "Unimed" : null,
                    StatusInternacao = StatusInternacao.Ativa
                };
                internacoes.Add(intern);

                altas.Add(new AltaHospitalar
                {
                    Id = Guid.NewGuid(),
                    InternacaoId = intern.Id,
                    DataAlta = intern.DataEntrada.AddDays(2),
                    CondicaoPaciente = "Estável",
                    InstrucoesPosAlta = "Retorno em 10 dias."
                });
            }
            ctx.Internacoes.AddRange(internacoes);
            ctx.AltasHospitalares.AddRange(altas);

            // ---------- grava ----------
            ctx.SaveChanges();
        }
    }
}
