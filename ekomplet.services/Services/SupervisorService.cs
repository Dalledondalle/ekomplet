using System.Data;
using System.Data.SqlClient;

namespace ekomplet.services
{
    public class SupervisorService : ISupervisorRepository
    {
        private readonly string connectionString;
        public SupervisorService(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<int> CreateSupervisor(Supervisor supervisor)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("new_supervisor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@firstname", supervisor.Firstname);
                    command.Parameters.AddWithValue("@lastname", supervisor.Lastname);
                    command.Parameters.AddWithValue("@phone", supervisor.Phone);
                    command.Parameters.AddWithValue("@email", supervisor.Email);

                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> DeleteSupervisor(Guid id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("delete_supervisor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);

                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> DeleteSupervisor(Supervisor supervisor)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("delete_supervisor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", supervisor.Id);

                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Supervisor>> GetAllSupervisor()
        {
            var supervisors = new List<Supervisor>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand(
                    "SELECT supervisor_id, firstname, lastname, phone_number, email " +
                    "FROM supervisors",
                    connection);

                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    supervisors.Add(new Supervisor(reader));
                }
            }

            return supervisors;
        }

        public async Task<Supervisor> GetSupervisorByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("get_supervisor_email", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@email", email);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Supervisor(reader);
                        }
                    }
                }
            }
            return new();
        }

        public async Task<Supervisor> GetSupervisorById(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("get_supervisor_id", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id.ToString());

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Supervisor(reader);
                        }
                    }
                }
            }

            return new();
        }

        public async Task<Supervisor> GetSupervisorByPhone(string phone)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("get_supervisor_phone", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@phone", phone);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Supervisor(reader);
                        }
                    }
                }
            }
            return new();
        }

        public async Task<List<Supervisor>> GetSupervisorsByInstaller(Installer installer)
        {
            var supervisors = new List<Supervisor>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("get_supervisors_by_installers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", installer.Id);
                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        supervisors.Add(new Supervisor(reader));

                    }

                }
            }

            return supervisors;
        }

        public async Task<int> RemoveInstallerFromSupervisor(Supervisor supervisor, Installer installer)
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

        public async Task<int> UpdateSupervisor(Supervisor supervisor)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                int rowsAffected = 0;
                using (var command = new SqlCommand("update_supervisor_email", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", supervisor.Id);
                    command.Parameters.AddWithValue("@email", supervisor.Email);

                    rowsAffected += await command.ExecuteNonQueryAsync();
                }

                using (var command = new SqlCommand("update_supervisor_phone", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", supervisor.Id);
                    command.Parameters.AddWithValue("@phone", supervisor.Phone);

                    rowsAffected += await command.ExecuteNonQueryAsync();
                }
                return rowsAffected;
            }
        }
    }
}
