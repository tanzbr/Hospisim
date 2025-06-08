using FluentValidation;
using Hospisim.Business.Contracts;
using Hospisim.Business.Validators;
using Hospisim.Data;
using Hospisim.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hospisim.Business.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly AppDbContext _ctx;
        private readonly PacienteValidator _validator = new();

        public PacienteService(AppDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Paciente>> GetAllAsync() =>
            await _ctx.Pacientes.AsNoTracking().ToListAsync();

        public async Task<Paciente?> GetByIdAsync(Guid id) =>
            await _ctx.Pacientes.FindAsync(id);

        public async Task<Paciente> AddAsync(Paciente novo)
        {
            _validator.ValidateAndThrow(novo);

            bool cpfDuplicado = await _ctx.Pacientes
                .AnyAsync(p => p.CPF == novo.CPF);
            if (cpfDuplicado)
                throw new InvalidOperationException("CPF já cadastrado");

            _ctx.Pacientes.Add(novo);
            await _ctx.SaveChangesAsync();
            return novo;
        }

        public async Task<Paciente> UpdateAsync(Paciente editado)
        {
            _validator.ValidateAndThrow(editado);
            _ctx.Pacientes.Update(editado);
            await _ctx.SaveChangesAsync();
            return editado;
        }

        public async Task DeleteAsync(Guid id)
        {
            var ent = await _ctx.Pacientes.FindAsync(id)
                      ?? throw new KeyNotFoundException("Paciente não encontrado");
            _ctx.Pacientes.Remove(ent);
            await _ctx.SaveChangesAsync();
        }
    }
}
