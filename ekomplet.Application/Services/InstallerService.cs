using ekomplet.Domain.Interfaces;
using ekomplet.Domain.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ekomplet.Application.Services
{
    public class InstallerService : IInstallerService
    {
        private readonly IInstallerRepository repository;

        public InstallerService(IInstallerRepository repository)
        {
            this.repository = repository;
        }

        public async Task<int> AddSupervisorAsync(Supervisor newSupervisor, Installer installer) 
            => await repository.AddSupervisorAsync(newSupervisor, installer);

        public async Task<int> CreateInstallerAsync(Installer installer) 
            => await repository.CreateInstallerAsync(installer);

        public async Task<int> DeleteInstallerAsync(Guid id) 
            => await repository.DeleteInstallerAsync(id);

        public async Task<int> DeleteInstallerAsync(Installer installer) 
            => await repository.DeleteInstallerAsync(installer);        

        public async Task<List<Installer>> GetAllInstallersAsync() 
            => await repository.GetAllInstallersAsync();

        public async Task<Installer> GetInstallerByEmailAsync(string email) 
            => await repository.GetInstallerByEmailAsync(email);
        
        public async Task<Installer> GetInstallerByIdAsync(Guid id) 
            => await repository.GetInstallerByIdAsync(id);        

        public async Task<Installer> GetInstallerByPhoneAsync(string phone) 
            => await repository.GetInstallerByPhoneAsync(phone);        

        public async Task<List<Installer>> GetInstallersBySupervisorAsync(Supervisor supervisor) 
            => await repository.GetInstallersBySupervisorAsync(supervisor);
        
        public async Task<int> RemoveSupervisorFromInstallerAsync(Installer installer, Supervisor supervisor) 
            => await repository.RemoveSupervisorFromInstallerAsync(installer, supervisor);

        public async Task<int> UpdateInstallerAsync(Installer installer) 
            => await repository.UpdateInstallerAsync(installer);        
    }
}
