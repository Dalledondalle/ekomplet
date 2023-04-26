using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ekomplet.Domain.Models
{
    

    public class Employee
    {
        private Guid id;
        private string firstname = string.Empty;
        private string lastname = string.Empty;
        private string phone = string.Empty;
        private string email = string.Empty;

        public string NameMail => $"{firstname} {lastname} -- {email}";

        public Guid Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Firstname
        {
            get
            {
                if (firstname == null) firstname = String.Empty;
                return firstname;
            }
            set
            {
                if (value == null) firstname = String.Empty;
                else firstname = value;
            }
        }
        public string Lastname
        {
            get
            {
                if (lastname == null) lastname = String.Empty;
                return lastname;
            }

            set
            {
                if (value == null) lastname = String.Empty;
                else lastname = value;
            }
        }
        public string Phone
        {
            get
            {
                if (phone == null) phone = String.Empty;
                return phone;
            }

            set
            {
                if (value == null) phone = String.Empty;
                else phone = value;
            }
        }
        public string Email
        {
            get
            {
                if (email == null) email = String.Empty;
                return email;
            }

            set
            {
                if (value == null) email = String.Empty;
                else email = value;
            }
        }
        public Role Role { get; set; }

        public bool Exists()
        {
            if(id != new Guid()) return true;
            return false;
        }

        public static bool operator == (Employee e1, Employee e2)
        {
            return e1.id == e2.id;
        }

        public static bool operator !=(Employee e1, Employee e2)
        {
            return e1.id == e2.id;
        }
    }

    public enum Role
    {
        Installer, Supervisor
    }
}
