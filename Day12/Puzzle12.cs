// #define Sample

using System.Diagnostics;
using System.Text;
using Point2d = (int x, int y);
using Segment = (int x1, int y1, int x2, int y2, Orentation orentation);

enum Orentation { Horizontal, Vertical };


public class Puzzle12 : IPuzzle
{
    private int _rows;
    private int _cols;

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

        HashSet<Point2d> notVisited = new();
        for (int r = 0; r < _rows; r++)
            for (int c = 0; c < _cols; c++)
                notVisited.Add((c, r));


        var regions = new List<Region>();
        long price = 0;

        while (notVisited.Count > 0)
        {
            var current = notVisited.First();

            var region = FindRegion(grid, current);
            System.Console.WriteLine($"Region {region.Plant} with area {region.Area} and perimeter {region.Perimeter}.");

            price += region.Area * region.Perimeter;

            regions.Add(region);


            // Remove the region's points for the set of unvisited
            notVisited.ExceptWith(region.Points);
        }

        long result = price;
        System.Console.WriteLine("Part 1 = {0}", result);



        long price2 = 0;
        int k = 0;
        foreach (var region in regions)
        {
            //    File.WriteAllText(Path.Combine(input.DirectoryName, $"region{k++:000} {region.Plant}.svg"), region.ToSvg());
            System.Console.WriteLine($"{k:000} Region {region.Plant} with area {region.Area} and sides {region.Sides()}.");

            price2 += region.Area * region.Sides();
        }
        result = price2;

        System.Console.WriteLine("Part 2 = {0}", result);

    }

    private Region FindRegion(string[] grid, Point2d current)
    {
        HashSet<Point2d> visited = [];
        List<Segment> segments = new();
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
                    segments.Add(NewSegment(curr, (dx, dy)));
                    continue;
                }

                if (visited.Contains(next))
                    continue;

                if (plant != grid[next.y][next.x])
                {
                    perimeter++;
                    segments.Add(NewSegment(curr, (dx, dy)));
                    continue;
                }

                queue.Enqueue(next);
            }
        }
        return new Region(plant, visited.Count, perimeter, visited, segments);
    }

    private Segment NewSegment(Point2d curr, (int dx, int dy) direction)
    {
        if (direction == N) return (curr.x, curr.y, curr.x + 1, curr.y, Orentation.Horizontal);
        else if (direction == E) return (curr.x + 1, curr.y, curr.x + 1, curr.y + 1, Orentation.Vertical);
        else if (direction == S) return (curr.x, curr.y + 1, curr.x + 1, curr.y + 1, Orentation.Horizontal);
        else if (direction == W) return (curr.x, curr.y, curr.x, curr.y + 1, Orentation.Vertical);
        else throw new UnreachableException();
    }

    record Region(char Plant, int Area, int Perimeter, HashSet<Point2d> Points, List<Segment> Segments)
    {
        private int _sides = 0;
        public int Sides()
        {
            if (_sides != 0) return _sides;

            var verticalSegments = Segments.Where(s => s.orentation == Orentation.Vertical)
                                           .OrderBy(s => s.y1)
                                           .GroupBy(s => s.x1)
                                           .OrderBy(g => g.Key)
                                           .ToArray();

            var verticalSides = CombineSegments(verticalSegments).ToList();

            var horizontalSegments = Segments.Where(s => s.orentation == Orentation.Horizontal)
                                             .OrderBy(s => s.x1)
                                             .GroupBy(s => s.y1)
                                             .OrderBy(g => g.Key)
                                             .ToArray();

            var horizontalSides = CombineSegments(horizontalSegments).ToList();


            // Split any sides that intesect.
            int intercestionCount = 0;
            foreach (var h in horizontalSides.ToArray())
            {
                var vs = verticalSides.Where(v => h.x1 < v.x1 && v.x1 < h.x2
                                           && v.y1 < h.y1 && h.y1 < v.y2);
                foreach (var v in vs)
                {
                    // TODO: Split h and v
                    // vertical.Remove(v);
                    // horizontal.Remove(h);
                    // vertical.Add()
                    intercestionCount += 2;
                }
            }

            _sides = verticalSides.Union(horizontalSides).Count() + intercestionCount;

            return _sides;

            static IEnumerable<Segment> CombineSegments(IGrouping<int, Segment>[] vertical)
            {
                foreach (var group in vertical)
                {
                    var start = group.First();
                    Point2d endLast = (start.x2, start.y2);

                    foreach (var segment in group.Skip(1))
                    {
                        if (segment.x1 != endLast.x || segment.y1 != endLast.y)
                        {
                            yield return (start.x1, start.y1, endLast.x, endLast.y, start.orentation);

                            start = segment;
                        }

                        endLast = (segment.x2, segment.y2);
                    }

                    yield return (start.x1, start.y1, endLast.x, endLast.y, start.orentation);

                }

            }
        }


        public string ToSvg()
        {
            var template = $"""
<?xml version="1.0" standalone="no"?>
<svg width="3000" height="3000" version="1.1" xmlns="http://www.w3.org/2000/svg">
::
</svg>
""";
            StringBuilder lines = new();
            int sf = 20;

            foreach (var segment in Segments)
            {
                lines.AppendLine($"""
 <line x1="{sf * segment.x1}" x2="{sf * segment.x2}" y1="{sf * segment.y1}" y2="{sf * segment.y2}" stroke="orange" stroke-width="5"/>
""");
            }

            return template.Replace("::", lines.ToString());

        }
    }

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
