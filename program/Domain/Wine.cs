using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Wine
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Photo { get; set; }
        
        public string Description{ get; set; }

        public int WineTypeId {  get; set; }

        public int VintageId { get; set; }

        public int WineryId { get; set; }

        public WineType WineType { get; set; }

        public Vintage Vintage { get; set; }

        public Winery Winery { get; set; }
    }
}
