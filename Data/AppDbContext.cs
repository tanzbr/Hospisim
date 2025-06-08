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
              .HasForeignKey(p => p.PacienteId);

            // Profissional N-1 Especialidade
            mb.Entity<ProfissionalSaude>()
              .HasOne(p => p.Especialidade)
              .WithMany(e => e.Profissionais)
              .HasForeignKey(p => p.EspecialidadeId);

            // Mapeie os demais relacionamentos (Atendimento ↔ Paciente/Profissional/Prontuário etc.)
        }
    }
}
