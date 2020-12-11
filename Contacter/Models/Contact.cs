using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contacter.Models
{
    public class Contact
    {
        public int ContactID { get; set; }
        public string ContactName { get; set; }
        public string ContactInfo { get; set; }
        public string ContactNote { get; set; }
    }
}
