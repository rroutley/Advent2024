// #define Sample

using Point2d = (int x, int y);

public class Puzzle12 : IPuzzle
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
        var grid = sample.Split("\r\n");
#else
        var grid = File.ReadAllLines(input.FullName);
#endif

        _rows = grid.Length;
        _cols = grid[0].Length;

        HashSet<Point2d> notVisited = new();
        for (int r = 0; r < _rows; r++)
            for (int c = 0; c < _cols; c++)
                notVisited.Add((c, r));


        long price = 0;
        while (notVisited.Count > 0)
        {
            var current = notVisited.First();

            var region = FindRegion(grid, current);
            System.Console.WriteLine($"Region {region.Plant} with area {region.Area} and perimeter {region.Perimeter}.");

            price += region.Area * region.Perimeter;

            // Remove the region's points for the set of unvisited
            notVisited.ExceptWith(region.Points);
        }

        long result = price;
        System.Console.WriteLine("Part 1 = {0}", result);



        System.Console.WriteLine("Part 2 = {0}", result);

    }

    private Region FindRegion(string[] grid, Point2d current)
    {
        HashSet<Point2d> visited = [];
        int perimeter = 0;
        var plant = grid[current.y][current.x];

        Queue<Point2d> queue = new();
        queue.Enqueue(current);

        while (queue.Count > 0)
        {
            var curr = queue.Dequeue();

            if (!visited.Add(curr))
            {
                continue;
            }

            foreach ((int dx, int dy) in _directions)
            {
                Point2d next = (curr.x + dx, curr.y + dy);
                if (next.x < 0 || next.x >= _cols || next.y < 0 || next.y >= _rows)
                {
                    perimeter++;
                    continue;
                }

                if (visited.Contains(next))
                    continue;

                if (plant != grid[next.y][next.x])
                {
                    perimeter++;
                    continue;
                }

                queue.Enqueue(next);
            }
        }
        return new Region(plant, visited.Count, perimeter, visited);
    }

    record Region(char Plant, int Area, int Perimeter, HashSet<Point2d> Points);

    private string sample = """
RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE
""";
}