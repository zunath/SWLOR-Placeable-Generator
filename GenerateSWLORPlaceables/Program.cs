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
            int start = 30000;

            string[] lines = File.ReadAllLines("./placeables.2da");
            byte[] template = File.ReadAllBytes("./template.utp");
            
            List<string> names = new List<string>();

            for(int x = start; x <= lines.Length-1; x++)
            {
                var line = lines[x];
                string name = line.Substring(8, 100);
                name = name.Replace("\"", "");
                name = name.Trim();

                if (name.Contains("****")) continue;

                names.Add(name);

            }

            if (!Directory.Exists("./Output"))
            {
                Directory.CreateDirectory("./Output");
            }

            foreach (var file in Directory.GetFiles("./Output"))
            {
                File.Delete(file);
            }

            int id = 1;
            foreach (var name in names)
            {
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
                Console.WriteLine("[SWLOR] XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");


                // Fix the tag
                for (int x = 1684; x <= 1693; x++)
                {
                    outputData[x] = (byte)tag[x - 1684];
                    Console.Write((char)outputData[x]);
                }
                Console.WriteLine();
                Console.WriteLine("swlor_xxxx");

                // Fix the appearance
                byte[] appearanceBytes = BitConverter.GetBytes(29999 + id);
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
