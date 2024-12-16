using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day7
    {
        private string[] _input;
        public Day7()
        {
            _input = Helper.GetInput(7);
            //_input = "190: 10 19\r\n3267: 81 40 27\r\n83: 17 5\r\n156: 15 6\r\n7290: 6 8 6 15\r\n161011: 16 10 13\r\n192: 17 8 14\r\n21037: 9 7 18 13\r\n292: 11 6 16 20".Split("\r\n");
            //_input = "3267: 81 40 27\r\n7290: 6 8 6 15\r\n161011: 16 10 13\r\n192: 17 8 14\r\n21037: 9 7 18 13\r\n292: 11 6 16 20".Split("\r\n");
            var testValues = _input.Select(l =>
            {
                var ls = l.Split(':');
                return new TestValue { Goal = long.Parse(ls[0]), Values = string.Concat(ls.Skip(1)).Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).Select(long.Parse).ToArray() };
            }).ToList();
            _alltvs = testValues;
        }
        private class TestValue
        {
            public long Goal { get; set; }
            public long[] Values {  get; set; } 
        }

        private bool TestBranch(TestValue tv, int operations)
        {
            var index = 0;
            var total = tv.Values[0];
            var pow = (int)Math.Pow(2, index);
            foreach (var v in tv.Values.Skip(1)) {
                var opcode = operations & pow;
                total = opcode == 0 ? total * v : total + v;
                if (total > tv.Goal)
                    return false;
                index++;
                pow *= 2;
            }
            return tv.Goal == total;
        }
        private long Concatenate(long a, long b)
        {
            return long.Parse(a.ToString() + b.ToString());
        }

        private bool TestBranchTernary(TestValue tv, int operations)
        {
            var index = 0;
            var total = tv.Values[0];
            var pow = (int)Math.Pow(3, index);
            foreach (var v in tv.Values.Skip(1))
            {
                var opcode = operations & pow;
                total = opcode == 0 ? total * v : total + v;
                if (total > tv.Goal)
                    return false;
                index++;
                pow *= 3;
            }
            return tv.Goal == total;
        }
        List<TestValue> _notsolvable = new List<TestValue>();
        List<TestValue> _alltvs = new List<TestValue>();
        public void Part1()
        {
            //parsing
           
            var solvable =  new List<TestValue>();
            int i = 0;
            foreach (var tv in _alltvs)
            {
                int intervalNumber = tv.Values.Length - 1;
                if(Enumerable.Range(0, (int)Math.Pow(2,intervalNumber)).Any(op => TestBranch(tv, op)))
                {
                    solvable.Add(tv);
                }
                else
                {
                    _notsolvable.Add(tv);
                }
                i++;
                if (i%100 == 0)
                {
                    Console.WriteLine("Processed " + i);
                }
            }
            Console.WriteLine(solvable.Sum(x => x.Goal));
        }
        enum Op
        {
            Mul, Add, Concat
        }

        private bool DoOperation(long[] values, long goal, long total,  int depth, Op op)
        {
            if (depth == values.Length )
            {
                return total == goal;
            }
            var v = values[depth];
            var t = total;
            switch (op)
            {
                case Op.Mul:
                    t = total* v;
                    break;
                case Op.Add:
                    t = total + v;
                    break;
                case Op.Concat:
                    t = Concatenate(total, v);
                    break;
            }
            return DoOperation(values, goal, t, depth + 1, Op.Mul) || DoOperation(values, goal, t, depth + 1, Op.Add) || DoOperation(values, goal, t, depth + 1, Op.Concat);
        }

        public void Part2()
        {
            var testValues = _alltvs;
            var solvable = new List<TestValue>();
            int i = 0;

            foreach (var tv in testValues)
            {
                if(DoOperation(tv.Values, tv.Goal, tv.Values[0], 1, Op.Mul) || DoOperation(tv.Values, tv.Goal, tv.Values[0], 1, Op.Add) || DoOperation(tv.Values, tv.Goal, tv.Values[0], 1, Op.Concat))
                    solvable.Add(tv);
                else
                    _notsolvable.Add(tv);

                i++;
                if (i % 100 == 0)
                {
                    Console.WriteLine("Processed " + i);
                }
            }

            Console.WriteLine(solvable.Sum(x => x.Goal));
        }
    }
}
