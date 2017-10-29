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
        public PointOnMap[] diamonds;
        public PointOnMap[] goals;
        public PointOnMap robotPosition;

        public Route wholeRoute; 

        /// <summary>
        /// Constructor of Map 
        /// </summary>
        public Map(int mapWidth, int mapHeight, int inputDiamonds, int inputGoals, String[] mapString)
        {
            map = new int[mapHeight, mapWidth]; // mapHeight = rows     mapWidth = cols
            diamonds = new PointOnMap[inputDiamonds];
            goals = new PointOnMap[inputGoals];
            wholeRoute = new Route();
            
            int x = 0, y = 0, diamondCounter = 0, goalCounter = 0;
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
                            goals[goalCounter] = new PointOnMap(x, y, FieldType.Goal);
                            goalCounter++;
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


        public Map(int[,] inputMap, PointOnMap[] goals, PointOnMap[] diamonds, PointOnMap robotPosition, Route wholeRoute)
        {
            map = inputMap;
            this.goals = goals;
            this.diamonds = diamonds;
            this.robotPosition = robotPosition;
            this.wholeRoute = wholeRoute;
        }

        public Map executeRoute(Route routeToBeExecuted)
        {
            int[,] result = new int[map.GetLength(0),map.GetLength(1)];
            Array.Copy(map, result, map.Length);

            PointOnMap[] newDiamonds = new PointOnMap[diamonds.Length];
            if (diamonds.Length < goals.Length) { throw new Exception("What the fuck!"); } 
            int i = 0;
            foreach(PointOnMap diamond in diamonds)
            {
                newDiamonds[i] = new PointOnMap(diamond.Row, diamond.Column, (FieldType) map[diamond.Row, diamond.Column]);
                i++;
            }

            Map newMap = new Map(result, this.goals, newDiamonds, robotPosition, wholeRoute.Append(routeToBeExecuted));

            // Set new shift Position where the robot has to go in order to push the diamond
            int shiftPostionX = routeToBeExecuted.GetSecondLast().Row;
            int shiftPostionY = routeToBeExecuted.GetSecondLast().Column;
            PointOnMap shiftPosition = new PointOnMap(shiftPostionX, shiftPostionY, (FieldType)newMap.map[shiftPostionX, shiftPostionY]);

            // Set new robot Position where the robot will be after pushing forward the diamond
            int newRobotPositionX = routeToBeExecuted.GetLast().Row;
            int newRobotPositionY = routeToBeExecuted.GetLast().Column;

            // The robot is placed on position of diamond
            newMap.map[newRobotPositionX, newRobotPositionY] = (int)FieldType.Robot;
            PointOnMap newRobotPosition = new PointOnMap(newRobotPositionX, newRobotPositionY, (FieldType)newMap.map[newRobotPositionX, newRobotPositionY]);


            
            // Diamond position has to be updated
            switch (PointOnMap.DirectionOfNeighbor(newMap, shiftPosition, newRobotPosition))
            {
                case Direction.North:
                    newMap.map[GetElementNorth(newRobotPosition).Row, GetElementNorth(newRobotPosition).Column] = (int)FieldType.Diamond;
                    break;
                case Direction.East:
                    newMap.map[GetElementEast(newRobotPosition).Row, GetElementEast(newRobotPosition).Column] = (int)FieldType.Diamond;
                    break;
                case Direction.South:
                    newMap.map[GetElementSouth(newRobotPosition).Row, GetElementSouth(newRobotPosition).Column] = (int)FieldType.Diamond;
                    break;
                case Direction.West:
                    newMap.map[GetElementWest(newRobotPosition).Row, GetElementWest(newRobotPosition).Column] = (int)FieldType.Diamond;
                    break;
                default:
                    Console.WriteLine("nicht gut!");
                    throw new Exception("Lost a diamond!");
                    break;
            }

            // Set old position of robot to walkable 
            newMap.map[robotPosition.Row, robotPosition.Column] = (int)FieldType.Walkable;
            // if old robot-position was goal, set to goal instead of walkable
            foreach (PointOnMap goal in goals)
            {
                if(robotPosition == goal)
                {
                    newMap.map[robotPosition.Row, robotPosition.Column] = (int)FieldType.Goal;
                }
            }

            newMap.robotPosition = newRobotPosition;
            newMap.searchDiamonds(); // Update Diamondpositions!
            return newMap;
        }

        public bool isWon()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if ((FieldType)map[i, j] == FieldType.Goal) return false;
                }
            }
                    return true;
        }

        private void searchDiamonds()
        {
            Console.WriteLine("Diamonds: ");
            int diamondCounter = 0;
            for(int i = 0; i < map.GetLength(0); i++)
            {
                for(int j = 0; j < map.GetLength(1); j++)
                {
                    if((FieldType)map[i,j] == FieldType.Diamond)
                    {
                        diamonds[diamondCounter] = new PointOnMap(i,j,(FieldType) map[i,j]);
                        Console.WriteLine(diamonds[diamondCounter].ToString() + " ");
                        diamondCounter++;
                        
                    }
                }
            }
        }

        public override string ToString()
        {
            string returnString = "";
            for(int i = 0; i < map.GetLength(0); i++){
                for(int j = 0; j < map.GetLength(1); j++)
                {
                    switch (map[i,j])
                    {
                        case (int) FieldType.Walkable:
                            returnString+=".";
                            break;
                        case (int)FieldType.Diamond:
                            returnString += "D";
                            break;
                        case (int)FieldType.Robot:
                            returnString += "R";
                            break;
                        case (int)FieldType.Goal:
                            returnString += "G";
                            break;
                        case (int)FieldType.Unwalkable:
                            returnString += "-";
                            break;
                        default:
                            Console.Error.WriteLine("Something went wrong");
                            Console.Read();
                            break;
                    }
                }
                returnString += "\n";
            }
            return returnString;
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
