using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class MailProvider
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public List<int> Ports { get; set; }
        public bool DefaultSsl { get; set; }
        public override string ToString() => Name;
    }
}
