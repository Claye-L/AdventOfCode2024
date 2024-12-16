using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day6
    {
        private char[][] _input;
        public Day6()
        {
            var rawinput = Helper.GetInput(6);
            //rawinput = "....#.....\r\n.........#\r\n..........\r\n..#.......\r\n.......#..\r\n..........\r\n.#..^.....\r\n........#.\r\n#.........\r\n......#...".Split("\r\n");
            _input = rawinput.Select(x => x.ToArray()).ToArray();
            //0,0  0,1  0,2
            //1,0  1,1  1,2
            //2,0  2,1  2,2
        }

        private (int i,int j) GetNextTile(int x, int y, char direction)
        {
            switch (direction)
            {
                case 'N':
                    return (x - 1, y);
                case 'E':
                    return (x, y + 1);
                case 'S': 
                    return (x + 1, y);
                case 'W':
                    return (x, y - 1);
                default:
                    throw new Exception();
            }
        }
        private char Rotate90(char direction) => direction == 'N' ? 'E' : direction == 'E' ? 'S' : direction == 'S' ? 'W' : 'N';
        public void Part1()
        {
            var visitedTiles = new bool[_input.Length, _input.Length];
            var initialPosition = _input.SelectMany((r, i) => r.Select((c, j) => (c, i, j))).FirstOrDefault(x => x.c == '^');
            var position = (initialPosition.i, initialPosition.j);
            var direction = 'N';
            while(true)
            {
                //log position
                visitedTiles[position.i, position.j] = true;
                //check next tile
                (int i, int j) = GetNextTile(position.i, position.j, direction);
                //if OOB, end
                if (i < 0 || i >= _input.Length || j < 0 || j >= _input.Length )
                    break;
                //if empty, move
                if (_input[i][j] == '.' || _input[i][j] == '^')
                {
                    position = (i, j);
                }
                //if blocked turn
                if (_input[i][j] == '#')
                {
                    direction = Rotate90(direction);
                }
            }

            Console.WriteLine(visitedTiles.Flatten().Count(x => x));
        }
        private int DirectionToBinary(char direction) => direction == 'N' ? 0b0001 : direction == 'E' ? 0b0010 : direction == 'S' ? 0b0100 : 0b1000;
        private bool IsPathLoop(char[][] grid, (int,int) initialPosition)
        {
            var visitedTiles = new int[_input.Length, _input.Length];
            (int i, int j) position = initialPosition;
            var direction = 'N';
            while (true)
            {
                //check for loop
                if ((visitedTiles[position.i, position.j] & DirectionToBinary(direction)) != 0)
                {
                    return true;
                }
                //log position
                visitedTiles[position.i, position.j] |= DirectionToBinary(direction);
                //check next tile
                (int i, int j) = GetNextTile(position.i, position.j, direction);
                //if OOB, end
                if (i < 0 || i >= _input.Length || j < 0 || j >= _input.Length)
                    return false;
                //if empty, move
                if (_input[i][j] == '.' || _input[i][j] == '^')
                {
                    position = (i, j);
                }
                //if blocked turn
                else if (_input[i][j] == '#')
                {
                    direction = Rotate90(direction);
                }
            }
        }

        public void Part2()
        {
            var initialPosition = _input.SelectMany((r, i) => r.Select((c, j) => (c, i, j))).FirstOrDefault(x => x.c == '^');
            int loops = 0;
            for (int row = 0; row < _input.Length; row++)
            {
                for (int col = 0; col < _input.Length; col++)
                {
                    if (_input[row][col] == '.')
                    {
                        _input[row][col] = '#';
                        if (IsPathLoop(_input, (initialPosition.i, initialPosition.j)))
                            loops++;
                        _input[row][col] = '.';
                    }
                }
            }
            Console.WriteLine(loops);
        }
    }
}
