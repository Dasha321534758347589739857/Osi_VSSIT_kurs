using System.Collections.ObjectModel;

namespace Data
{
    
        public class InputParameters
        {
            public InputParameters()
            {
                this.Level = 0;
                this.Concentration = 0;
                this.Expenditure = 0;
            }
            public InputParameters(double expenditure, double level, double concentration)
            {
                this.Level = level;
                this.Concentration = concentration;
                this.Expenditure = expenditure;
            }
            public double Expenditure { get; set; }
            public double Level { get; set; }
            public double Concentration { get; set; }

        }
    public class ManyParameters
    {
       
            public ManyParameters(ObservableCollection<InputParameters> parameters)
            {
                Expenditures = new List<double>();
                Levels = new List<double>();
                Concentrations = new List<double>();
                foreach (var elem in parameters)
                {
                    Expenditures.Add(elem.Expenditure);
                    Levels.Add(elem.Level);
                    Concentrations.Add(elem.Concentration);
                }

            }
            public List<double> Expenditures { get; set; }
            public List<double> Levels { get; set; }
            public List<double> Concentrations { get; set; }
            public ManyParameters(){}
        }
    }