using Hospisim.Domain.Entities;

namespace Hospisim.Business.Contracts
{
    public interface IEspecialidadeService
    {
        Task<IEnumerable<Especialidade>> GetAllAsync();
        Task<Especialidade?> GetByIdAsync(Guid id);
        Task<Especialidade> CreateAsync(Especialidade especialidade);
        Task<Especialidade> UpdateAsync(Especialidade especialidade);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
} 