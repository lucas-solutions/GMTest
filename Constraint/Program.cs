using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint
{
    class Program
    {
        static void Main(string[] args)
        {
            float weight = 0;
            var accesories = File.ReadAllLines("accesories.txt").Select(l => l.Split(' '))
                .Where(l => l.Length == 2 && float.TryParse(l[1], out weight))
                .Select(l => new Tuple<string, float>(l[0], weight))
                .ToArray();

            //Sort items, put the heaviest first
            Array.Sort(accesories, (Comparison<Tuple<string, float>>)((Tuple<string, float> a, Tuple<string, float> b) => b.Item2.CompareTo(a.Item2)));

            for (var cmd = "list"; cmd != "exit"; cmd = Console.ReadLine().ToLower())
            {
                switch (cmd)
                {
                    case "list":
                        Console.WriteLine();
                        Console.WriteLine("{0,-8}\t{1,10}", "Accesory", "Weight");
                        foreach (var item in accesories)
                        {
                            Console.WriteLine("{0,8}\t{1,10:#,##0.00}", item.Item1, item.Item2);
                        }
                        break;

                    default:
                        if (!float.TryParse(cmd, out weight))
                        {
                            Console.WriteLine("Invalid weght value");
                            continue;
                        }

                        var indexes = MaximizeWeight(accesories, weight);

                        Console.WriteLine();
                        Console.WriteLine("The best match is:");
                        Console.WriteLine("{0,-8}\t{1,10}", "Accesory", "Weight");
                        foreach (var index in indexes)
                        {
                            Console.WriteLine("{0,8}\t{1,10:#,##0.00}", accesories[index].Item1, accesories[index].Item2);
                        }

                        break;
                }
                Console.WriteLine();
                Console.Write("Enter command (list/exit) or maximum weight: ");
            }
        }

        static int[] MaximizeWeight(Tuple<string, float>[] items, float limit)
        {
            if (items == null || items.Length == 0)
                return new int[0];

            var current = new Stack<int>();
            Func<float> calcCurrentWeight = () => current.ToArray().Sum(i => items[i].Item2);

            var bestSet = new int[0];
            float bestWeight = 0;

            Action<int, float> constraint = null;
            
            constraint = (int index, float currentWeight) =>
                {
                    for (var i = index; i < items.Length; i++)
                    {   
                        var iWeight = items[i].Item2;
                        var totalWeight = currentWeight + iWeight;
                        if (totalWeight <= limit)
                        {
                            current.Push(i);

                            if (bestWeight < totalWeight)
                            {
                                bestSet = current.ToArray();
                                bestWeight = totalWeight;
                            }

                            // remove this if you want the best set with the larger amount of items
                            if (bestWeight == limit) return;

                            constraint(i + 1, totalWeight);

                            current.Pop();
                        }
                    }
                };

            constraint(0, 0);

            return bestSet;
        }
    }
}
