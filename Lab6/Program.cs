using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        static void Astar(Node start, Node goal)
        {
            List<Node> closedSet = new List<Node>();
            List<Node> openSet = new List<Node>();
            openSet.Add(start);
            List<Node> cameFrom = new List<Node>();
            Dictionary<Node,int> gScore = new Dictionary<Node, int>();
            gScore[start] = 0;
            Dictionary<Node, int> fScore = new Dictionary<Node, int>();
            fScore[start] = gScore[start] + 

            while (openSet.Any())
            {
                var current =  
            }
            
        }
    }

    internal class Node
    {
    }
}
