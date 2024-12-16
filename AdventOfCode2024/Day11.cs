using static System.Formats.Asn1.AsnWriter;

namespace AdventOfCode2024
{
    public class Day11
    {
        private string input;
        public Day11()
        {
            input = Helper.ReadInputFile(11);
            //input = "0 1 10 99 999";
            //input = "125 17";
        }

        public void Part1()
        {
            List<long> stones = input.Split(" ").Select(long.Parse).ToList();
            for (int i = 0; i < 25; i++) 
            {
                List<long> newstones = new List<long>(stones.Count * 2);
                foreach (long stone in stones)
                {
                    if (stone == 0)
                        newstones.Add(1);
                    else if (stone.ToString().Length % 2 == 0)
                    {
                        var s = stone.ToString();
                        var s1 = long.Parse(string.Concat(s.Take(s.Length / 2)));
                        var s2 = long.Parse(string.Concat(s.Skip(s.Length / 2)));
                        newstones.Add(s1);
                        newstones.Add(s2);
                    }
                    else
                    {
                        newstones.Add(stone * 2024L);
                    }
                }
                stones = newstones;
                //Console.WriteLine("processed " + i);
            }
            Console.WriteLine(stones.Count);
            //run time 136 ms
        }

        private struct StoneResult
        {
            public long Id;
            public bool Split;
            public long Next;
            public (long, long) NextSplit;
        }

        private void AddOrIncrement(Dictionary<long, long> stoneCounts, long key, long count)
        {
            if (stoneCounts.ContainsKey(key))
                stoneCounts[key] = stoneCounts[key] + count;
            else
                stoneCounts.Add(key, count);
        }

        public void Part2()
        {
            Dictionary<long, StoneResult> cachedStones = new Dictionary<long, StoneResult>();
            List<long> stones = input.Split(" ").Select(long.Parse).ToList();
            //seeding
            Dictionary<long, long> stoneCounts = new Dictionary<long, long>();
            foreach (var item in stones)
            {
                stoneCounts.Add(item, 1L);
            }

            for (int i = 0; i < 75; i++)
            {
                var currentLevel = new Dictionary<long, long>();
                foreach (var stoneCount in stoneCounts)
                {
                    StoneResult res;
                    var stone = stoneCount.Key;
                    if (!cachedStones.TryGetValue(stone, out res))
                    {
                        if (stone == 0)
                        {
                            res = new StoneResult { Id = stone, Next = 1, Split = false };
                        }
                        else if (stone.ToString().Length % 2 == 0)
                        {
                            var s = stone.ToString();
                            var s1 = long.Parse(string.Concat(s.Take(s.Length / 2)));
                            var s2 = long.Parse(string.Concat(s.Skip(s.Length / 2)));
                            res = new StoneResult { Id = stone, NextSplit = (s1, s2), Split = true };
                        }
                        else
                        {
                            res = new StoneResult { Id = stone, Next = stone * 2024L, Split = false };
                        }
                        cachedStones.Add(stone, res);
                    }
                    if (!res.Split)
                    {
                        AddOrIncrement(currentLevel, res.Next, stoneCount.Value);
                    }
                    else
                    {
                        AddOrIncrement(currentLevel, res.NextSplit.Item1, stoneCount.Value);
                        AddOrIncrement(currentLevel, res.NextSplit.Item2, stoneCount.Value);
                    }
                }
                stoneCounts = currentLevel;
            }
            Console.WriteLine(stoneCounts.Sum(x => x.Value));

        }
    }
}
