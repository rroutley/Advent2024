// #define Sample

using Point2d = (int x, int y);
public class Puzzle10 : IPuzzle
{
    private int _rows;
    private int _cols;

    private readonly Point2d[] _directions = [
        (0,-1),
        (1,0),
        (0,1),
        (-1,0)
    ];

    public void Excute(FileInfo input)
    {
#if Sample
        var lines = sample.Split("\r\n");
#else
        var lines = File.ReadAllLines(input.FullName);
#endif

        var grid = ParseGrid(lines);

        var trailHeads = FindTrailHeads(grid);

        Render(grid);

        long result = 0;
        foreach (var head in trailHeads)
        {
            var peaks = FindRoutes(grid, head).Distinct().Count();

            result += peaks;

            Console.WriteLine($"Head {head} has {peaks} peaks");
        }

        Console.WriteLine("Part 1 = {0}", result);


        result = 0;
        foreach (var head in trailHeads)
        {
            var routes = FindRoutes(grid, head).Count();

            result += routes;

            Console.WriteLine($"Head {head} has {routes} routes");
        }
        Console.WriteLine("Part 2 = {0}", result);

    }

    private int[,] ParseGrid(string[] lines)
    {
        _rows = lines.Length;
        _cols = lines[0].Length;

        int[,] grid = new int[_cols, _rows];

        for (int r = 0; r < _rows; r++)
            for (int c = 0; c < _cols; c++)
                grid[c, r] = char.IsDigit(lines[r][c]) ? lines[r][c] - 48 : -1;
        return grid;
    }

    private List<(int x, int y)> FindTrailHeads(int[,] grid)
    {
        var trailHeads = new List<Point2d>();
        for (int r = 0; r < _rows; r++)
            for (int c = 0; c < _cols; c++)
                if (grid[c, r] == 0)
                    trailHeads.Add((c, r));
        return trailHeads;
    }

    private void Render(int[,] grid)
    {
        Console.WriteLine("Render");
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                if (grid[c, r] < 0)
                    Console.Write('.');
                else
                    Console.Write(grid[c, r]);
            }
            Console.WriteLine();
        }
    }

    private IEnumerable<Point2d> FindRoutes(int[,] grid, Point2d head)
    {
        var queue = new Queue<Point2d>();
        queue.Enqueue(head);

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();
            var position = state;
            var height = grid[position.x, position.y];

            if (height == 9)
            {
                yield return position;
            }

            foreach (var (dx, dy) in _directions)
            {
                Point2d next = (position.x + dx, position.y + dy);

                if (next.x < 0 || next.x >= _cols || next.y < 0 || next.y >= _rows)
                    continue;


                if (grid[next.x, next.y] == height + 1)
                {
                    queue.Enqueue(next);
                }
            }
        }

    }




    private string sample = """
89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732
""";
}