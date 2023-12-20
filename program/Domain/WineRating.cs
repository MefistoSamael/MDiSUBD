using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class WineRating
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public int WineId { get; set; }

        public Wine? Wine {  get; set; } 
    }
}
