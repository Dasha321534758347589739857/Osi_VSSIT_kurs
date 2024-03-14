namespace MenuMetods
{
    
        public class Linear
        {
        public Linear()
        {
         
        }
        public Linear(double a, double b)
            {
                this.a = a;
                this.b = b;
            }
            public double a { get; set; }
            public double b { get; set; }
        public override string ToString()
        {
            return "Линейная аппроксимация";
        }
    }
        public class Parabola
    {
        public Parabola()
        {

        }
        public Parabola(double a, double b, double c)
            {
                this.a = a;
                this.b = b;
                this.c = c;
            }
            public double a { get; set; }
            public double b { get; set; }
            public double c { get; set; }
        public override string ToString()
        {
            return "Параболическая аппроксимация";
        }
    }




    
}