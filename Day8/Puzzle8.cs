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

        var antennas = LoadAntenna(grid);


        var antiNodes = new HashSet<Point2d>();
        foreach (var antenna in antennas.GroupBy(a => a.Freq))
        {
            foreach (var a in ComputeAntinodes(antenna.ToList()))
            {
                if (a.x >= 0 && a.x < cols && a.y >= 0 && a.y < rows)
                    antiNodes.Add(a);
            }
        }



        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                var a = antennas.FirstOrDefault(a => a.X == x && a.Y == y);
                if (antiNodes.Contains((x, y)))
                {
                    System.Console.Write('#');
                }
                else if (a != null)
                {
                    System.Console.Write(a.Freq);
                }
                else
                {
                    System.Console.Write('.');
                }

            }
            System.Console.WriteLine();
        }

        long result = antiNodes.Count;

        System.Console.WriteLine("Part 1 = {0}", result);



        System.Console.WriteLine("Part 2 = {0}", result);

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
