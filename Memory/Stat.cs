using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory
{
    public class Stat
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int clicks { get; set; }
        public TimeSpan time { get; set; }
        public bool isFive { get; set; }
    }

    public class StatsContext : DbContext
    {
        public virtual DbSet<Stat> highscores { get; set; }
    }
}
