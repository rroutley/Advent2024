//#define Sample
using Point2d = (int x, int y);

public class Puzzle20 : IPuzzle
{
    int _rows;
    int _cols;

    private readonly static Point2d N = (0, -1), E = (1, 0), S = (0, 1), W = (-1, 0);
    private readonly Point2d[] _directions = [N, E, S, W];

    public void Excute(FileInfo input)
    {
#if Sample
        var grid = sample.Split("\r\n");
#else
        var grid = File.ReadAllLines(input.FullName);
#endif

        _rows = grid.Length;
        _cols = grid[0].Length;
        Point2d start = (0, 0);
        Point2d end = (0, 0);
        List<Point2d> walls = [];

        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                char v = grid[r][c];
                if (v == 'S')
                {
                    start = (c, r);
                }
                else if (v == 'E')
                {
                    end = (c, r);
                }
                else if (v == '#' && r >= 1 && r < _rows - 1 && c >= 1 && c < _cols - 1)
                {
                    walls.Add((c, r));
                }
            }
        }

        var route = Route(grid, start, end);


        List<Point2d[]> cheats = [];

        for (int i = 0; i < route.Count; i++)
        {
            for (int j = i + 1; j < route.Count; j++)
            {
                int dx = route[j].x - route[i].x;
                int dy = route[j].y - route[i].y;
                if (dx == 0 || dy == 0)
                {
                    // compare the distance between every two points on the route
                    var dist = Math.Abs(dx) + Math.Abs(dy);
                    if (dist >= 2 && dist < 3)
                    {
                        // Is there a wall between?
                        Point2d mid = ((route[i].x + route[j].x) / 2, (route[i].y + route[j].y) / 2);
                        Point2d dir = (Math.Sign(dx), Math.Sign(dy));
                        if (walls.Contains(mid))
                        {
                            int saving = j - i - 2;
                            if (saving >= 100)
                            {
                                System.Console.WriteLine($"Cheat at {mid} saves {saving} picoseconds");
                                cheats.Add([mid, (mid.x + dir.x, mid.y + dir.y)]);
                            }

                        }

                    }
                }
            }
        }


       // Render(grid, cheats.Select(c => c[0]).ToArray(), cheats.Select(c => c[1]).ToArray());

        long result = cheats.Count;

        System.Console.WriteLine("Part 1 = {0}", result);


        System.Console.WriteLine("Part 2 = {0}", result);

    }

    private void Render(IList<string> grid, ICollection<Point2d> cheats, ICollection<Point2d> cheats2)
    {
        System.Console.WriteLine(string.Empty.PadRight(50, '-'));
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                if (cheats.Contains((c, r)))
                {
                    Console.Write('1');
                }
                else if (cheats2.Contains((c, r)))
                {
                    Console.Write('2');
                }
                else
                    Console.Write(grid[r][c]);


            }
            System.Console.WriteLine();
        }
    }

    private List<Point2d> Route(string[] grid, Point2d start, Point2d end)
    {
        List<Point2d> route = [start];
        var current = start;
        while (current != end)
        {
            foreach (var direction in _directions)
            {
                var x = current.x + direction.x;
                var y = current.y + direction.y;
                if (grid[y][x] != '#' && !route.Contains((x, y)))
                {
                    current = (x, y);
                    route.Add(current);
                    break;
                }
            }
        }
        return route;
    }

    private string sample = """
###############
#...#...#.....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..E#...#...#
###.#######.###
#...###...#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############
""";
}