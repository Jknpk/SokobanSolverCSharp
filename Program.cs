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
            List<Map> alreadyVisitedMaps = new List<Map>();
            maps.Add(startMap);
            
            while(maps.Count > 0)
            {
                Map map = maps.First();
                maps.RemoveAt(0);
                alreadyVisitedMaps.Add(map);
                
                // Check if Map is impossible to solve 
                if (map.isImpossible()) continue;

                
                Robot r = new Robot(map);
                foreach (Route route in r.GetRoutes())
                {
                    //Console.WriteLine("\n\n------------------\nBefore:\n"+ map.ToString()); 
                    //Console.WriteLine(route.ToString());
                    Map newMap = map.executeRoute(route);
                    if (alreadyVisitedMaps.Contains(newMap))
                    {
                        
                        continue;
                    }
                    //Console.WriteLine("\n" + newMap.ToString()); //!!!!!!!!!!!
                    if (newMap.isWon())
                    {
                        //WON!
                        Console.WriteLine(newMap.wholeRoute.ToString());
                        throw new Exception("Won!");
                    }
                    maps.Add(newMap);
                    

                }
                //Console.WriteLine(map.ToString());
                

            }
            // Each turn:
            // Robot on the new Position: Print Chosen Route
            // Diamond on the new Position
            // Check if Game is won
        }

        
    }
}
