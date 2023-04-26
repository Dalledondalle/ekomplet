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
    public class InstallerRepository : IInstallerRepository
    {
        private readonly string connectionString;
        public InstallerRepository(IConfiguration config)
        {
            connectionString = config.GetConnectionString("LocalSQL")!;
        }

        public async Task<int> AddSupervisorAsync(Supervisor newSupervisor, Installer installer)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync();

            using var command = new SqlCommand("add_supervisor_to_installer", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@supervisor_id", newSupervisor.Id);
            command.Parameters.AddWithValue("@installer_id", installer.Id);
            return await command.ExecuteNonQueryAsync();
        }

        public async Task<int> CreateInstallerAsync(Installer installer)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("new_installer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@firstname", installer.Firstname);
                    command.Parameters.AddWithValue("@lastname", installer.Lastname);
                    command.Parameters.AddWithValue("@phone", installer.Phone);
                    command.Parameters.AddWithValue("@email", installer.Email);
                    command.Parameters.AddWithValue("@supervisor_id", installer.SupervisorId.ToString());

                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> DeleteInstallerAsync(Guid id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("delete_installer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> DeleteInstallerAsync(Installer installer)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("delete_installer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", installer.Id);

                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Installer>> GetAllInstallersAsync()
        {
            var installers = new List<Installer>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "SELECT installer_id, firstname, lastname, phone_number, email, supervisor_id " +
                    "FROM installers",
                    connection);

                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    installers.Add(new Installer(reader));
                }
            }

            return installers;
        }

        public async Task<Installer> GetInstallerByEmailAsync(string email)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("get_installer_email", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@email", email);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new(reader);
                        }
                    }
                }
            }

            return new();
        }

        public async Task<Installer> GetInstallerByIdAsync(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("get_installer_id", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id.ToString());

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Installer(reader);
                        }
                    }
                }
            }

            return new();
        }

        public async Task<Installer> GetInstallerByPhoneAsync(string phone)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("get_installer_phone", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@phone", phone);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Installer(reader);
                        }
                    }
                }
            }

            return new();
        }

        public async Task<List<Installer>> GetInstallersBySupervisorAsync(Supervisor supervisor)
        {
            var installers = new List<Installer>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("get_installers_by_supervisors", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", supervisor.Id);
                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        installers.Add(new Installer(reader));

                    }

                }
            }

            return installers;
        }

        public async Task<int> RemoveSupervisorFromInstallerAsync(Installer installer, Supervisor supervisor)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("remove_supervisor_from_installer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@installer_id", installer.Id);
                    command.Parameters.AddWithValue("@supervisor_id", supervisor.Id);

                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> UpdateInstallerAsync(Installer installer)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                int rowsAffected = 0;
                using (var command = new SqlCommand("update_installer_email", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", installer.Id);
                    command.Parameters.AddWithValue("@email", installer.Email);

                    rowsAffected += await command.ExecuteNonQueryAsync();
                }

                using (var command = new SqlCommand("update_installer_phone", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", installer.Id);
                    command.Parameters.AddWithValue("@phone", installer.Phone);

                    rowsAffected += await command.ExecuteNonQueryAsync();
                }
                return rowsAffected;
            }
        }
    }
}
