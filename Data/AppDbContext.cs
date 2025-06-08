using Hospisim.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hospisim.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Paciente> Pacientes => Set<Paciente>();
        public DbSet<Prontuario> Prontuarios => Set<Prontuario>();
        public DbSet<ProfissionalSaude> Profissionais => Set<ProfissionalSaude>();
        public DbSet<Especialidade> Especialidades => Set<Especialidade>();
        public DbSet<Atendimento> Atendimentos => Set<Atendimento>();
        public DbSet<Prescricao> Prescricoes => Set<Prescricao>();
        public DbSet<Exame> Exames => Set<Exame>();
        public DbSet<Internacao> Internacoes => Set<Internacao>();
        public DbSet<AltaHospitalar> AltasHospitalares => Set<AltaHospitalar>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            // Paciente 1-N Prontuário
            mb.Entity<Prontuario>()
              .HasOne(p => p.Paciente)
              .WithMany(pac => pac.Prontuarios)
              .HasForeignKey(p => p.PacienteId)
              .OnDelete(DeleteBehavior.Cascade);

            // Profissional N-1 Especialidade
            mb.Entity<ProfissionalSaude>()
              .HasOne(p => p.Especialidade)
              .WithMany(e => e.Profissionais)
              .HasForeignKey(p => p.EspecialidadeId);

            // Atendimento N-1 Paciente
            mb.Entity<Atendimento>()
              .HasOne(a => a.Paciente)
              .WithMany()
              .HasForeignKey(a => a.PacienteId)
              .OnDelete(DeleteBehavior.Cascade);

            // Atendimento N-1 Prontuário
            mb.Entity<Atendimento>()
              .HasOne(a => a.Prontuario)
              .WithMany(p => p.Atendimentos)
              .HasForeignKey(a => a.ProntuarioId)
              .OnDelete(DeleteBehavior.Restrict);

            // Atendimento N-1 Profissional
            mb.Entity<Atendimento>()
              .HasOne(a => a.Profissional)
              .WithMany(p => p.Atendimentos)
              .HasForeignKey(a => a.ProfissionalId);

            // Atendimento 1-1 Internação
            mb.Entity<Atendimento>()
              .HasOne(a => a.Internacao)
              .WithOne(i => i.Atendimento)
              .HasForeignKey<Internacao>(i => i.AtendimentoId)
              .OnDelete(DeleteBehavior.Cascade);

            // Internação N-1 Paciente
            mb.Entity<Internacao>()
              .HasOne(i => i.Paciente)
              .WithMany()
              .HasForeignKey(i => i.PacienteId)
              .OnDelete(DeleteBehavior.Restrict);

            // Internação 1-1 Alta Hospitalar
            mb.Entity<Internacao>()
              .HasOne(i => i.Alta)
              .WithOne(a => a.Internacao)
              .HasForeignKey<AltaHospitalar>(a => a.InternacaoId);

            // --- Prescricao ---
            mb.Entity<Prescricao>()
              .HasOne(p => p.Atendimento)
              .WithMany(a => a.Prescricoes)
              .HasForeignKey(p => p.AtendimentoId)
              .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<Prescricao>()
              .HasOne(p => p.Profissional)
              .WithMany(pr => pr.Prescricoes)
              .HasForeignKey(p => p.ProfissionalId)
              .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
