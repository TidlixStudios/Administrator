using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.GameData
{
    public class GameReader
    {
        public int Counting_CurrentNumber { get; set; }
        public ulong Counting_LastUserID { get; set; }

        public async Task ReadCounting()
        {
            using (StreamReader sr = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}/GameData/Counting.json"))
            {
                var json = await sr.ReadToEndAsync();

                CountingStructure data = JsonConvert.DeserializeObject<CountingStructure>(json);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                this.Counting_CurrentNumber = data.currentNumber;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                this.Counting_LastUserID = data.lastUserID;
            }
        }
        public async Task SetCounting(int nr = 0, ulong userID = 1203630363088519188)
        {
            using (StreamWriter sw = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory}/GameData/Counting.json"))
            {
                    string json = "{" +
                        $"\n  \"currentNumber\": {nr}," +
                        $"\n  \"lastUserID\": {userID}" +
                        "\n}";
                await sw.WriteAsync(json);
            }
        }


        internal class CountingStructure ()
        {
            public int currentNumber { get; set; }
            public ulong lastUserID { get; set; }
        }
    }
}
