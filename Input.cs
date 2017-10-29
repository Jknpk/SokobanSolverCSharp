using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanSolver
{
    class Input
    {
        string inputFile; 

        public Input(string pathToTxt)
        {
            inputFile = pathToTxt;
            
        }

        
        /// <summary>
        /// This Method reads input File specified in inputFile, extracts first numbers like width, 
        /// height and amounts of Diamonds and calls the Map-Constructor
        /// </summary>
        /// <returns>Map-Object representing the whole map</returns>
        public Map GenerateMap()
        {
            String[] mapInput = null;
            try
            {
                mapInput = File.ReadLines(inputFile).ToArray();
            }
            catch (FileNotFoundException e) {
                Console.Error.WriteLine("Couldn't find the file: " + inputFile);
                Console.Read();
                Environment.Exit(1);
            }
            string numbers = mapInput.GetValue(0).ToString();
            mapInput = mapInput.Skip(1).ToArray();
            numbers.Split(' ').ToArray();

            int width = int.Parse(numbers.Split(' ').ToArray()[0]);
            int height = int.Parse(numbers.Split(' ').ToArray()[1]);
            int amountOfDiamonds = int.Parse(numbers.Split(' ').ToArray()[2]);

            Map map = new Map(width, height, amountOfDiamonds, mapInput);
            
            return map;
        }
    }
}
