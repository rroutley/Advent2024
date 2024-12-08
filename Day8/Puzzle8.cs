// #define Sample

using Point2d = (int x, int y);

public class Puzzle8 : IPuzzle
{

    int rows;
    int cols;

    public void Excute(FileInfo input)
    {
#if Sample
        var grid = sample.Split("\r\n");
#else
        var grid = File.ReadAllLines(input.FullName);
#endif

        var antennas = LoadAntenna(grid).ToArray();


        var antiNodes = new HashSet<Point2d>();
        foreach (var antenna in antennas.GroupBy(a => a.Freq))
        {
            foreach (var a in ComputeAntinodes(antenna.ToList()))
            {
                if (IsWithinGrid(a))
                    antiNodes.Add(a);
            }
        }

        Dump(antennas, antiNodes);

        long result = antiNodes.Count;

        Console.WriteLine("Part 1 = {0}", result);


        antiNodes = new HashSet<Point2d>();
        foreach (var antenna in antennas.GroupBy(a => a.Freq))
        {
            foreach (var a in ComputeAntinodes2(antenna.ToList()))
            {
                antiNodes.Add(a);
            }
        }
        Dump(antennas, antiNodes);

        result = antiNodes.Count;

        Console.WriteLine("Part 2 = {0}", result);

    }

    private bool IsWithinGrid(Point2d a)
    {
        return a.x >= 0 && a.x < cols && a.y >= 0 && a.y < rows;
    }

    private void Dump(IEnumerable<Antenna> antennas, HashSet<(int x, int y)> antiNodes)
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                var a = antennas.FirstOrDefault(a => a.X == x && a.Y == y);
                if (a != null)
                {
                    Console.Write(a.Freq);
                }
                else if (antiNodes.Contains((x, y)))
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('.');
                }

            }
            Console.WriteLine();
        }
    }

    private IEnumerable<Antenna> LoadAntenna(string[] grid)
    {

        rows = grid.Length;
        cols = grid[0].Length;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                if (grid[y][x] != '.')
                {
                    yield return new Antenna(x, y, grid[y][x]);
                }
            }
        }
    }

    private IEnumerable<Point2d> ComputeAntinodes(IList<Antenna> antenna)
    {
        for (int i = 0; i < antenna.Count; i++)
        {
            var lhs = antenna[i];
            for (int j = i + 1; j < antenna.Count; j++)
            {
                var rhs = antenna[j];

                var dx = rhs.X - lhs.X;
                var dy = rhs.Y - lhs.Y;

                yield return (lhs.X - dx, lhs.Y - dy);
                yield return (rhs.X + dx, rhs.Y + dy);
            }
        }
    }


    private IEnumerable<Point2d> ComputeAntinodes2(IList<Antenna> antenna)
    {
        for (int i = 0; i < antenna.Count; i++)
        {
            var lhs = antenna[i];
            for (int j = i + 1; j < antenna.Count; j++)
            {
                var rhs = antenna[j];

                var dx = rhs.X - lhs.X;
                var dy = rhs.Y - lhs.Y;

                yield return (lhs.X, lhs.Y);

                for (int t = 1; ; t++)
                {
                    Point2d a1 = (lhs.X - t * dx, lhs.Y - t * dy);
                    if (IsWithinGrid(a1))
                        yield return a1;

                    Point2d a2 = (lhs.X + t * dx, lhs.Y + t * dy);
                    if (IsWithinGrid(a2))
                        yield return a2;

                    if (!IsWithinGrid(a1) && !IsWithinGrid(a2))
                        break;
                }

            }
        }
    }


    record Antenna(int X, int Y, char Freq);


    private string sample = """
............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............    
""";
}
