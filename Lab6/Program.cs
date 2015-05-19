using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Console;

namespace Lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            int[][] matrix = File.ReadAllLines("matrix.txt").Select(l => l.Split(' ').Select(int.Parse).ToArray()).ToArray(); // Reads the matrix from file matrix.txt in debug folder
            Vector2 start = new Vector2() { X = 0, Y = 0 };
            Vector2 end = new Vector2() { X = 16, Y = 16 };

            var result = Astar(start, end, matrix);

            if (result.Any())
            {
                WriteLine("Path:");
                foreach (var res in result)
                    WriteLine($" X={res.X,2}, Y = {res.Y}");
            }
            else
                WriteLine("No path found!");

            ReadKey();
        }

        static LinkedList<Vector2> Astar(Vector2 start, Vector2 end, int[][] matrix)
        {
            Node startNode = new Node() { position = start, cameFrom = null, DistFromStart = 0, HeuristicEstimate = calcHeuristic(start, end) };
            LinkedList<Node> openSet = new LinkedList<Node>();
            openSet.AddLast(startNode);
            LinkedList<Node> closedSet = new LinkedList<Node>();

            while(openSet.Any())
            {
                Node current = openSet.OrderBy(node => node.FullPathEstimate).First();

                if (Vector2.areEqual(current.position,end))
                    return GetPathFromNode(current);

                closedSet.AddLast(current);
                openSet.Remove(current);

                foreach(var neighbour in getNeighbours(current, end, ref matrix))
                {
                    var openNode = openSet.FirstOrDefault(node => Vector2.areEqual(node.position, neighbour.position));
                    if (openNode == null)
                        openSet.AddLast(neighbour);
                    else if(openNode.DistFromStart > neighbour.DistFromStart)
                    {
                        openNode.cameFrom = current;
                        openNode.DistFromStart = neighbour.DistFromStart;
                    }
                }
            }
            return new LinkedList<Vector2>();
        }

        private static LinkedList<Node> getNeighbours(Node from, Vector2 end, ref int[][] matrix)
        {
            var result = new LinkedList<Node>();
            Vector2[] neighbours = new Vector2[4];

            neighbours[0] = new Vector2() { X = from.position.X + 1, Y = from.position.Y };
            neighbours[1] = new Vector2() { X = from.position.X - 1, Y = from.position.Y };
            neighbours[2] = new Vector2() { X = from.position.X, Y = from.position.Y + 1 };
            neighbours[3] = new Vector2() { X = from.position.X, Y = from.position.Y - 1 };
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

        private static LinkedList<Vector2> GetPathFromNode(Node node)
        {
            var result = new LinkedList<Vector2>();
            var current = node;
            while(current !=null)
            {
                result.AddFirst(current.position);
                current = current.cameFrom;
            }
            return result;
        }

        static int calcHeuristic(Vector2 from, Vector2 to) => Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);

        private class Node
        {
            public Vector2 position;
            public Node cameFrom;
            public int DistFromStart;
            public int HeuristicEstimate;
            public int FullPathEstimate => DistFromStart + HeuristicEstimate;
        }

        public struct Vector2
        {
            public int X;
            public int Y;

            public static bool areEqual(Vector2 left, Vector2 right) => left.X == right.X && left.Y == right.Y;
        }
    }
}