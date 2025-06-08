using Hospisim.Business.Contracts;
using Hospisim.Data;
using Hospisim.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hospisim.Business.Services
{
    public class AltaHospitalarService : IAltaHospitalarService
    {
        private readonly AppDbContext _context;

        public AltaHospitalarService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AltaHospitalar>> GetAllAsync()
        {
            return await _context.AltasHospitalares
                .Include(a => a.Internacao)
                    .ThenInclude(i => i!.Paciente)
                .Include(a => a.Internacao)
                    .ThenInclude(i => i!.Atendimento)
                .OrderByDescending(a => a.DataAlta)
                .ToListAsync();
        }

        public async Task<AltaHospitalar?> GetByIdAsync(Guid id)
        {
            return await _context.AltasHospitalares
                .Include(a => a.Internacao)
                    .ThenInclude(i => i!.Paciente)
                .Include(a => a.Internacao)
                    .ThenInclude(i => i!.Atendimento)
                        .ThenInclude(at => at!.Profissional)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AltaHospitalar> CreateAsync(AltaHospitalar alta)
        {
            _context.AltasHospitalares.Add(alta);
            await _context.SaveChangesAsync();
            return alta;
        }

        public async Task<AltaHospitalar> UpdateAsync(AltaHospitalar alta)
        {
            _context.AltasHospitalares.Update(alta);
            await _context.SaveChangesAsync();
            return alta;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var alta = await _context.AltasHospitalares.FindAsync(id);
            if (alta == null) return false;

            _context.AltasHospitalares.Remove(alta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.AltasHospitalares.AnyAsync(a => a.Id == id);
        }

        public async Task<AltaHospitalar?> GetByInternacaoAsync(Guid internacaoId)
        {
            return await _context.AltasHospitalares
                .Include(a => a.Internacao)
                    .ThenInclude(i => i!.Paciente)
                .FirstOrDefaultAsync(a => a.InternacaoId == internacaoId);
        }

        public async Task<IEnumerable<AltaHospitalar>> GetByDataAsync(DateTime data)
        {
            return await _context.AltasHospitalares
                .Include(a => a.Internacao)
                    .ThenInclude(i => i!.Paciente)
                .Where(a => a.DataAlta.Date == data.Date)
                .OrderBy(a => a.DataAlta)
                .ToListAsync();
        }

        public async Task<IEnumerable<AltaHospitalar>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _context.AltasHospitalares
                .Include(a => a.Internacao)
                    .ThenInclude(i => i!.Paciente)
                .Where(a => a.DataAlta.Date >= dataInicio.Date && a.DataAlta.Date <= dataFim.Date)
                .OrderByDescending(a => a.DataAlta)
                .ToListAsync();
        }
    }
} 