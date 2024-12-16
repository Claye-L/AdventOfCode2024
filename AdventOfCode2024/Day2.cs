using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day2
    {
        private string[] _input;
        public Day2()
        {
            _input = Helper.GetInput(2);
        }

        public void Part1()
        {
            var reports = _input.Select(line => line.Split(' ').Select(n => int.Parse(n)).ToArray());
            var result = reports.Where(line => 
                            line.Zip(line.Skip(1)) 
                            .Select(t => Math.Sign(t.Second - t.First)).Distinct().Count() == 1)
                .Where(line => 
                            line.Zip(line.Skip(1))
                            .All(t => Math.Abs(t.First - t.Second) >= 1 && Math.Abs(t.First - t.Second) <= 3)).Count();
            Console.WriteLine(result);
        }

        public void Part2()
        {
            var reports = _input.Select(line => line.Split(' ').Select(n => int.Parse(n)).ToArray());
            var valids = new List<int[]>();
            foreach (var line in reports)
            {
                if(TestReport(line))
                {
                    valids.Add(line);
                }
                else
                {
                    bool valid = false;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (TestReport(line.Take(i).Concat(line.Skip(i + 1)).ToArray()))
                            valid = true;
                    }
                    if (valid)
                        valids.Add(line);
                }
            }

            Console.WriteLine(valids.Count);
        }

        private bool TestReport(int[] line)
        {
            return line.Zip(line.Skip(1)).Select(t => Math.Sign(t.Second - t.First)).Distinct().Count() == 1 &&
                    line.Zip(line.Skip(1)).All(t => Math.Abs(t.First - t.Second) >= 1 && Math.Abs(t.First - t.Second) <= 3);
        }

    }
}
