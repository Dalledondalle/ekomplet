using ekomplet.Domain.Interfaces;
using ekomplet.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ekomplet.Infrastructure.Repositories
{
    public class SupervisorRepository : ISupervisorRepository
    {
        private readonly string connectionString;

        public SupervisorRepository(IConfiguration config)
        {
            connectionString = config.GetConnectionString("LocalSQL")!;
        }
        public async Task<int> CreateSupervisorAsync(Supervisor supervisor)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("new_supervisor", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@firstname", supervisor.Firstname);
            command.Parameters.AddWithValue("@lastname", supervisor.Lastname);
            command.Parameters.AddWithValue("@phone", supervisor.Phone);
            command.Parameters.AddWithValue("@email", supervisor.Email);

            return await command.ExecuteNonQueryAsync();

        }

        public async Task<int> DeleteSupervisorAsync(Guid id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("delete_supervisor", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteSupervisorAsync(Supervisor supervisor)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync();

            using var command = new SqlCommand("delete_supervisor", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", supervisor.Id);

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Supervisor>> GetAllSupervisorAsync()
        {
            var supervisors = new List<Supervisor>();

            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT supervisor_id, firstname, lastname, phone_number, email 
                    FROM supervisors",
                connection);

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                supervisors.Add(new Supervisor(reader));

            return supervisors;
        }

        public async Task<Supervisor> GetSupervisorByEmailAsync(string email)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("get_supervisor_email", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@email", email);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
                return new Supervisor(reader);
            return new();
        }

        public async Task<Supervisor> GetSupervisorByIdAsync(Guid id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("get_supervisor_id", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id.ToString());

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return new Supervisor(reader);

            return new();
        }

        public async Task<Supervisor> GetSupervisorByPhoneAsync(string phone)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("get_supervisor_phone", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@phone", phone);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return new Supervisor(reader);

            return new();
        }

        public async Task<List<Supervisor>> GetSupervisorsByInstallerAsync(Installer installer)
        {
            var supervisors = new List<Supervisor>();

            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();


            using var command = new SqlCommand("get_supervisors_by_installers", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", installer.Id);
            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                supervisors.Add(new Supervisor(reader));

            return supervisors;
        }

        public async Task<int> RemoveInstallerFromSupervisorAsync(Supervisor supervisor, Installer installer)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("remove_supervisor_from_installer", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@installer_id", installer.Id);
            command.Parameters.AddWithValue("@supervisor_id", supervisor.Id);

            return await command.ExecuteNonQueryAsync();

        }

        public async Task<int> UpdateSupervisorAsync(Supervisor supervisor)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            int rowsAffected = 0;
            using var commandEmail = new SqlCommand("update_supervisor_email", connection);

            commandEmail.CommandType = CommandType.StoredProcedure;
            commandEmail.Parameters.AddWithValue("@id", supervisor.Id);
            commandEmail.Parameters.AddWithValue("@email", supervisor.Email);

            rowsAffected += await commandEmail.ExecuteNonQueryAsync();

            using var commandPhone = new SqlCommand("update_supervisor_phone", connection);
            commandPhone.CommandType = CommandType.StoredProcedure;
            commandPhone.Parameters.AddWithValue("@id", supervisor.Id);
            commandPhone.Parameters.AddWithValue("@phone", supervisor.Phone);

            rowsAffected += await commandPhone.ExecuteNonQueryAsync();

            return rowsAffected;
        }
    }
}
