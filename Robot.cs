using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanSolver
{
    class Robot
    {
        private Map map;
        private PointOnMap position;
        private PointOnMap[] diamondPositions;

        public Robot(Map currentMap, PointOnMap currentPosition, PointOnMap[] currentDiamondPositions)
        {
            map = currentMap;
            position = currentPosition;
            position.order = 1;
            diamondPositions = currentDiamondPositions;
            List<Route> usefulRoutes = calculateUsefulRoutes(calculateShiftPoints(), calculatePossibleRobotPositions());
           // foreach (Route r in usefulRoutes)
            ///{
             //   Console.Write(r.ToString() + "----------\n\n");
            //}
            //List<Tuple<PointOnMap, PointOnMap>> possibleShiftPoints

            Route routeToBeExecuted = usefulRoutes.First();
            Console.Write(routeToBeExecuted.ToString());
            int newRobotPositionX = routeToBeExecuted.GetLast().Row;
            int newRobotPositionY = routeToBeExecuted.GetLast().Column;
            currentMap.map[newRobotPositionX, newRobotPositionY] = (int)FieldType.Robot;

            map.map[newRobotPositionX, newRobotPositionY] = (int)FieldType.Robot;
            PointOnMap newRobotPosition = new PointOnMap(newRobotPositionX, newRobotPositionY, (FieldType)map.map[newRobotPositionX, newRobotPositionY]);
            // update Diamond position
            switch (PointOnMap.DirectionOfNeighbor(map, currentPosition, newRobotPosition))
            {
                case Direction.North:
                    map.map[map.GetElementNorth(newRobotPosition).Row, map.GetElementNorth(newRobotPosition).Column] = (int)FieldType.Diamond;
                    break;
                case Direction.East:
                    map.map[map.GetElementEast(newRobotPosition).Row, map.GetElementEast(newRobotPosition).Column] = (int)FieldType.Diamond;
                    break;
                case Direction.South:
                    map.map[map.GetElementSouth(newRobotPosition).Row, map.GetElementSouth(newRobotPosition).Column] = (int)FieldType.Diamond;
                    break;
                case Direction.West:
                    map.map[map.GetElementWest(newRobotPosition).Row, map.GetElementWest(newRobotPosition).Column] = (int)FieldType.Diamond;
                    break;
                default:
                    Console.WriteLine("nicht gut!");
                    break;
            }

            map.map[currentPosition.Row, currentPosition.Column] = (int)FieldType.Walkable; // Problem: Reveiling a Goal Field!
            currentPosition = newRobotPosition;

            Console.WriteLine("Jetzt gehts rund!\n" + map.ToString());
        }


        private void breadthFirstSearch()
        {

        }




        public List<Route> calculateUsefulRoutes(List<Tuple<PointOnMap, PointOnMap>> possibleShiftPoints, int[,] movementMap)
        {
            List<Route> allUsefulRoutes = new List<Route>();
            //Console.WriteLine("Possible shift Positions!");
            foreach (Tuple<PointOnMap, PointOnMap> shiftPoint in possibleShiftPoints)
            {

                //Console.WriteLine(shiftPoint.Item1.Row + "|" + shiftPoint.Item1.Column);
                // Is it possible to move robot to several Shift Points? 
                if (movementMap[shiftPoint.Item1.Row, shiftPoint.Item1.Column] != 0)
                {
                    //Let's create a route from Robot to shift Position! The final position must be the diamond itself
                    // The diamond must move in the right direction!
                    // The route is equal to following the numbers backwards
                    Route routeToShiftPoint = new Route();
                    routeToShiftPoint.Append(shiftPoint.Item2);
                    routeToShiftPoint.Append(shiftPoint.Item1);

                    int toRobotCounter = movementMap[shiftPoint.Item1.Row, shiftPoint.Item1.Column];
                    PointOnMap current = shiftPoint.Item1;

                    while(toRobotCounter != 1)
                    {
                        toRobotCounter -= 1;
                        if(movementMap[map.GetElementNorth(current).Row, map.GetElementNorth(current).Column] == 
                            toRobotCounter)
                        {
                            current = map.GetElementNorth(current);
                            routeToShiftPoint.Append(current);
                            continue;
                        }

                        if (movementMap[map.GetElementEast(current).Row, map.GetElementEast(current).Column] ==
                            toRobotCounter)
                        {
                            current = map.GetElementEast(current);
                            routeToShiftPoint.Append(current);
                            continue;
                        }

                        if (movementMap[map.GetElementSouth(current).Row, map.GetElementSouth(current).Column] ==
                            toRobotCounter)
                        {
                            current = map.GetElementSouth(current);
                            routeToShiftPoint.Append(current);
                            continue;
                        }

                        if (movementMap[map.GetElementWest(current).Row, map.GetElementWest(current).Column] ==
                            toRobotCounter)
                        {
                            current = map.GetElementWest(current);
                            routeToShiftPoint.Append(current);
                            continue;
                        }
                    }
                    allUsefulRoutes.Add(routeToShiftPoint.Reverse());
                }
            }
            return allUsefulRoutes;

        }

        public int[,] calculatePossibleRobotPositions()
        {
            int[,] movementMap = (int[,])this.map.map.Clone();  // Deep Copy
            Array.Clear(movementMap, 0, movementMap.Length);

            List<PointOnMap> stillToInspect = new List<PointOnMap>();
            stillToInspect.Add(position);

            while (stillToInspect.Count > 0)
            {
                PointOnMap currentlyInspectedPosition = stillToInspect[0];
                stillToInspect.RemoveAt(0);

                if((currentlyInspectedPosition.FieldType == FieldType.Robot ||
                   currentlyInspectedPosition.FieldType == FieldType.Goal ||
                   currentlyInspectedPosition.FieldType == FieldType.Walkable)
                   &&
                   (currentlyInspectedPosition.order < movementMap[currentlyInspectedPosition.Row, currentlyInspectedPosition.Column] ||
                   movementMap[currentlyInspectedPosition.Row, currentlyInspectedPosition.Column] == 0))
                {
                    movementMap[currentlyInspectedPosition.Row, currentlyInspectedPosition.Column] = currentlyInspectedPosition.order;

                    PointOnMap pNorth = map.GetElementNorth(currentlyInspectedPosition);
                    pNorth.order = currentlyInspectedPosition.order +1;
                    stillToInspect.Add(pNorth);

                    PointOnMap pEast = map.GetElementEast(currentlyInspectedPosition);
                    pEast.order = currentlyInspectedPosition.order + 1;
                    stillToInspect.Add(pEast);

                    PointOnMap pSouth = map.GetElementSouth(currentlyInspectedPosition);
                    pSouth.order = currentlyInspectedPosition.order + 1;
                    stillToInspect.Add(pSouth);

                    PointOnMap pWest = map.GetElementWest(currentlyInspectedPosition);
                    pWest.order = currentlyInspectedPosition.order + 1;
                    stillToInspect.Add(pWest);

                }
                
            }

            // Prints the map with distances to each reachable point
            for(int i = 0; i < movementMap.GetLength(0); i++)
            {
                for(int j = 0; j < movementMap.GetLength(1); j++)
                {
                        Console.Write(movementMap[i, j] + "\t");
                }
                Console.WriteLine();
            }
            return movementMap;
        }


        /// <summary>
        /// Searches for moveable diamonds and calculates the Positions where 
        /// the robot has to be in order to move a diamond
        /// </summary>
        /// <returns>Returns a List of Tuples where the first Element specifies the 
        /// robot position and the second Element represents the diamond that can be moved 
        /// </returns>
        public List<Tuple<PointOnMap,PointOnMap>> calculateShiftPoints()
        {
            List<Tuple<PointOnMap, PointOnMap>> shiftPoints = new List<Tuple<PointOnMap, PointOnMap>>();
            
            // Search for moveable diamonds
            // A diamond is moveable when two opposite fields around the diamond are walkable or Goal-fields
            foreach(PointOnMap diamond in diamondPositions)
            {
                // North-South Direction
                if((map.GetElementNorth(diamond).FieldType == FieldType.Walkable || map.GetElementNorth(diamond).FieldType == FieldType.Goal)
                    &&
                    (map.GetElementSouth(diamond).FieldType == FieldType.Walkable || map.GetElementSouth(diamond).FieldType == FieldType.Goal)
                    )
                {
                    shiftPoints.Add( new Tuple<PointOnMap, PointOnMap>( map.GetElementNorth(diamond), diamond));
                    shiftPoints.Add( new Tuple<PointOnMap, PointOnMap>( map.GetElementSouth(diamond), diamond));
                }
                // East-West Direction
                if ((map.GetElementEast(diamond).FieldType == FieldType.Walkable || map.GetElementEast(diamond).FieldType == FieldType.Goal)
                    &&
                    (map.GetElementWest(diamond).FieldType == FieldType.Walkable || map.GetElementWest(diamond).FieldType == FieldType.Goal)
                    )
                {
                    shiftPoints.Add(new Tuple<PointOnMap, PointOnMap>(map.GetElementEast(diamond), diamond));
                    shiftPoints.Add(new Tuple<PointOnMap, PointOnMap>(map.GetElementWest(diamond), diamond));
                }
            }
            return shiftPoints;
        }




    }
}
