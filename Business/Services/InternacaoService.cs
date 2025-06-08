using Hospisim.Business.Contracts;
using Hospisim.Data;
using Hospisim.Domain.Entities;
using Hospisim.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hospisim.Business.Services
{
    public class InternacaoService : IInternacaoService
    {
        private readonly AppDbContext _context;

        public InternacaoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Internacao>> GetAllAsync()
        {
            return await _context.Internacoes
                .Include(i => i.Paciente)
                .Include(i => i.Atendimento)
                .Include(i => i.Alta)
                .OrderByDescending(i => i.DataEntrada)
                .ToListAsync();
        }

        public async Task<Internacao?> GetByIdAsync(Guid id)
        {
            return await _context.Internacoes
                .Include(i => i.Paciente)
                .Include(i => i.Atendimento)
                    .ThenInclude(a => a.Profissional)
                .Include(i => i.Alta)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Internacao> CreateAsync(Internacao internacao)
        {
            _context.Internacoes.Add(internacao);
            await _context.SaveChangesAsync();
            return internacao;
        }

        public async Task<Internacao> UpdateAsync(Internacao internacao)
        {
            _context.Internacoes.Update(internacao);
            await _context.SaveChangesAsync();
            return internacao;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var internacao = await _context.Internacoes.FindAsync(id);
            if (internacao == null) return false;

            _context.Internacoes.Remove(internacao);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Internacoes.AnyAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Internacao>> GetByPacienteAsync(Guid pacienteId)
        {
            return await _context.Internacoes
                .Include(i => i.Atendimento)
                .Include(i => i.Alta)
                .Where(i => i.PacienteId == pacienteId)
                .OrderByDescending(i => i.DataEntrada)
                .ToListAsync();
        }

        public async Task<IEnumerable<Internacao>> GetByStatusAsync(StatusInternacao status)
        {
            return await _context.Internacoes
                .Include(i => i.Paciente)
                .Include(i => i.Atendimento)
                .Where(i => i.StatusInternacao == status)
                .OrderByDescending(i => i.DataEntrada)
                .ToListAsync();
        }

        public async Task<IEnumerable<Internacao>> GetAtivasAsync()
        {
            return await _context.Internacoes
                .Include(i => i.Paciente)
                .Include(i => i.Atendimento)
                .Where(i => i.StatusInternacao == StatusInternacao.Ativa)
                .OrderBy(i => i.Setor)
                .ThenBy(i => i.Quarto)
                .ToListAsync();
        }

        public async Task<IEnumerable<Internacao>> GetBySetorAsync(string setor)
        {
            return await _context.Internacoes
                .Include(i => i.Paciente)
                .Include(i => i.Atendimento)
                .Where(i => i.Setor == setor)
                .OrderBy(i => i.Quarto)
                .ThenBy(i => i.Leito)
                .ToListAsync();
        }
    }
} 