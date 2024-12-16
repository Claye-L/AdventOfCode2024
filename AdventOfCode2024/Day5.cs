using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public  class Day5
    {
        private string[] _rulesInput;
        private string[] _pagesInput;
        public Day5()
        {
            var rawinput = Helper.GetInput(5);
            //rawinput = "47|53\r\n97|13\r\n97|61\r\n97|47\r\n75|29\r\n61|13\r\n75|53\r\n29|13\r\n97|29\r\n53|29\r\n61|53\r\n97|53\r\n61|29\r\n47|13\r\n75|47\r\n97|75\r\n47|61\r\n75|61\r\n47|29\r\n75|13\r\n53|13\r\n\r\n75,47,61,53,29\r\n97,61,53,29,13\r\n75,29,13\r\n75,97,47,61,53\r\n61,13,29\r\n97,13,75,29,47".Split("\r\n");
            _rulesInput = rawinput.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            _pagesInput = rawinput.Skip(_rulesInput.Length + 1).ToArray();
        }

        private class Rule
        {
            public int before;
            public int after;
        }
        private Rule ParseRule(string s)
        {
            var ns = s.Split('|').Select(n => int.Parse(n)).ToArray();
            return new Rule {before = ns[0], after = ns[1] };
        }
        private bool IsValidPage(List<int> pages, ILookup<int,Rule> rules)
        {
            for (int i = 0; i < pages.Count; i++)
            {
                int n = pages[i];
                foreach (var rule in rules[n])
                {
                    int j = pages.IndexOf(rule.after);
                    if (j != -1 && j < i)
                        return false;
                }
            }
            return true;
        }
        public void Part1()
        {
            var rules = _rulesInput.Select(ParseRule).ToLookup(x => x.before);
            var pagesList = _pagesInput.Select(s => s.Split(',').Select(n => int.Parse(n)).ToList());
            var res = pagesList.Where(x => IsValidPage(x, rules)).Select(x => x[x.Count / 2 ]).Sum();
            Console.WriteLine(res);
        }

        public void Part2()
        {
            //find all incorrect protocols using part 1 solution
            var allRules = _rulesInput.Select(ParseRule).ToList();
            var rulesBefore = allRules.ToLookup(x => x.before);
            var protocolList = _pagesInput.Select(s => s.Split(',').Select(n => int.Parse(n)).ToList()).Where(x => !IsValidPage(x, rulesBefore)).ToList();

            var orderedProtocols = new List<List<int>>();
            foreach (var protocol in protocolList)
            {
                //Grab all the rules that apply to the current protocol
                var remainingRules = allRules.Where(r => protocol.Contains(r.before) && protocol.Contains(r.after)).ToList();
                var remainingNodes = protocol.ToList();
                var orderedNodes = new List<int>(protocol.Count);
                while (remainingNodes.Count > 1)
                {
                    //all arrows point from a node. This list will have every node but the last one
                    rulesBefore = remainingRules.ToLookup(x => x.before);
                    //all arrows pointing to a node
                    var rulesAfter = remainingRules.ToLookup(x => x.after);
                    //Find nodes that have no arrows pointing from them
                    var endNodes = rulesAfter.Where(rs => !rulesBefore[rs.Key].Any()).ToList();
                    if (endNodes.Count > 1)
                    {
                        throw new Exception("oh shit fuck we need backtracking");
                    }
                    //remove the last node from the graph and take it back from the top
                    var node = endNodes.First().Key;
                    orderedNodes.Add(node);
                    remainingNodes.Remove(node);
                    remainingRules = remainingRules.Where(r => r.after != node && r.before != node).ToList();
                }
                orderedNodes.Add(remainingNodes.FirstOrDefault());
                orderedProtocols.Add(orderedNodes);
            }

            Console.WriteLine(orderedProtocols.Select(x => x[x.Count / 2]).Sum());
        }
    }
}
