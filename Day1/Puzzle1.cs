using static System.Math;
class Puzzle1 : IPuzzle
{
    public void Excute(FileInfo input)
    {
        var lines = sample.Split("\r\n");
        lines = File.ReadAllLines(input.FullName);

        var left = lines.Select(a => a.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]).Select(int.Parse).ToList();
        var right = lines.Select(a => a.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]).Select(int.Parse).ToList();

        var result = left.OrderBy(a => a)
                         .Zip(right.OrderBy(b => b))
                         .Select(item => Abs(item.First - item.Second))
                         .Sum(z => z);
        System.Console.WriteLine("Part 1 = {0}",result);


        var freq = right.GroupBy(a => a)
                        .ToDictionary(k => k.Key, v => v.Count());
        result = left.Join(freq, l => l, r => r.Key, (l, r) => l * r.Value).Sum();

        System.Console.WriteLine("Part 2 = {0}",result);
    }



    private string sample = """
3   4
4   3
2   5
1   3
3   9
3   3
""";
}