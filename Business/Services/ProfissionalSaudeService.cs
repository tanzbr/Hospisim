using Hospisim.Business.Contracts;
using Hospisim.Data;
using Hospisim.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hospisim.Business.Services
{
    public class ProfissionalSaudeService : IProfissionalSaudeService
    {
        private readonly AppDbContext _context;

        public ProfissionalSaudeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProfissionalSaude>> GetAllAsync()
        {
            return await _context.Profissionais
                .Include(p => p.Especialidade)
                .OrderBy(p => p.NomeCompleto)
                .ToListAsync();
        }

        public async Task<ProfissionalSaude?> GetByIdAsync(Guid id)
        {
            return await _context.Profissionais
                .Include(p => p.Especialidade)
                .Include(p => p.Atendimentos)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProfissionalSaude> CreateAsync(ProfissionalSaude profissional)
        {
            _context.Profissionais.Add(profissional);
            await _context.SaveChangesAsync();
            return profissional;
        }

        public async Task<ProfissionalSaude> UpdateAsync(ProfissionalSaude profissional)
        {
            _context.Profissionais.Update(profissional);
            await _context.SaveChangesAsync();
            return profissional;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var profissional = await _context.Profissionais.FindAsync(id);
            if (profissional == null) return false;

            _context.Profissionais.Remove(profissional);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Profissionais.AnyAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<ProfissionalSaude>> GetByEspecialidadeAsync(Guid especialidadeId)
        {
            return await _context.Profissionais
                .Include(p => p.Especialidade)
                .Where(p => p.EspecialidadeId == especialidadeId)
                .OrderBy(p => p.NomeCompleto)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProfissionalSaude>> GetAtivosAsync()
        {
            return await _context.Profissionais
                .Include(p => p.Especialidade)
                .Where(p => p.Ativo)
                .OrderBy(p => p.NomeCompleto)
                .ToListAsync();
        }
    }
} 