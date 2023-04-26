using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ekomplet.services
{
    public class Error
    {
        private string message;

        public string Message
        {
            get
            {
                if(message == null) message = string.Empty;
                return message;
            }
        }
        public Error(string message)
        {
            this.message = message;
        }

        public static Error PersonNotFound()
        {
            return new Error("Person ikke fundet. Gå tilbage og prøv igen");
        }

        public static Error UnknownError()
        {
            return new Error("Der er sket en fejl. Prøv igen. Hvis problemet fortsætter, kontakt support på tlf: xx yy zz jj");
        }
    }
}
