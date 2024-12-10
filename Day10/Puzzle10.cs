// #define Sample

using System.Drawing;
using Point2d = (int x, int y);
public class Puzzle10 : IPuzzle
{

    int rows;
    int cols;

    int[][] directions = [
        [0,-1],
        [1,0],
        [0,1],
        [-1,0]
    ];
    public void Excute(FileInfo input)
    {
#if Sample
        var lines = sample.Split("\r\n");
#else
        var lines = File.ReadAllLines(input.FullName);
#endif

        rows = lines.Length;
        cols = lines[0].Length;

        int[,] grid = new int[cols, rows];

        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                grid[c, r] = char.IsDigit(lines[r][c]) ? lines[r][c] - 48 : -1;


        var trailHeads = new List<Point2d>();
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                if (grid[c, r] == 0)
                    trailHeads.Add((c, r));


        System.Console.WriteLine("Render");
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (grid[c, r] < 0)
                    System.Console.Write('.');
                else
                    System.Console.Write(grid[c, r]);
            }
            System.Console.WriteLine();
        }

        long result = 0;
        foreach (var head in trailHeads)
        {
            var peaks = FindRoutes(grid, head).Count();

            result += peaks;

            System.Console.WriteLine($"Head {head} has {peaks} peaks");
        }

        System.Console.WriteLine("Part 1 = {0}", result);



        System.Console.WriteLine("Part 2 = {0}", result);

    }

    private IEnumerable<Point2d> FindRoutes(int[,] grid, Point2d head)
    {
        var visited = new HashSet<Point2d>();

        var queue = new Queue<Point2d>();
        queue.Enqueue(head);

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();
            var position = state;
            var height = grid[position.x, position.y];

            if (height == 9 && visited.Add(position))
            {
                yield return position;
            }

            foreach (var direction in directions)
            {
                Point2d next = (position.x + direction[0], position.y + direction[1]);

                if (next.x < 0 || next.x >= cols || next.y < 0 || next.y >= rows)
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