using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanSolver
{
    class Program
    {
        static void Main(string[] args)
        {

            Input inputData = new Input("map1.txt");
            Map map = inputData.GenerateMap();

            map.startGame();
            map.ToString();
            Console.Read();



        }
    }
}
