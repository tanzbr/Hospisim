using Hospisim.Domain.Entities;

namespace Hospisim.Business.Contracts
{
    public interface IAltaHospitalarService
    {
        Task<IEnumerable<AltaHospitalar>> GetAllAsync();
        Task<AltaHospitalar?> GetByIdAsync(Guid id);
        Task<AltaHospitalar> CreateAsync(AltaHospitalar alta);
        Task<AltaHospitalar> UpdateAsync(AltaHospitalar alta);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<AltaHospitalar?> GetByInternacaoAsync(Guid internacaoId);
        Task<IEnumerable<AltaHospitalar>> GetByDataAsync(DateTime data);
        Task<IEnumerable<AltaHospitalar>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    }
} 