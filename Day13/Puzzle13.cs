// #define Sample

using Vector2d = (int x, int y);

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

        List<Claw> claws = Parse(lines);

        long result = 0;
        foreach (var claw in claws)
        {
            System.Console.WriteLine($"a={claw.ButtonA} b={claw.ButtonB} p={claw.Prize}");

            var minA = 1; //Math.Min(int.MaxValue, p.x / Math.Min(a.x, b.x));
            var minB = 1; //Math.Min(int.MaxValue, p.y / Math.Min(a.y, b.y));
            var maxA = 100; //p.x / Math.Max(a.x, b.x);
            var maxB = 100; //p.y / Math.Max(a.y, b.y);

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
            
            // for (int a = minX; a <= maxX; a++)
            // {
            //     int remindingX = prize.x - buttonA.x * a;
            //     if (remindingX > 0 && remindingX % buttonB.x == 0)
            //     {
            //         int remainingY = prize.y - buttonA.y * a;
            //         int b = remindingX / buttonB.x;

            //         if (remainingY > 0 && remainingY % buttonB.y == 0 && b <= 100)
            //         {
            //             var cost = a * 3 + b;
            //             if (cost < minCost)
            //             {
            //                 minCost = cost;
            //                 System.Console.WriteLine($"Button A * {a} and B * {b}");
            //             }
            //         }

            //     }
            // }

            if (minCost < int.MaxValue)
            {
                result += minCost;
            }

        }



        System.Console.WriteLine("Part 1 = {0}", result);



        System.Console.WriteLine("Part 2 = {0}", result);

    }

    private static List<Claw> Parse(string lines)
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
            Vector2d prize = (int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value));

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