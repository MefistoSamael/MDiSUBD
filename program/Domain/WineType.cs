using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class WineType
    {
        public int Id { get; set; }
        public bool Sparkling { get; set; }

        public int ColorId { get; set; }

        public int SweetnessId { get; set; }

        public Color Color { get; set; }

        public Sweetness Sweetness { get; set; }
    }
}
