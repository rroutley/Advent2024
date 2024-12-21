// #define Sample

using Point2d = (int x, int y);

public class Puzzle18 : IPuzzle
{
    int _rows;
    int _cols;
    int _take;

    private readonly static Point2d N = (0, -1), E = (1, 0), S = (0, 1), W = (-1, 0);
    private readonly Point2d[] _directions = [N, E, S, W];

    public void Excute(FileInfo input)
    {
#if Sample
        var lines = sample.Split("\r\n");
        _rows = 7;
        _cols = 7;
        _take = 12;
#else
        var lines = File.ReadAllLines(input.FullName);

        _rows = 71;
        _cols = 71;
        _take = 1024;
#endif

        HashSet<Point2d> points = new();

        foreach (var pair in lines.Take(_take))
        {
            (var x, var y) = pair.Split(',').Select(int.Parse).ToArray();
            points.Add((x, y));
        }

        Point2d start = (0, 0);
        Point2d end = (_cols - 1, _rows - 1);

        int steps = ShortestPath(points, start, end);


        Render(points);

        long result = steps;

        System.Console.WriteLine("Part 1 = {0}", result);



        foreach (var pair in lines.Skip(_take))
        {
            (var x, var y) = pair.Split(',').Select(int.Parse).ToArray();
            points.Add((x, y));

            if (ShortestPath(points, start, end) == int.MaxValue)
            {
                System.Console.WriteLine("Part 2 = {0}", (x, y));
                break;
            }
            System.Console.Write('.');
        }



    }

    private int ShortestPath(HashSet<Point2d> points, Point2d start, Point2d end)
    {
        var queue = new PriorityQueue<Point2d, int>();
        queue.Enqueue(start, 0);

        Dictionary<Point2d, int> distance = new();
        Dictionary<Point2d, Point2d> path = new();

        for (int r = 0; r < _rows; r++)
            for (int c = 0; c < _cols; c++)            
                distance[(c, r)] = int.MaxValue;
            

        distance[start] = 0;

        while (queue.TryDequeue(out var current, out _))
        {

            foreach (var direction in _directions)
            {
                Point2d next = (current.x + direction.x, current.y + direction.y);
                if (next.x < 0 || next.x >= _cols || next.y < 0 || next.y >= _rows)
                    continue;

                if (points.Contains(next))
                {
                    continue;
                }

                var alt = distance[current] + 1;
                if (alt < distance[next])
                {

                    path[next] = current;
                    distance[next] = alt;

                    queue.Remove(next, out _, out _);
                    queue.Enqueue(next, alt);

                }
            }
        }

        // // Retrace path
        // var x = end;
        // while (x != start)
        // {
        //     grid[x.x, x.y] = 'o';
        //     x = prev[x];
        // }

        return distance[end];

    }

    private void Render(ISet<Point2d> grid)
    {
        System.Console.WriteLine(string.Empty.PadRight(50, '-'));
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                if (grid.Contains((c, r)))
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('.');
                }


            }
            System.Console.WriteLine();
        }
    }

    private string sample = """
5,4
4,2
4,5
3,0
2,1
6,3
2,4
1,5
0,6
3,3
2,6
5,1
1,2
5,5
2,5
6,5
1,4
0,4
6,4
1,1
6,1
1,0
0,5
1,6
2,0    
""";
}