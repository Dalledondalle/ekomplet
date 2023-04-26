using ekomplet.Domain.Interfaces;
using ekomplet.Domain.Models;
using System.Data;
using System.Data.SqlClient;

namespace ekomplet.Application.Services
{
    public class SupervisorService : ISupervisorService
    {
        private readonly ISupervisorRepository repository;

        public SupervisorService(ISupervisorRepository repository)
        {
            this.repository = repository;
        }

        public async Task<int> CreateSupervisorAsync(Supervisor supervisor) 
            => await repository.CreateSupervisorAsync(supervisor);

        public async Task<int> DeleteSupervisorAsync(Guid id)
            => await repository.DeleteSupervisorAsync(id);

        public async Task<int> DeleteSupervisorAsync(Supervisor supervisor)
            => await repository.DeleteSupervisorAsync(supervisor);

        public async Task<List<Supervisor>> GetAllSupervisorAsync()
            => await repository.GetAllSupervisorAsync();

        public async Task<Supervisor> GetSupervisorByEmailAsync(string email)
            => await repository.GetSupervisorByEmailAsync(email);

        public async Task<Supervisor> GetSupervisorByIdAsync(Guid id)
            => await repository.GetSupervisorByIdAsync(id);

        public async Task<Supervisor> GetSupervisorByPhoneAsync(string phone)
            => await repository.GetSupervisorByPhoneAsync(phone);

        public async Task<List<Supervisor>> GetSupervisorsByInstallerAsync(Installer installer)
            => await repository.GetSupervisorsByInstallerAsync(installer);

        public async Task<int> RemoveInstallerFromSupervisorAsync(Supervisor supervisor, Installer installer)
            => await repository.RemoveInstallerFromSupervisorAsync(supervisor, installer);

        public async Task<int> UpdateSupervisorAsync(Supervisor supervisor)
            => await repository.UpdateSupervisorAsync(supervisor);
    }
}
