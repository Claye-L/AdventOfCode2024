using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day4
    {
        private string[] _input;
        public Day4()
        {
            _input = Helper.GetInput(4);
            //_input = "MMMSXXMASM\r\nMSAMXMSMSA\r\nAMXSXMAAMM\r\nMSAMASMSMX\r\nXMASAMXAMM\r\nXXAMMXXAMA\r\nSMSMSASXSS\r\nSAXAMASAAA\r\nMAMMMXMMMM\r\nMXMXAXMASX".Split("\r\n");
        }

        public void Part1()
        {
            int cols = _input[0].Length;
            int rows = _input.Length;
            int result = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    //horizontal
                    if(j + 3 <= cols - 1)
                    {
                        var s = string.Concat(_input[i][j], _input[i][j + 1], _input[i][j + 2], _input[i][j + 3]);
                        if(s == "XMAS" || s == "SAMX")
                            result++;
                    }
                    //vertical
                    if (i + 3 <= rows - 1)
                    {
                        var s = string.Concat(_input[i][j], _input[i + 1][j], _input[i + 2][j], _input[i + 3][j]);
                        if (s == "XMAS" || s == "SAMX")
                            result++;
                    }
                    if(i + 3 <= rows - 1 && j + 3 <= cols - 1)
                    {
                        var s = string.Concat(_input[i][j], _input[i + 1][j + 1], _input[i + 2][j + 2], _input[i + 3][j + 3]);
                        if (s == "XMAS" || s == "SAMX")
                            result++;
                    }
                    if (i + 3 <= rows - 1 && j - 3 >= 0)
                    {
                        var s = string.Concat(_input[i][j], _input[i + 1][j - 1], _input[i + 2][j - 2], _input[i + 3][j - 3]);
                        if (s == "XMAS" || s == "SAMX")
                            result++;
                    }
                }
            }
            Console.WriteLine(result);
        }

        public void Part2()
        {
            int cols = _input[0].Length;
            int rows = _input.Length;
            int result = 0;
            for (int i = 1; i < rows - 1; i++)
            {
                for (int j = 1; j < cols - 1; j++)
                {
                    if (_input[i][j] == 'A')
                    {
                        var s1 = string.Concat(_input[i - 1][j - 1], _input[i + 1][j + 1]);
                        var s2 = string.Concat(_input[i + 1][j - 1], _input[i - 1][j + 1]);
                        if ((s1 == "SM" || s1 == "MS") && (s2 == "MS" || s2 == "SM"))
                        {
                            result++;
                        }
                    }
                }
            }
            Console.WriteLine(result);
        }
    }
}
