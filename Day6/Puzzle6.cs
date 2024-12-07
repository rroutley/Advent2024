// #define Sample
using Point2d = (int x, int y);
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

        int result = Part1(grid, guard);

        System.Console.WriteLine("Part 1 = {0}", result);

    }

    private int Part1(string[] grid, Point2d guard)
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

        return visited.Count;
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