using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode2024
{
    public class Day8
    {
        private char[][] _input;
        public Day8()
        {
            var rawinput = Helper.GetInput(8);
            //rawinput = "............\r\n........0...\r\n.....0......\r\n.......0....\r\n....0.......\r\n......A.....\r\n............\r\n............\r\n........A...\r\n.........A..\r\n............\r\n............".Split("\r\n");
            _input = rawinput.Select(x => x.ToArray()).ToArray();
        }
        private (int i, int j) ApplySpacing((int i1, int j1) item1, (int i2, int j2) item2) => (item1.i1 + item2.i2, item1.j1 + item2.j2);
        public void Part1()
        {
            var rows = _input.Length;
            var cols = _input[0].Length;
            //Group symbols with their coordinates
            var antennas = new Dictionary<char,List<(char symbol, int i, int j)>>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var symbol = _input[i][j];
                    if (symbol != '.')
                    {
                        if (antennas.TryGetValue(symbol, out var list))
                            list.Add((symbol, i, j));
                        else
                            antennas.Add(symbol, new List<(char symbol, int i, int j)>() {(symbol, i, j) });
                    } 
                    
                }
            }
            var nodes = new List<(int, int)>();
            //do pairwise comparisons
            foreach (var list in antennas.Values)
            {
                foreach (var item in list)
                {
                    foreach(var item2 in list.Where(x => x.i != item.i && x.j != item.j))
                    {
                        var spacing1 = (item.i - item2.i, item.j - item2.j);
                        var spacing2 = (item2.i -item.i, item2.j - item.j);
                        var node1 = ApplySpacing((spacing1.Item1, spacing1.Item2), (item.i, item.j));
                        var node2 = ApplySpacing((spacing2.Item1, spacing2.Item2), (item2.i, item2.j));
                        nodes.Add(node1);
                        nodes.Add(node2);
                    }
                }
            }
            var nodesFixed = nodes.Where(i => i.Item1 >= 0 && i.Item1 < rows && i.Item2 >= 0 && i.Item2 < cols).Distinct().ToList();
            Console.WriteLine(nodesFixed.Count);
        }
        private bool IsInBounds((int i, int j) point, int rows, int cols) => point.i >= 0 && point.i < rows && point.j >= 0 && point.j < cols;

        public void Part2()
        {
            var rows = _input.Length;
            var cols = _input[0].Length;
            //Group symbols with their coordinates
            var antennas = new Dictionary<char, List<(char symbol, int i, int j)>>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var symbol = _input[i][j];
                    if (symbol != '.')
                    {
                        if (antennas.TryGetValue(symbol, out var list))
                            list.Add((symbol, i, j));
                        else
                            antennas.Add(symbol, new List<(char symbol, int i, int j)>() { (symbol, i, j) });
                    }

                }
            }

            var nodes = new List<(int, int)>();
            //do pairwise comparisons
            foreach (var list in antennas.Values)
            {
                foreach (var item in list)
                {
                    foreach (var item2 in list.Where(x => x.i != item.i && x.j != item.j))
                    {
                        var spacing1 = (item.i - item2.i, item.j - item2.j);
                        var spacing2 = (item2.i - item.i, item2.j - item.j);
                        // while in bounds
                        var node1 = (item.i, item.j);
                        while (IsInBounds(node1, rows, cols))
                        {
                            nodes.Add(node1);
                            node1 = ApplySpacing(spacing1, node1);
                        }
                        var node2 = (item2.i, item2.j);
                        while (IsInBounds(node2, rows, cols))
                        {
                            nodes.Add(node2);
                            node2 = ApplySpacing(spacing2, node2);
                        }
                    }
                }
            }
            var nodesFixed = nodes.Where(i =>IsInBounds(i, rows, cols)).Distinct().ToList();
            Console.WriteLine(nodesFixed.Count);
        }
    }
}
