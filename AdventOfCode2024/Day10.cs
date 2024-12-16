using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day10
    {
        private int[][] _input;
        public Day10()
        {
            var rawinput = Helper.GetInput(10);
            //rawinput = "89010123\r\n78121874\r\n87430965\r\n96549874\r\n45678903\r\n32019012\r\n01329801\r\n10456732".Split("\r\n");
            _input = rawinput.Select(x => x.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
        }

        private bool CheckBounds(int i, int j) => i >= 0 && j >= 0 && i < _input.Length && j < _input[0].Length;

        private  List<(int i, int j)> SearchNeighbours(int i, int j, int target, List<(int i, int j)> list)
        {
            if(!CheckBounds(i, j))
                return list;
            var value = _input[i][j];
            if (value != target)
                return  list;
            else if (value == 9)
            {
                list.Add((i, j));
                return  list;
            }
            else
            {
                SearchNeighbours(i + 1, j, target + 1, list);
                SearchNeighbours(i - 1, j, target + 1, list);
                SearchNeighbours(i, j + 1, target + 1, list);
                SearchNeighbours(i, j - 1, target + 1, list);
                return list;
            }
        }

        public void Part1()
        {
            List<List<(int i, int j)>> trails = new List<List<(int i, int j)>>();
            for (int i = 0; i < _input.Length; i++)
            {
                for (int j = 0; j < _input[0].Length; j++)
                {
                    if (_input[i][j] == 0)
                    {
                        var nodes = new List<(int i, int j)>();
                        SearchNeighbours(i, j, 0, nodes);
                        trails.Add(nodes);
                    }
                }
            }
            var distinctTrails = trails.Select(n => n.Distinct().ToArray()).ToArray();
            Console.WriteLine(distinctTrails.Select(x => x.Count()).Sum());
        }

        public void Part2()
        {
            List<List<(int i, int j)>> trails = new List<List<(int i, int j)>>();
            for (int i = 0; i < _input.Length; i++)
            {
                for (int j = 0; j < _input[0].Length; j++)
                {
                    if (_input[i][j] == 0)
                    {
                        var nodes = new List<(int i, int j)>();
                        SearchNeighbours(i, j, 0, nodes);
                        trails.Add(nodes);
                    }
                }
            }
            var distinctTrails = trails.Select(n => n.ToArray()).ToArray();
            Console.WriteLine(distinctTrails.Select(x => x.Count()).Sum());
        }
    }
}
