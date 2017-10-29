using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanSolver
{
    class Route
    {
        private List<PointOnMap> route;

        public Route()
        {
            route = new List<PointOnMap>();
        }


        public void Append(PointOnMap nextStep)
        {
            route.Add(nextStep);
        }

        public Route Append(Route r)
        {
            Route returnRoute = new Route();
            foreach(PointOnMap a in route)
            {
                returnRoute.Append(a);
            }
            foreach(PointOnMap b in r.route)
            {
                returnRoute.Append(b);
            }
            return returnRoute;
        }

        public override string ToString()
        {
            string returnString = "Route:\n";
            foreach (PointOnMap pointOnRoute in route)
            {
                returnString += pointOnRoute.ToString() + "\n";
            }
            return returnString;
        }

        public Route Reverse()
        {
            route.Reverse();
            return this;
        }

        public PointOnMap GetLast()
        {
            return route.Last();
        }
        public PointOnMap GetSecondLast()
        {
            return route.ElementAt(route.Count-2);
        }
    }
}
