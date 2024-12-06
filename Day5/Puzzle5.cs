//#define Sample

using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Text.Json.Nodes;

public class Puzzle5 : IPuzzle
{

    public void Excute(FileInfo input)
    {
#if Sample
        var lines = sample.Split("\r\n");
#else
        var lines = File.ReadAllLines(input.FullName);
#endif

        var rules = new HashSet<(int, int)>();
        var updates = new List<int[]>();

        int state = 0;
        foreach (var line in lines)
        {
            if (state == 0)
            {
                if (line == "")
                {
                    state = 1;
                }
                else
                {
                    (int first, int second) = line.Split('|', 2).Select(int.Parse).ToArray();
                    rules.Add((first, second));
                }
            }
            else
            {
                var update = line.Split(',').Select(int.Parse).ToArray();
                updates.Add(update);

            }
        }

        var outOfOrder = new List<int[]>();

        var result = 0;
        foreach (var update in updates)
        {
            if (IsInCorrectOrder(update, rules))
            {

                result += update[update.Length / 2];
            }
            else
            {
                outOfOrder.Add(update);
                System.Console.WriteLine(string.Join(',', update));
            }
        }
        System.Console.WriteLine("Part 1 = {0}", result);


        result = 0;


        var grouped = rules.GroupBy(r => r.Item1).ToDictionary(k => k.Key, v => v.Select(s => s.Item2).ToArray());

        foreach (var update in outOfOrder)
        {
            System.Console.WriteLine(string.Join(',', update));
            var correct = CorrectOrder(update, 0, grouped.ToImmutableDictionary());
            System.Console.WriteLine(string.Join(',', correct));

            result += correct[correct.Count / 2];
        }

        System.Console.WriteLine("Part 2 = {0}", result);

    }



    private bool IsInCorrectOrder(IList<int> update, HashSet<(int, int)> rules)
    {
        for (int i = 0; i < update.Count; i++)
        {
            int f = update[i];

            // Is there a rule for every update after this ?
            for (int j = i + 1; j < update.Count; j++)
            {
                var s = update[j];

                if (!rules.Contains((f, s)))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private IList<int> CorrectOrder(IList<int> update, int index, IImmutableDictionary<int, int[]> rules)
    {

        var stack = new Stack<State>();

        stack.Push(new State([], update.ToImmutableList(), rules));


        while (stack.Count > 0)
        {
            var state = stack.Pop();

            int length = state.Remainder.Count - 1;
            var remainder = state.Remainder;

            if (length == 0)
            {
                return state.Start.Add(remainder[0]);
            }


            var candidates = state.rules.Where(kv => kv.Value.Count() >= length).Select(kv => kv.Key);

            foreach (var candidate in candidates)
            {
                var i = remainder.IndexOf(candidate);
                if (i < 0)
                {
                    continue;
                }


                List<int> result = [.. remainder];


                // Swap
                (result[i], result[index]) = (result[index], result[i]);

                // Check there is a rule for all subsequent items
                var valid = state.rules[candidate];
                if (!result[(index + 1)..result.Count].All(s => valid.Contains(s)))
                {
                    continue;
                }

                stack.Push(new State(
                    state.Start.Add(candidate),
                    state.Remainder.Remove(candidate),
                    state.rules.Remove(candidate)
                    ));


            }

        }

        throw new UnreachableException();
    }
    record State(ImmutableList<int> Start, ImmutableList<int> Remainder, IImmutableDictionary<int, int[]> rules);



    private string sample = """
47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47
""";
}