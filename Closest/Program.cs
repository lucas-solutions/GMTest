using KdTree.Math;
using KDTreeDLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Closest
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("dealers.txt").Select(l => l.Split(' ')).ToArray();

            //JavaKdTree(lines);
            NiceKdTree(lines);
        }

        static void JavaKdTree(string[][] lines)
        {
            var kdTree = new KDTree(2);

            double lon, lat;
            foreach (var line in lines)
            {
                if (line.Length == 3 && double.TryParse(line[1], out lon) && double.TryParse(line[2], out lat))
                {
                    kdTree.insert(new double[2] { lon, lat }, line[0]);
                }
            }

            string[] tokens;
            for (var cmd = "list"; cmd != "exit"; cmd = Console.ReadLine().ToLower())
            {
                switch (cmd)
                {
                    case "list":
                        Console.WriteLine();
                        Console.WriteLine("{0,-8}\t{1,10}\t{2,10}", "Dealer", "Longitude", "Latitude");
                        foreach (var line in lines)
                        {
                            Console.WriteLine("{0,8}\t{1,10:#,##0.00}\t{2,10:#,##0.00}", line[0], line[1], line[2]);
                        }
                        break;

                    default:
                        tokens = cmd.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                        if (tokens.Length == 0)
                        {
                            Console.WriteLine("Too few parameters. Expected one or two.");
                            continue;
                        }

                        if (tokens.Length == 1)
                        {
                            var match = lines.FirstOrDefault(l => string.Equals(l[0], tokens[0], StringComparison.OrdinalIgnoreCase));

                            if (match == null)
                            {
                                Console.WriteLine("Dealter {0} not Found.", tokens[0]);
                                continue;
                            }

                            tokens = new[] { match[1], match[2] };
                        }

                        if (tokens.Length > 2)
                        {
                            Console.WriteLine("Too many parameters. Expected one or two.");
                            continue;
                        }

                        if (!double.TryParse(tokens[0], out lon))
                        {
                            Console.WriteLine("Invalid Longitude");
                            continue;
                        }

                        if (!double.TryParse(tokens[1], out lat))
                        {
                            Console.WriteLine("Invalid Latitude");
                            continue;
                        }
                        {
                            object[] nodes;

                            try
                            {
                                nodes = kdTree.nearest(new double[] { lon, lat }, 2);
                                Console.WriteLine("The two nearest neighbors around ({0:#,##0.00}, {1:#,##0.00}) are:", lon, lat);
                                Console.WriteLine();
                                Console.WriteLine("{0,-8}\t{1,10}\t{2,10}", "Dealer", "Longitude", "Latitude");
                                foreach (string key in nodes)
                                {
                                    var value = lines.FirstOrDefault(l => string.Equals(l[0], key, StringComparison.OrdinalIgnoreCase));

                                    Console.WriteLine("{0,8}\t{1,10:#,##0.00}\t{2,10:#,##0.00}", key, value[1], value[2]);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Algoritm failed. {0}", e.Message);
                            }
                        }

                        break;
                }
                Console.WriteLine();
                Console.Write("Enter command (list/exit), dealer code, or coords: ");
            }
        }

        static void NiceKdTree(string[][] lines)
        {
            var kdTree = new KdTree.KdTree<float, string>(2, new FloatMath());

            float lon, lat;
            foreach (var line in lines)
            {
                if (line.Length == 3 && float.TryParse(line[1], out lon) && float.TryParse(line[2], out lat))
                {
                    kdTree.Add(new float[2] { lon, lat }, line[0]);
                }
            }

            kdTree.Balance();

            string[] tokens;
            for (var cmd = "list"; cmd != "exit"; cmd = Console.ReadLine().ToLower())
            {
                switch (cmd)
                {
                    case "list":
                        Console.WriteLine();
                        Console.WriteLine("{0,-8}\t{1,10}\t{2,10}", "Dealer", "Longitude", "Latitude");
                        foreach (var line in lines)
                        {
                            Console.WriteLine("{0,8}\t{1,10:#,##0.00}\t{2,10:#,##0.00}", line[0], line[1], line[2]);
                        }
                        break;

                    default:
                        tokens = cmd.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                        float[] point = null;

                        switch (tokens.Length)
                        {
                            case 0:
                                Console.WriteLine("Too few parameters. Expected one or two.");
                                break;

                            case 1:
                                if (!kdTree.TryFindValue(tokens[0], out point))
                                {
                                    Console.WriteLine("Dealer code not found.");
                                }
                                break;

                            case 2:
                                if (!float.TryParse(tokens[0], out lon))
                                {
                                    Console.WriteLine("Invalid Longitude");
                                }
                                else if (!float.TryParse(tokens[1], out lat))
                                {
                                    Console.WriteLine("Invalid Latitude");
                                }
                                else
                                {
                                    point = new float[] { lon, lat };
                                }
                                break;

                            default:
                                Console.WriteLine("Too many parameters. Expected one or two.");
                                break;
                        }

                        if (point != null)
                        {
                            var nodes = kdTree.GetNearestNeighbours(point, 2);

                            Console.WriteLine("The two nearest neighbors around ({0:#,##0.00}, {1:#,##0.00}) are:", point[0], point[1]);
                            Console.WriteLine();
                            Console.WriteLine("{0,-8}\t{1,10}\t{2,10}", "Dealer", "Longitude", "Latitude");
                            foreach (var node in nodes)
                            {
                                Console.WriteLine("{0,8}\t{1,10:#,##0.00}\t{2,10:#,##0.00}", node.Value, node.Point[0], node.Point[1]);
                            }
                        }

                        break;
                }
                Console.WriteLine();
                Console.Write("Enter command (list/exit), dealer code, or coords: ");
            }
        }
    }
}
