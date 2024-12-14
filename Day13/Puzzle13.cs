// #define Sample

using Vector2d = (long x, long y);

using System.Text.RegularExpressions;

public class Puzzle13 : IPuzzle
{

    public void Excute(FileInfo input)
    {
#if Sample
        var lines = sample;
#else
        var lines = File.ReadAllText(input.FullName);
#endif

        List<Claw> claws = Parse(lines, 0);

        long result = 0;
        foreach (var claw in claws)
        {
            System.Console.WriteLine($"a={claw.ButtonA} b={claw.ButtonB} p={claw.Prize}");

            var minA = 1;
            var minB = 1;
            var maxA = 100;
            var maxB = 100;

            var minCost = int.MaxValue;
            for (int a = minA; a <= maxA; a++)
            {
                for (int b = minB; b <= maxB; b++)
                {
                    if (a * claw.ButtonA.x + b * claw.ButtonB.x == claw.Prize.x
                        && a * claw.ButtonA.y + b * claw.ButtonB.y == claw.Prize.y)
                    {
                        var cost = a * 3 + b;
                        if (cost < minCost)
                        {
                            minCost = cost;
                            System.Console.WriteLine($"Button A * {a} and B * {b}");
                        }
                    }

                }
            }

            if (minCost < int.MaxValue)
            {
                result += minCost;
            }

        }

        System.Console.WriteLine("Part 1 = {0}", result);

        claws = Parse(lines, 10000000000000);
        result = 0;
        foreach (var claw in claws)
        {
            System.Console.WriteLine($"a={claw.ButtonA} b={claw.ButtonB} p={claw.Prize}");

            var determinant = claw.ButtonA.x * claw.ButtonB.y - claw.ButtonB.x * claw.ButtonA.y;

            var a = claw.Prize.x * claw.ButtonB.y / determinant - claw.Prize.y * claw.ButtonB.x / determinant;
            var b = -claw.Prize.x * claw.ButtonA.y / determinant + claw.Prize.y * claw.ButtonA.x / determinant;

            if (a * claw.ButtonA.x + b * claw.ButtonB.x == claw.Prize.x
                && a * claw.ButtonA.y + b * claw.ButtonB.y == claw.Prize.y)
            {
                var cost = a * 3 + b;
                System.Console.WriteLine($"Button A * {a} and B * {b}");

                result += cost;
            }

        }

        System.Console.WriteLine("Part 2 = {0}", result);

    }

    private static List<Claw> Parse(string lines, long extra)
    {
        var pattern = """
Button A: X\+(\d+), Y\+(\d+)[\n|\r]*
Button B: X\+(\d+), Y\+(\d+)[\n|\r]*
Prize: X=(\d+), Y=(\d+)
""";
        var claws = new List<Claw>();

        var matches = Regex.Matches(lines, pattern);

        foreach (Match match in matches)
        {
            Vector2d buttonA = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
            Vector2d buttonB = (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
            Vector2d prize = (extra + int.Parse(match.Groups[5].Value), extra + int.Parse(match.Groups[6].Value));

            claws.Add(new Claw(buttonA, buttonB, prize));

        }

        return claws;
    }

    record Claw(Vector2d ButtonA, Vector2d ButtonB, Vector2d Prize);

    private string sample = """
Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400

Button A: X+26, Y+66
Button B: X+67, Y+21
Prize: X=12748, Y=12176

Button A: X+17, Y+86
Button B: X+84, Y+37
Prize: X=7870, Y=6450

Button A: X+69, Y+23
Button B: X+27, Y+71
Prize: X=18641, Y=10279
""";
}