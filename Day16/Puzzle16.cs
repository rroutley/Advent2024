//#define Sample

using System.Diagnostics;
using Point2d = (int x, int y);

public class Puzzle16 : IPuzzle
{
    int _rows;
    int _cols;

    private readonly static Point2d N = (0, -1), E = (1, 0), S = (0, 1), W = (-1, 0);
    private readonly Point2d[] _directions = [N, E, S, W];

    public void Excute(FileInfo input)
    {
#if Sample
        var lines = sample.Split("\r\n");
#else
        var lines = File.ReadAllLines(input.FullName);
#endif
        _rows = lines.Length;
        _cols = lines[0].Length;
        var grid = new char[_cols, _rows];

        Point2d start = (0, 0);
        Point2d heading = E;
        Point2d end = (0, 0);

        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                if (lines[r][c] == 'S')
                {
                    start = (c, r);
                    grid[c, r] = '.';
                }
                else if (lines[r][c] == 'E')
                {
                    end = (c, r);
                    grid[c, r] = '.';
                }
                else
                {
                    grid[c, r] = lines[r][c];
                }
            }
        }


        long result = PathToEnd(grid, start, heading, end); ;


        Render(grid);

        System.Console.WriteLine("Part 1 = {0}", result);
        // 105512 is too high


        System.Console.WriteLine("Part 2 = {0}", result);

    }

    private int PathToEnd(char[,] grid, Point2d start, Point2d initialHeading, Point2d end)
    {
        var queue = new PriorityQueue<Point2d, int>();
        queue.Enqueue(start, 0);

        var distance = new Dictionary<Point2d, int>();
        var prev = new Dictionary<Point2d, Point2d>
        {
            // Set "previous" for the start to be West of start so initial heading will be East
            [start] = (start.x - initialHeading.x, start.y - initialHeading.y)
        };

        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                if (grid[c, r] == '.')
                {
                    distance[(c, r)] = int.MaxValue;
                    queue.Enqueue((c, r), int.MaxValue);
                }
            }
        }
        distance[start] = 0;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            Point2d heading = (current.x - prev[current].x, current.y - prev[current].y);

            Debug.Assert(Math.Abs(heading.x) + Math.Abs(heading.y) == 1);

            Point2d ahead = (current.x + heading.x, current.y + heading.y);
            if (grid[ahead.x, ahead.y] == '.')
            {
                var alt = distance[current] + 1;
                if (alt < distance[ahead])
                {
                    prev[ahead] = current;
                    distance[ahead] = alt;
                    queue.Remove(ahead, out _, out _);
                    queue.Enqueue(ahead, alt);
                }
            }

            Point2d clockwise = (current.x + heading.y, current.y + heading.x);
            if (grid[clockwise.x, clockwise.y] == '.')
            {
                var alt = distance[current] + 1001;
                if (alt < distance[clockwise])
                {
                    prev[clockwise] = current;
                    distance[clockwise] = alt;
                    queue.Remove(clockwise, out _, out _);
                    queue.Enqueue(clockwise, alt);
                }
            }

            Point2d counterClockwise = (current.x - heading.y, current.y - heading.x);
            if (grid[counterClockwise.x, counterClockwise.y] == '.')
            {
                var alt = distance[current] + 1001;
                if (alt < distance[counterClockwise])
                {
                    prev[counterClockwise] = current;
                    distance[counterClockwise] = alt;
                    queue.Remove(counterClockwise, out _, out _);
                    queue.Enqueue(counterClockwise, alt);
                }
            }
        }

        // Retrace path
        var x = end;
        while (x != start)
        {
            grid[x.x, x.y] = 'o';
            x = prev[x];
        }

        return distance[end];
    }


    private void Render(char[,] grid)
    {
        System.Console.WriteLine(string.Empty.PadRight(50, '-'));
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                Console.Write(grid[c, r]);

            }
            System.Console.WriteLine();
        }
    }

    private string sample = """
###############
#.......#....E#
#.#.###.#.###.#
#.....#.#...#.#
#.###.#####.#.#
#.#.#.......#.#
#.#.#####.###.#
#...........#.#
###.#.#####.#.#
#...#.....#.#.#
#.#.#.###.#.#.#
#.....#...#.#.#
#.###.#.#.#.#.#
#S..#.....#...#
###############
""";
}