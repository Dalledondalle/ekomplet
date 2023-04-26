using ekomplet.Domain.Models;

namespace ekomplet.Domain.Interfaces
{
    public interface ISupervisorRepository
    {
        public Task<List<Supervisor>> GetAllSupervisorAsync();
        public Task<List<Supervisor>> GetSupervisorsByInstallerAsync(Installer installer);
        public Task<Supervisor> GetSupervisorByIdAsync(Guid id);
        public Task<Supervisor> GetSupervisorByPhoneAsync(string phone);
        public Task<Supervisor> GetSupervisorByEmailAsync(string email);
        public Task<int> CreateSupervisorAsync(Supervisor supervisor);
        public Task<int> UpdateSupervisorAsync(Supervisor supervisor);
        public Task<int> DeleteSupervisorAsync(Guid id);
        public Task<int> DeleteSupervisorAsync(Supervisor supervisor);
        public Task<int> RemoveInstallerFromSupervisorAsync(Supervisor supervisor, Installer installer);
    }
}
