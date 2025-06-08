using Hospisim.Domain.Entities;

namespace Hospisim.Business.Contracts
{
    public interface IPacienteService
    {
        Task<IEnumerable<Paciente>> GetAllAsync();
        Task<Paciente?> GetByIdAsync(Guid id);
        Task<Paciente> AddAsync(Paciente novo);
        Task<Paciente> UpdateAsync(Paciente editado);
        Task DeleteAsync(Guid id);
    }
}
