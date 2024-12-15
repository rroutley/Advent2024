// #define Sample

using System.Text.RegularExpressions;
using Point2d = (int x, int y);
using Vector2d = (int x, int y);

public class Puzzle14 : IPuzzle
{
#if Sample
    static int _rows = 7;
    static int _cols = 11;
#else
    static int _rows = 103;
    static int _cols = 101;
#endif

    public void Excute(FileInfo input)
    {
#if Sample
        var lines = sample.Split("\r\n");
#else
        var lines = File.ReadAllLines(input.FullName);
#endif

        var robots = Parse(lines);

        Plot(robots, Console.Out);

        for (int i = 0; i < 100; i++)
        {
            foreach (var robot in robots)
            {
                robot.Update();
            }
        }

        Plot(robots, Console.Out);

        Point2d centre = (_cols / 2, _rows / 2);

        var q1 = robots.Where(r => r.Position.x < centre.x && r.Position.y < centre.y).Count();
        var q2 = robots.Where(r => r.Position.x > centre.x && r.Position.y < centre.y).Count();
        var q3 = robots.Where(r => r.Position.x < centre.x && r.Position.y > centre.y).Count();
        var q4 = robots.Where(r => r.Position.x > centre.x && r.Position.y > centre.y).Count();

        Console.WriteLine($"q1={q1}, q2={q2}, q3={q3}, q4={q4}");


        long result = q1 * q2 * q3 * q4;

        Console.WriteLine("Part 1 = {0}", result);



        using var writer = File.CreateText(Path.Combine(input.DirectoryName, "output.txt"));

        robots = Parse(lines);
        var robotCount = robots.Count;
        long s = 0;
        for (int i = 0; i < _rows * _cols; i++)
        {
            foreach (var robot in robots)
                robot.Update();

            s++;

            // Does the Chrustmas tree emerge when there are no overlaps?
            if (robots.Select(r => r.Position).Distinct().Count() == robotCount)
            {
                writer.WriteLine($"Seconds = {s}");
                Plot(robots, writer);
                writer.WriteLine();
            }
        }



        Console.WriteLine("Part 2 = {0}", result);

    }

    private void Plot(List<Robot> robots, TextWriter writer)
    {
        writer.WriteLine(string.Empty.PadRight(50, '='));
        for (int r = 0; r < _rows; r++)
        {
            writer.Write($"{r:000} ");
            for (int c = 0; c < _cols; c++)
            {
                Point2d pos = (c, r);
                var count = robots.Count(r => r.Position == pos);
                if (count == 0)
                    writer.Write('.');
                else
                    writer.Write(count);

            }
            writer.WriteLine();
        }
    }

    private static List<Robot> Parse(string[] lines)
    {
        var robots = new List<Robot>();
        foreach (var line in lines)
        {
            var match = Regex.Match(line, @"p=(\d+),(\d+) v=(-?\d+),(-?\d+)");

            Point2d position = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
            Vector2d velocity = (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));

            robots.Add(new Robot(position, velocity));
        }

        return robots;
    }

    public class Robot(Point2d position, Vector2d velocity)
    {
        public Point2d Position { get; private set; } = position;
        public Vector2d Velocity { get; private set; } = velocity;

        public void Update()
        {
            var x = Position.x + Velocity.x;
            var y = Position.y + Velocity.y;
            if (x < 0) x += _cols;
            if (y < 0) y += _rows;
            if (x >= _cols) x -= _cols;
            if (y >= _rows) y -= _rows;

            Position = (x, y);
        }
    }


    private string sample = """
p=2,4 v=2,-3
p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=9,5 v=-3,-3
""";

    string spare = """

""";
}
