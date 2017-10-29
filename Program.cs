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

            Input inputData = new Input("map2.txt");
            Map map = inputData.GenerateMap();
            startGame(map);

            //map.ToString();
            Console.Read();

        }


        public static void startGame(Map startMap)
        {
            //Breitensuche!
            //map.ToString();
            List<Map> maps = new List<Map>();
            maps.Add(startMap);
            
            while(maps.Count > 0)
            {
                Map map = maps.First();
                Robot r = new Robot(map);
                foreach (Route route in r.GetRoutes())
                {
                    Console.WriteLine(route.ToString());
                    maps.Add(map.executeRoute(route));
                    Console.Read();
                }
                //Console.WriteLine(map.ToString());
                
                maps.RemoveAt(0);
            }
            // Each turn:
            // Robot on the new Position: Print Chosen Route
            // Diamond on the new Position
            // Check if Game is won
        }

        
    }
}
