using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ekomplet.Domain.Models
{
    public class Installer : Employee
    {
        private List<Supervisor> supervisors = new();
        private Guid supervisorId;

        public List<Supervisor> Supervisors
        {
            get
            {
                if (supervisors == null) new List<Supervisor>();
                return supervisors;
            }
            set
            {
                if (value == null) supervisors = new List<Supervisor>();
                else supervisors = value;
            }
        }

        public Guid SupervisorId
        {
            get
            {
                return supervisorId;
            }

            set
            {
                supervisorId = value;
            }
        }

        public Installer()
        {
            supervisors = new List<Supervisor>();
        }

        public Installer(SqlDataReader reader)
        {

            Id = new Guid(reader.GetString(reader.GetOrdinal("installer_id")));
            Firstname = reader.GetString(reader.GetOrdinal("firstname"));
            Lastname = reader.GetString(reader.GetOrdinal("lastname"));
            Phone = reader.GetString(reader.GetOrdinal("phone_number"));
            Email = reader.GetString(reader.GetOrdinal("email"));
            SupervisorId = new Guid(reader.GetString(reader.GetOrdinal("supervisor_id")));
            Role = Role.Installer;
        }
    }
}
