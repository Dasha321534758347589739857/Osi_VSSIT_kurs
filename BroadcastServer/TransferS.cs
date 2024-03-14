using Data;
using System.Text;
using System.Text.Json;




namespace TransferServer
{
    public class TransferS
    {
        public ManyParameters AppDecoder(string info)
        {
            return JsonSerializer.Deserialize<ManyParameters>(info);
        }

        public string AppEncoder(ManyParameters Info,object selectedMethod,  double step)
        {
            var builder = new StringBuilder();
            
            builder.Append(JsonSerializer.Serialize(step) + '@');
            builder.Append(JsonSerializer.Serialize(selectedMethod) + '$');
            builder.Append(JsonSerializer.Serialize(Info));
            return builder.ToString();
        }

        public (double,object ,  ManyParameters) ServerDecoder(string info)
        {
            
            double step = 0;
            object selectedMethod = 0;
            var builder = new StringBuilder();
            var points = new List<InputParameters>();
            foreach (var elem in info)
               if (elem == '@')
                {
                    step = JsonSerializer.Deserialize<double>(builder.ToString());
                    builder.Clear();
                }
            else if (elem == '$')
            {
                selectedMethod = JsonSerializer.Deserialize<object>(builder.ToString());
                builder.Clear();
            }
            else
                {
                    builder.Append(elem);
                }

            return (step, selectedMethod, JsonSerializer.Deserialize<ManyParameters>(builder.ToString()));
        }

        public string ServerEncoder(ManyParameters Info)
        {
            return JsonSerializer.Serialize(Info);
        }
    }
}