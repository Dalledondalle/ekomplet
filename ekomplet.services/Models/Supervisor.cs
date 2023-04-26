using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ekomplet.services.Models
{
    public class Supervisor : Employee
    {
        public Guid InstallerId { get; set; }
        private List<Installer> installers = new();

        public List<Installer> Installers
        {
            get
            {
                if (installers == null) installers = new List<Installer>();
                return installers;
            }

            set
            {
                if (value == null) installers = new List<Installer>();
                else installers = value;
            }
        }
        public Supervisor()
        {
            InstallerId = new Guid();
        }
        public Supervisor(SqlDataReader reader)
        {
            Id = new Guid(reader.GetString(reader.GetOrdinal("supervisor_id")));
            Firstname = reader.GetString(reader.GetOrdinal("firstname"));
            Lastname = reader.GetString(reader.GetOrdinal("lastname"));
            Phone = reader.GetString(reader.GetOrdinal("phone_number"));
            Email = reader.GetString(reader.GetOrdinal("email"));
            Role = Role.Supervisor;
        }
    }
}
