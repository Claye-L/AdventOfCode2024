using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    
    public class Day1
    {
        private string[] _input;
        public Day1() {
            _input = Helper.GetInput(1);
        }
        public void Part1()
        {
            var pairs = _input.Select(x => x.Split(' ').Select(p => int.Parse(p))).ToArray();
            var leftlist = pairs.Select(x => x.First()).Order().ToList();
            var rightlist = pairs.Select(x => x.Last()).Order().ToList();
            var result = leftlist.Zip(rightlist).Select(x => Math.Abs(x.First - x.Second)).Sum();
            Console.WriteLine(result);
        }

        public void Part2()
        {
            var pairs = _input.Select(x => x.Split(' ').Select(p => int.Parse(p))).ToArray();
            var leftlist = pairs.Select(x => x.First()).Order().ToList();
            var rightlist = pairs.Select(x => x.Last()).GroupBy(x => x).ToDictionary(x => x.Key);
            var result = 0;
            foreach (var n in leftlist )
            {
                if (rightlist.TryGetValue(n, out var group))
                {
                    result += group.Count() * n;
                }
            }
            Console.WriteLine(result);
        }
    }


}
