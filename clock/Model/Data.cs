using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clock.Model
{
    public class Data
    {
        public Data(List<int> expenditure, List<double> level, List<double> concentration)
        {
            this.Expenditure = expenditure;
            this.Level = level;
            this.Concentration = concentration;
        }
        public List<int> Expenditure { get; set; }

        public List<double> Level { get; set; }

        public List<double> Concentration { get; set; }
    }
}
