using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            int[][] matrix = File.ReadLines("matrix.txt").Select(l => l.Split(' ').Select(int.Parse).ToArray()).ToArray();
            
            Point start = new Point() { X = 0, Y = 0 };
            Point end = new Point() { X = 16, Y = 16 };

            var result = Astar(start, end, matrix);

            Console.WriteLine("Path:");
            foreach (var res in result)
                Console.WriteLine(res.X + " " + res.Y);
            
            Console.ReadKey();

        }

        static LinkedList<Point> Astar(Point start, Point end, int[][] matrix)
        {
            Node startNode = new Node() { position = start, cameFrom = null, DistFromStart = 0, HeuristicEstimate = calcHeuristic(start, end) };
            LinkedList<Node> openSet = new LinkedList<Node>();
            openSet.AddLast(startNode);
            LinkedList<Node> closedSet = new LinkedList<Node>();

            while(openSet.Any())
            {
                Node current = openSet.OrderBy(node => node.FullPathEstimate).First();

                if (current.position == end)
                    return GetPathFromNode(current);

                closedSet.AddLast(current);
                openSet.Remove(current);

                foreach(var neighbour in getNeighbours(current, end, ref matrix))
                {
                    if (closedSet.Count(node => node.position == neighbour.position) > 0)
                        continue;
                    var openNode = openSet.FirstOrDefault(node => node.position == neighbour.position);
                    if (openNode == null)
                        openSet.AddLast(neighbour);
                    else if(openNode.DistFromStart > neighbour.DistFromStart)
                    {
                        openNode.cameFrom = current;
                        openNode.DistFromStart = neighbour.DistFromStart;
                    }
                           
                }
            }


            return new LinkedList<Point>();
            
        }

        private static LinkedList<Node> getNeighbours(Node from, Point end, ref int[][] matrix)
        {
            var result = new LinkedList<Node>();

            Point[] neighbours = new Point[4];
            neighbours[0] = new Point() { X = from.position.X + 1, Y = from.position.Y };
            neighbours[1] = new Point() { X = from.position.X - 1, Y = from.position.Y };
            neighbours[2] = new Point() { X = from.position.X, Y = from.position.Y + 1 };
            neighbours[3] = new Point() { X = from.position.X, Y = from.position.Y - 1 };
            matrix[from.position.X][from.position.Y] = 0;

            foreach (var point in neighbours)
            {
                if (point.X < 0 || point.X >= matrix.Length)
                    continue;
                if (point.Y < 0 || point.Y >= matrix[0].Length)
                    continue;
                if ((matrix[point.X][point.Y] == 0))
                    continue;
                Node neighbour = new Node() { position = point, cameFrom = from, DistFromStart = from.DistFromStart + 1, HeuristicEstimate = calcHeuristic(point, end) };
                result.AddFirst(neighbour);
            }

            return result;

        }

        private static LinkedList<Point> GetPathFromNode(Node node)
        {
            var result = new LinkedList<Point>();
            var current = node;
            while(current !=null)
            {
                result.AddFirst(current.position);
                current = current.cameFrom;
            }
            return result;
        }

        static int calcHeuristic(Point from, Point to) => Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);

        private class Node
        {
            public Point position;
            public Node cameFrom;
            public int DistFromStart;
            public int HeuristicEstimate;
            public int FullPathEstimate => DistFromStart + HeuristicEstimate;
        }

        public struct Point
        {
            public int X;
            public int Y;
            
            public static bool operator ==(Point left, Point right)
            {
                return left.X == right.X && left.Y == right.Y;
            }
            public static bool operator !=(Point left, Point right)
            {
                return left.X != right.X || left.Y != right.Y;
            }
        }
    }
}