using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory
{
    class Stat
    {
        public string name { get; set; }
        public int clicks { get; set; }
        public TimeSpan time { get; set; }
    }

    public class StatsContext : DbContext
    {
        public virtual DbSet<Stat> fours { get; set; }
        public virtual DbSet<Stat> fives { get; set; }
    }
}
