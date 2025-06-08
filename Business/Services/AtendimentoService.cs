using Hospisim.Business.Contracts;
using Hospisim.Data;
using Hospisim.Domain.Entities;
using Hospisim.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hospisim.Business.Services
{
    public class AtendimentoService : IAtendimentoService
    {
        private readonly AppDbContext _context;

        public AtendimentoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Atendimento>> GetAllAsync()
        {
            return await _context.Atendimentos
                .Include(a => a.Paciente)
                .Include(a => a.Profissional)
                .Include(a => a.Prontuario)
                .OrderByDescending(a => a.DataHora)
                .ToListAsync();
        }

        public async Task<Atendimento?> GetByIdAsync(Guid id)
        {
            return await _context.Atendimentos
                .Include(a => a.Paciente)
                .Include(a => a.Profissional)
                .Include(a => a.Prontuario)
                .Include(a => a.Prescricoes)
                .Include(a => a.Exames)
                .Include(a => a.Internacao)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Atendimento> CreateAsync(Atendimento atendimento)
        {
            _context.Atendimentos.Add(atendimento);
            await _context.SaveChangesAsync();
            return atendimento;
        }

        public async Task<Atendimento> UpdateAsync(Atendimento atendimento)
        {
            _context.Atendimentos.Update(atendimento);
            await _context.SaveChangesAsync();
            return atendimento;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var atendimento = await _context.Atendimentos.FindAsync(id);
            if (atendimento == null) return false;

            _context.Atendimentos.Remove(atendimento);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Atendimentos.AnyAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Atendimento>> GetByPacienteAsync(Guid pacienteId)
        {
            return await _context.Atendimentos
                .Include(a => a.Profissional)
                .Include(a => a.Prontuario)
                .Where(a => a.PacienteId == pacienteId)
                .OrderByDescending(a => a.DataHora)
                .ToListAsync();
        }

        public async Task<IEnumerable<Atendimento>> GetByProfissionalAsync(Guid profissionalId)
        {
            return await _context.Atendimentos
                .Include(a => a.Paciente)
                .Include(a => a.Prontuario)
                .Where(a => a.ProfissionalId == profissionalId)
                .OrderByDescending(a => a.DataHora)
                .ToListAsync();
        }

        public async Task<IEnumerable<Atendimento>> GetByStatusAsync(StatusAtendimento status)
        {
            return await _context.Atendimentos
                .Include(a => a.Paciente)
                .Include(a => a.Profissional)
                .Where(a => a.Status == status)
                .OrderByDescending(a => a.DataHora)
                .ToListAsync();
        }

        public async Task<IEnumerable<Atendimento>> GetByDataAsync(DateTime data)
        {
            return await _context.Atendimentos
                .Include(a => a.Paciente)
                .Include(a => a.Profissional)
                .Where(a => a.DataHora.Date == data.Date)
                .OrderBy(a => a.DataHora)
                .ToListAsync();
        }
    }
} 