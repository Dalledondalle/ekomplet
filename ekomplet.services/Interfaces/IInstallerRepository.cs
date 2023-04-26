using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ekomplet.services
{
    public interface IInstallerRepository
    {
        public Task<List<Installer>> GetAllInstallers();
        public Task<List<Installer>> GetInstallersBySupervisor(Supervisor supervisor);
        public Task<Installer> GetInstallerById(Guid id);
        public Task<Installer> GetInstallerByPhone(string phone);
        public Task<Installer> GetInstallerByEmail(string email);
        public Task<int> CreateInstaller(Installer installer);
        public Task<int> UpdateInstaller(Installer installer);
        public Task<int> DeleteInstaller(Guid id);
        public Task<int> DeleteInstaller(Installer installer);
        public Task<int> AddSupervisor(Supervisor newSupervisor, Installer installer);
        public Task<int> RemoveSupervisorFromInstaller(Installer installer, Supervisor supervisor);
    }
}
