using Hospisim.Business.Contracts;
using Hospisim.Data;
using Hospisim.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hospisim.Business.Services
{
    public class ProntuarioService : IProntuarioService
    {
        private readonly AppDbContext _context;

        public ProntuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Prontuario>> GetAllAsync()
        {
            return await _context.Prontuarios
                .Include(p => p.Paciente)
                .Include(p => p.Atendimentos)
                .OrderByDescending(p => p.DataAbertura)
                .ToListAsync();
        }

        public async Task<Prontuario?> GetByIdAsync(Guid id)
        {
            return await _context.Prontuarios
                .Include(p => p.Paciente)
                .Include(p => p.Atendimentos)
                    .ThenInclude(a => a.Profissional)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Prontuario> CreateAsync(Prontuario prontuario)
        {
            _context.Prontuarios.Add(prontuario);
            await _context.SaveChangesAsync();
            return prontuario;
        }

        public async Task<Prontuario> UpdateAsync(Prontuario prontuario)
        {
            _context.Entry(prontuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return prontuario;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var prontuario = await _context.Prontuarios.FindAsync(id);
            if (prontuario == null) return false;

            _context.Prontuarios.Remove(prontuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Prontuarios.AnyAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Prontuario>> GetByPacienteAsync(Guid pacienteId)
        {
            return await _context.Prontuarios
                .Include(p => p.Atendimentos)
                .Where(p => p.PacienteId == pacienteId)
                .OrderByDescending(p => p.DataAbertura)
                .ToListAsync();
        }

        public async Task<Prontuario?> GetByNumeroAsync(string numero)
        {
            return await _context.Prontuarios
                .Include(p => p.Paciente)
                .Include(p => p.Atendimentos)
                .FirstOrDefaultAsync(p => p.Numero == numero);
        }
    }
} 