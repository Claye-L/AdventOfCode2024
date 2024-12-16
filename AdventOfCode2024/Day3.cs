using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day3
    {
        private string _input;
        public Day3()
        {
            _input = string.Concat(Helper.GetInput(3));
        }

        public void Part1()
        {
            var regex = new Regex("mul\\((\\d{1,3}),(\\d{1,3})\\)");
            var matches = regex.Matches(_input);
            var result = matches.Select(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value)).Sum();
            Console.WriteLine(result);
        }

        public void Part2()
        {
            var regex = new Regex("(?'mul'mul\\((?'n1'\\d{1,3}),(?'n2'\\d{1,3})\\))|(?'do'do\\(\\))|(?'dont'don't\\(\\))");
            var matches = regex.Matches(_input);
            bool mul_enabled = true;
            int result = 0;
            foreach (Match m in matches)
            {
                if (m.Groups["do"].Success)
                    mul_enabled = true;
                else if (m.Groups["dont"].Success)
                    mul_enabled = false;
                else if (m.Groups["mul"].Success && mul_enabled)
                    result += int.Parse(m.Groups["n1"].Value) * int.Parse(m.Groups["n2"].Value);
            }

            Console.WriteLine(result);
        }
    }
}
