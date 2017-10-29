using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanSolver
{
    public enum FieldType { Walkable, Diamond, Robot, Goal, Unwalkable };

    class Map
    {


        public int[,] map;
        PointOnMap[] diamonds;
        PointOnMap robotPosition;



        /// <summary>
        /// Constructor of Map 
        /// </summary>
        public Map(int mapWidth, int mapHeight, int inputDiamonds, String[] mapString)
        {
            map = new int[mapHeight, mapWidth]; // mapHeight = rows     mapWidth = cols
            diamonds = new PointOnMap[inputDiamonds];


            int x = 0, y = 0, diamondCounter = 0;
            foreach (string row in mapString){
                foreach(char field in row)
                {
                    switch (field)
                    {
                        case '.':
                            map[x,y] = (int) FieldType.Walkable;
                            break;
                        case 'J':
                            map[x, y] = (int)FieldType.Diamond;
                            diamonds[diamondCounter] = new PointOnMap(x, y, FieldType.Diamond);
                            diamondCounter++;
                            break;
                        case 'M':
                            map[x, y] = (int)FieldType.Robot;
                            robotPosition = new PointOnMap(x,y, FieldType.Robot);
                            break;
                        case 'G':
                            map[x, y] = (int)FieldType.Goal;
                            break;
                        case 'X':
                            map[x, y] = (int)FieldType.Unwalkable;
                            break;
                        case ' ':
                            map[x, y] = (int)FieldType.Unwalkable;
                            break;
                        default:
                            Console.Error.WriteLine("Faulty Input Letter: " + field);
                            Environment.Exit(1);
                            Console.Read();
                            break;
                    }
                    y++;
                }
                y = 0;
                x++;
            }


            
        }

        public void startGame()
        {
            //Breitensuche!

            Robot r = new Robot(this, robotPosition, diamonds);
            
            // Each turn:
            // Robot on the new Position: Print Chosen Route
            // Diamond on the new Position
            // Check if Game is won
        }

        public override string ToString()
        {
            for(int i = 0; i < map.GetLength(0); i++){
                for(int j = 0; j < map.GetLength(1); j++)
                {
                    switch (map[i,j])
                    {
                        case (int) FieldType.Walkable:
                            Console.Write(".");
                            break;
                        case (int)FieldType.Diamond:
                            Console.Write("D");
                            break;
                        case (int)FieldType.Robot:
                            Console.Write("R");
                            break;
                        case (int)FieldType.Goal:
                            Console.Write("G");
                            break;
                        case (int)FieldType.Unwalkable:
                            Console.Write("-");
                            break;
                        default:
                            Console.Error.WriteLine("Something went wrong");
                            Console.Read();
                            break;
                    }
                }
                Console.WriteLine();
            }
            return base.ToString();
        }


        public PointOnMap GetElementNorth(PointOnMap current)
        {
            return new PointOnMap(current.Row - 1, current.Column, (FieldType) map[current.Row - 1, current.Column]);
        }
        public PointOnMap GetElementEast(PointOnMap current)
        {
            return new PointOnMap(current.Row, current.Column + 1, (FieldType)map[current.Row, current.Column + 1]);
        }
        public PointOnMap GetElementSouth(PointOnMap current)
        {
            return new PointOnMap(current.Row + 1, current.Column, (FieldType)map[current.Row + 1, current.Column]);
        }
        public PointOnMap GetElementWest(PointOnMap current)
        {
            return new PointOnMap(current.Row, current.Column - 1, (FieldType)map[current.Row, current.Column - 1]);
        }

    }
}
