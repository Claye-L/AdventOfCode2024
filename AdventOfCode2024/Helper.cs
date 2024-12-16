using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Helper
    {
        public static string[] GetInput(int day)
        {
            var sourceLocation = @"C:\Users\Claye_L\source\repos\AdventOfCode2024\AdventOfCode2024";
            return File.ReadAllLines(Path.Combine(sourceLocation, "Inputs", $"day{day}.txt"));
        }

        public static string ReadInputFile(int day)
        {
            var sourceLocation = @"C:\Users\Claye_L\source\repos\AdventOfCode2024\AdventOfCode2024";
            return File.ReadAllText(Path.Combine(sourceLocation, "Inputs", $"day{day}.txt"));
        }

        public static IEnumerable<T> Flatten<T>(this T[,] map)
        {
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    yield return map[row, col];
                }
            }
        }
        public static bool CheckBounds(int i, int j, int rows, int cols) => i >= 0 && j >= 0 && i < rows && j < cols;
        public static void Write2dArray<T>(T[,] map)
        {
            int l = map.GetLength(0);
            foreach(var ts in Flatten(map).Chunk(l))
            {
                Console.WriteLine(string.Concat(ts.Select(t => t.ToString())));
            }
        }
    }

    public static class Helper<T>
    {
        public static IEnumerable<T> Flatten(T[,] map)
        {
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    yield return map[row, col];
                }
            }
        }
        
    }
}
