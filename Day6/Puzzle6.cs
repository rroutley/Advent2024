//#define Sample
using Point2d = (int x, int y);
using Point3d = (int x, int y, int z);
using Vector2d = (int x, int y);
public class Puzzle6 : IPuzzle
{

    int rows;
    int cols;
    Vector2d[] directions;

    public void Excute(FileInfo input)
    {
#if Sample
        var grid = sample.Split("\r\n");
#else
        var grid = File.ReadAllLines(input.FullName);
#endif

        rows = grid.Length;
        cols = grid[0].Length;
        directions = [
            (0,-1),
            (1,0),
            (0,1),
            (-1,0)
        ];

        // find the guard
        Point2d guard = (0, 0);
        for (int y = 0; y < rows; y++)
            for (int x = 0; x < cols; x++)
                if (grid[y][x] == '^')
                    guard = (x, y);

        var result = Part1(grid, guard);

        System.Console.WriteLine("Part 1 = {0}", result.Count);

        int count = Part2(grid, guard, result);

        System.Console.WriteLine("Part 2 = {0}", count);
    }

    private HashSet<Point2d> Part1(string[] grid, Point2d guard)
    {
        HashSet<Point2d> visited = new();
        int heading = 0;
        while (true)
        {
            visited.Add(guard);

            // Next Forward step
            Point2d next = (guard.x + directions[heading].x, guard.y + directions[heading].y);
            if (next.x < 0 || next.x >= cols || next.y < 0 || next.y >= rows)
            {
                break;
            }

            if (grid[next.y][next.x] == '#')
            {
                // Turn clockwise
                heading = (heading + 1) % 4;
            }
            else
            {
                guard = next;
            }
        }

        return visited;
    }


    private int Part2(string[] grid, (int x, int y) guard, HashSet<(int x, int y)> result)
    {
        int count = 0;

        foreach (var point in result)
        {
            if (point == guard) continue;

            string original = grid[point.y];
            grid[point.y] = $"{original[..point.x]}O{original[(point.x + 1)..]}";

            if (IsLoop(grid, guard))
            {
                System.Console.WriteLine($"Loop Found {point}");
                count++;
            }
            grid[point.y] = original;
        }

        return count;
    }

    private bool IsLoop(string[] grid, Point2d guard)
    {
        HashSet<Point3d> visited = new();
        int heading = 0;
        while (true)
        {
            var added = visited.Add((guard.x, guard.y, heading));

            // Have we been here and going in this direction before?
            if (!added)
            {
                return true;
            }

            // Next Forward step
            Point2d next = (guard.x + directions[heading].x, guard.y + directions[heading].y);

            // Are we off the grid?
            if (next.x < 0 || next.x >= cols || next.y < 0 || next.y >= rows)
            {
                return false;
            }

            char v = grid[next.y][next.x];
            if (v == '#' || v == 'O')
            {
                // Turn clockwise
                heading = (heading + 1) % 4;
            }
            else
            {
                guard = next;
            }
        }
    }

    private string sample = """
....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...
""";
}