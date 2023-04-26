using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ekomplet.services
{
    public interface ISupervisorRepository
    {
        public Task<List<Supervisor>> GetAllSupervisor();
        public Task<List<Supervisor>> GetSupervisorsByInstaller(Installer installer);
        public Task<Supervisor> GetSupervisorById(Guid id);
        public Task<Supervisor> GetSupervisorByPhone(string phone);
        public Task<Supervisor> GetSupervisorByEmail(string email);
        public Task<int> CreateSupervisor(Supervisor supervisor);
        public Task<int> UpdateSupervisor(Supervisor supervisor);
        public Task<int> DeleteSupervisor(Guid id);
        public Task<int> DeleteSupervisor(Supervisor supervisor);
        public Task<int> RemoveInstallerFromSupervisor(Supervisor supervisor, Installer installer);
    }
}
