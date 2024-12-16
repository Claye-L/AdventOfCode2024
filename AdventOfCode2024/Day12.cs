using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day12
    {
        private char[][] _input;
        public Day12()
        {
            var rawinput = Helper.GetInput(12);
            //rawinput = "RRRRIICCFF\r\nRRRRIICCCF\r\nVVRRRCCFFF\r\nVVRCCCJFFF\r\nVVVVCJJCFE\r\nVVIVCCJJEE\r\nVVIIICJJEE\r\nMIIIIIJJEE\r\nMIIISIJEEE\r\nMMMISSJEEE".Split("\r\n");
            //rawinput = "OOOOO\r\nOXOXO\r\nOOOOO\r\nOXOXO\r\nOOOOO".Split("\r\n");
            //rawinput = "AAAA\r\nBBCD\r\nBBCC\r\nEEEC".Split("\r\n");
            //rawinput = "EEEEE\r\nEXXXX\r\nEEEEE\r\nEXXXX\r\nEEEEE".Split("\r\n");
            //rawinput = "AAAAAA\r\nAAABBA\r\nAAABBA\r\nABBAAA\r\nABBAAA\r\nAAAAAA".Split("\r\n");
            _input = rawinput.Select(x => x.ToArray()).ToArray();
        }

        public void Part1()
        {
            int[,] regionIds = new int[_input.Length, _input[0].Length];
            int currentId = 1;

            void TagContiguousRegion(char region, int id, int i, int j)
            {
                if (!Helper.CheckBounds(i,j, _input.Length, _input[0].Length))
                {
                    return;
                }
                if (regionIds[i,j] == 0 && _input[i][j] == region)
                {
                    regionIds[i, j] = id;
                    TagContiguousRegion(region, id, i + 1, j);
                    TagContiguousRegion(region, id, i - 1, j);
                    TagContiguousRegion(region, id, i, j + 1);
                    TagContiguousRegion(region, id, i, j - 1);
                }
            }

            int GetPerimeter(int i, int j)
            {
                int perimeter = 0;
                perimeter += !Helper.CheckBounds(i + 1, j, _input.Length, _input[0].Length) || regionIds[i + 1, j] != regionIds[i, j] ? 1 : 0;
                perimeter += !Helper.CheckBounds(i - 1, j, _input.Length, _input[0].Length) || regionIds[i - 1, j] != regionIds[i, j] ? 1 : 0;
                perimeter += !Helper.CheckBounds(i, j + 1, _input.Length, _input[0].Length) || regionIds[i, j + 1] != regionIds[i, j] ? 1 : 0;
                perimeter += !Helper.CheckBounds(i, j - 1, _input.Length, _input[0].Length) || regionIds[i, j - 1] != regionIds[i, j] ? 1 : 0;

                return perimeter;
            }
            List<(int i, int j)> regionCorners = new List<(int i, int j)>();
            for (int i = 0; i < _input.Length; i++)
            {
                for (int j = 0; j < _input[0].Length; j++)
                {
                    if (regionIds[i,j] == 0)
                    {
                        TagContiguousRegion(_input[i][j], currentId, i, j);
                        regionCorners.Add((i, j));
                        currentId++;
                    }
                }
            }
            var areas = Helper.Flatten(regionIds).ToLookup(x => x);
            var perimeters = Helper.Flatten(regionIds).Distinct().ToDictionary(x => x, x => 0);
            for (int i = 0; i < _input.Length; i++)
            {
                for (int j = 0; j < _input[0].Length; j++)
                {
                     perimeters[regionIds[i,j]] += GetPerimeter(i, j);
                }
            }
            var retval = areas.Select(g => g.Count() * perimeters[g.Key]).Sum();
            Console.WriteLine(retval);
        }

        public void Part2()
        {
            int[,] regionIds = new int[_input.Length, _input[0].Length];
            int currentId = 1;

            void TagContiguousRegion(char region, int id, int i, int j)
            {
                if (!Helper.CheckBounds(i, j, _input.Length, _input[0].Length))
                {
                    return;
                }
                if (regionIds[i, j] == 0 && _input[i][j] == region)
                {
                    regionIds[i, j] = id;
                    TagContiguousRegion(region, id, i + 1, j);
                    TagContiguousRegion(region, id, i - 1, j);
                    TagContiguousRegion(region, id, i, j + 1);
                    TagContiguousRegion(region, id, i, j - 1);
                }
            }

            List<(int i, int j)> regionCorners = new List<(int i, int j)>();
            for (int i = 0; i < _input.Length; i++)
            {
                for (int j = 0; j < _input[0].Length; j++)
                {
                    if (regionIds[i, j] == 0)
                    {
                        TagContiguousRegion(_input[i][j], currentId, i, j);
                        regionCorners.Add((i, j));
                        currentId++;
                    }
                }
            }
            var areas = Helper.Flatten(regionIds).ToLookup(x => x);
            int NormalisedValue(int i, int j, int targetId) => Helper.CheckBounds(i, j, _input.Length, _input[0].Length) && regionIds[i,j] == targetId ? 1 :0 ;
            //edge detection matrix
            //1 in a slot = an edge above//to the right
            //Helper.Write2dArray(regionIds);
            Dictionary<int,int> regionEdgeCounts = new Dictionary<int,int>();
            foreach (var regionid in areas.Select(x => x.Key))
            {
                int[,] edges = new int[_input.Length, _input[0].Length ];
                for (int i = 0; i < _input.Length; i++)
                {
                    for (int j = 0; j < _input[0].Length; j++)
                    {
                        if (regionIds[i,j] == regionid)
                        {
                            //up is 1, right 2, down 4, left 8
                            edges[i, j] += NormalisedValue(i - 1, j, regionid) == NormalisedValue(i, j, regionid) ? 0 : 1;
                            edges[i, j] += NormalisedValue(i + 1, j, regionid) == NormalisedValue(i, j, regionid) ? 0 : 4;
                            edges[i, j] += NormalisedValue(i, j - 1, regionid) == NormalisedValue(i, j, regionid) ? 0 : 8;
                            edges[i, j] += NormalisedValue(i, j + 1, regionid) == NormalisedValue(i, j, regionid) ? 0 : 2;
                        }
                    }
                }
                //Helper.Write2dArray(horizontalEdges);
                //Helper.Write2dArray(verticalEdges);
                int upCount = 0;
                int downCount =0;
                int leftCount = 0;
                int rightCount = 0;
                int state = 0;
                for (int i = 0; i < _input.Length; i++)
                {
                    state = 0;
                    for (int j = 0; j < _input[0].Length; j++)
                    {
                        //Up edges
                        if ((edges[i,j] & 1) > 0)
                        {
                            if (state == 0)
                            {
                                upCount++;
                            }
                            state = 1;
                        }
                        else
                        {
                            state = 0;
                        }
                    }
                }
                for (int i = 0; i < _input.Length; i++)
                {
                    state = 0;
                    for (int j = 0; j < _input[0].Length; j++)
                    {
                        //Down edges
                        if ((edges[i, j] & 4) > 0)
                        {
                            if (state == 0)
                            {
                                downCount++;
                            }
                            state = 1;
                        }
                        else
                        {
                            state = 0;
                        }
                    }
                }
                for (int j = 0; j < _input[0].Length; j++)
                {
                    state = 0;
                    for (int i = 0; i < _input.Length; i++)
                    {
                        //left edges
                        if ((edges[i, j] & 8) > 0)
                        {
                            if (state == 0)
                            {
                                leftCount++;
                            }
                            state = 1;
                        }
                        else
                        {
                            state = 0;
                        }
                    }

                }
                for (int j = 0; j < _input[0].Length; j++)
                {
                    state = 0;
                    for (int i = 0; i < _input.Length; i++)
                    {
                        //right edges
                        if ((edges[i, j] & 2) > 0)
                        {
                            if (state == 0)
                            {
                                rightCount++;
                            }
                            state = 1;
                        }
                        else
                        {
                            state = 0;
                        }
                    }

                }
                regionEdgeCounts.Add(regionid, upCount + downCount + rightCount + leftCount);
            }
            var retval = areas.Select(g => g.Count() * regionEdgeCounts[g.Key]).Sum();
            Console.WriteLine(retval);
        }
    }
}
