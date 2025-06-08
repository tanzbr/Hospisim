using Hospisim.Business.Contracts;
using Hospisim.Data;
using Hospisim.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hospisim.Business.Services
{
    public class EspecialidadeService : IEspecialidadeService
    {
        private readonly AppDbContext _context;

        public EspecialidadeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Especialidade>> GetAllAsync()
        {
            return await _context.Especialidades
                .Include(e => e.Profissionais)
                .OrderBy(e => e.Nome)
                .ToListAsync();
        }

        public async Task<Especialidade?> GetByIdAsync(Guid id)
        {
            return await _context.Especialidades
                .Include(e => e.Profissionais)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Especialidade> CreateAsync(Especialidade especialidade)
        {
            _context.Especialidades.Add(especialidade);
            await _context.SaveChangesAsync();
            return especialidade;
        }

        public async Task<Especialidade> UpdateAsync(Especialidade especialidade)
        {
            _context.Entry(especialidade).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return especialidade;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var especialidade = await _context.Especialidades.FindAsync(id);
            if (especialidade == null) return false;

            _context.Especialidades.Remove(especialidade);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Especialidades.AnyAsync(e => e.Id == id);
        }
    }
} 