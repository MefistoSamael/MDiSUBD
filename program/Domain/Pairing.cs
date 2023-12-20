using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Pairing
    {
        public int Id { get; set; }

        public string Description{ get; set; }

        public int WineId { get; set; }

        public Wine wine { get; set; }
    }
}
