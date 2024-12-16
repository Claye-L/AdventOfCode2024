using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day9
    {
        private string _input;
        public Day9()
        {
            _input = Helper.ReadInputFile(9);
            //_input = "12345";
            //_input = "2333133121414131402";
        }

        private struct Range
        {
            public Range(int start, int end)
            {
                Start = start;
                End = end;
            }
            public int Start; 
            public int End; 
            public int Length => End - Start;
        }
        private struct FileRange
        {
            public int Id;
            public int Start;
            public int End;
            public int Length => End - Start;
        }
        
        private void SetValue(int[] list ,int id, int start, int end)
        {
            for (int i = start; i < end; i++)
                list[i] = id;
        }
        public void Part1()
        {
            Stack<FileRange> files = new Stack<FileRange>();
            List<Range> spaces = new List<Range>();
            int[] memory = new int[_input.Length * 10];
            int j = 0;
            for (int i = 0; i < _input.Length; i++) 
            { 
                var c = int.Parse(_input[i].ToString());
                if (i % 2 == 0)
                {
                    files.Push(new FileRange { Id = i / 2, Start = j, End = j + c});
                    SetValue(memory, i/2, j, j+c);
                }
                else
                {
                    SetValue(memory, -1, j, j+c);
                    spaces.Add(new Range(j, j + c));
                }
                j += c;
            }
            var currentFile = files.Pop();
            int maxindex =currentFile.End;
            j = maxindex-1;
            for (int i = 0; i < maxindex; i++)
            {
                if (j < i)
                    break;
                if (memory[i] == -1)
                {
                    memory[i] = currentFile.Id;
                    memory[j] = -1;
                    if (j <= currentFile.Start)
                    {
                        currentFile = files.Pop();
                        j = currentFile.End -1;
                    }
                    else
                    {
                        j--;
                    }
                    
                }
                //check if empty
                //grab at index j
                //empty index j
                //decrement j (-1 or pop off the stack)
                //end condition is j <= i
            }
            Console.WriteLine(memory.Select((x, i) => (long)(x * i)).Where(x => x > 0).Sum(x => x));
        }

        private (bool,Range) FindNextSlot(int[] memory, int i)
        {
            bool running = false;
            int start = i;
            while (i < memory.Length)
            {
                if (memory[i] == -1)
                {
                    if (!running) 
                    { 
                        running = true;
                        start = i;
                    }
                }
                else if(running)
                {
                    return (true,new Range(start, i));
                }
                i++;
            }
            return (false, new Range(start, i));
        }

        public void Part2()
        {
            Stack<FileRange> files = new Stack<FileRange>();
            List<Range> spaces = new List<Range>();
            int[] memory = new int[_input.Length * 10];
            SetValue(memory, -1, 0, memory.Length);
            int j = 0;
            for (int i = 0; i < _input.Length; i++)
            {
                var c = int.Parse(_input[i].ToString());
                if (i % 2 == 0)
                {
                    files.Push(new FileRange { Id = i / 2, Start = j, End = j + c });
                    SetValue(memory, i / 2, j, j + c);
                }
                else
                {
                    SetValue(memory, -1, j, j + c);
                    spaces.Add(new Range(j, j + c));
                }
                j += c;
            }

            foreach (FileRange range in files) 
            {
                int i = 0;
                while (i < range.Start)
                {
                    (bool found, Range space) = FindNextSlot(memory, i);
                    if (found && space.Length >= range.Length && space.Start < range.Start)
                    {
                        SetValue(memory, range.Id, space.Start, space.Start + range.Length);
                        SetValue(memory, -1, range.Start, range.End);
                        break;
                    }
                    else if (!found)
                    {
                        break;
                    }
                    i = space.End;

                }
            }
            Console.WriteLine(memory.Select((x, i) => (long)(x * i)).Where(x => x > 0).Sum(x => x));

        }
    }
}
