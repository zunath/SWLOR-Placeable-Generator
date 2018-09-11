using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateSWLORPlaceables
{
    class Program
    {
        static void Main(string[] args)
        {

            const string NameFilter = "[mdrn]";
            string[] lines = File.ReadAllLines("./placeables.2da");
            byte[] template = File.ReadAllBytes("./template.utp");
            
            List<Tuple<string, int>> records = new List<Tuple<string, int>>();

            for(int x = 2; x <= lines.Length-1; x++)
            {
                var line = lines[x];
                string name = line.Substring(8, 100);
                name = name.Replace("\"", "");
                name = name.Trim();

                if (name.Contains("****")) continue;
                if (!name.Contains(NameFilter)) continue;

                records.Add(new Tuple<string, int>(name, x));

            }

            if (!Directory.Exists("./Output"))
            {
                Directory.CreateDirectory("./Output");
            }

            foreach (var file in Directory.GetFiles("./Output"))
            {
                File.Delete(file);
            }

            int id = 487; // start at 487 since the first batch stopped there
            foreach (var record in records)
            {
                string name = record.Item1;
                int index = record.Item2;
                string paddedName = name.PadRight(85);
                string tag = "swlor_" + (id.ToString().PadLeft(4, '0'));
                byte[] outputData = new byte[template.Length];
                template.CopyTo(outputData, 0);
                
                // Fix the name
                for (int x = 1586; x <= 1670; x++)
                {
                    outputData[x] = (byte) paddedName[x - 1586];
                    Console.Write((char)outputData[x]);
                }
                Console.WriteLine();
                Console.WriteLine("[MDRN] XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");


                // Fix the tag
                for (int x = 1684; x <= 1693; x++)
                {
                    outputData[x] = (byte)tag[x - 1684];
                    Console.Write((char)outputData[x]);
                }
                Console.WriteLine();
                Console.WriteLine("swlor_xxxx");

                // Fix the appearance
                byte[] appearanceBytes = BitConverter.GetBytes(index);
                outputData[364] = appearanceBytes[0];
                outputData[365] = appearanceBytes[1];
                outputData[366] = appearanceBytes[2];
                outputData[367] = appearanceBytes[3];



                File.WriteAllBytes("./output/swlor_" + id.ToString().PadLeft(4, '0') + ".utp", outputData);

                id++;
            }


            Console.WriteLine("Press a key to end");
            Console.ReadKey();
        }
    }
}
