using ekomplet.services.Models;

namespace ekomplet.services.Interfaces
{
    public interface IInstallerService
    {
        public Task<List<Installer>> GetAllInstallersAsync();
        public Task<List<Installer>> GetInstallersBySupervisorAsync(Supervisor supervisor);
        public Task<Installer> GetInstallerByIdAsync(Guid id);
        public Task<Installer> GetInstallerByPhoneAsync(string phone);
        public Task<Installer> GetInstallerByEmailAsync(string email);
        public Task<int> CreateInstallerAsync(Installer installer);
        public Task<int> UpdateInstallerAsync(Installer installer);
        public Task<int> DeleteInstallerAsync(Guid id);
        public Task<int> DeleteInstallerAsync(Installer installer);
        public Task<int> AddSupervisorAsync(Supervisor newSupervisor, Installer installer);
        public Task<int> RemoveSupervisorFromInstallerAsync(Installer installer, Supervisor supervisor);
    }
}