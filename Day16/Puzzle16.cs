//#define Sample

using Point2d = (int x, int y);

public class Puzzle16 : IPuzzle
{
    int _rows;
    int _cols;

    private readonly static Point2d N = (0, -1), E = (1, 0), S = (0, 1), W = (-1, 0);
    private readonly Point2d[] _directions = [N, E, S, W];

    public void Excute(FileInfo input)
    {
#if Sample
        var lines = sample.Split("\r\n");
#else
        var lines = File.ReadAllLines(input.FullName);
#endif
        _rows = lines.Length;
        _cols = lines[0].Length;
        var grid = new char[_cols, _rows];

        Point2d start = (0, 0);
        Point2d heading = E;
        Point2d end = (0, 0);

        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                if (lines[r][c] == 'S')
                {
                    start = (c, r);
                    grid[c, r] = '.';
                }
                else if (lines[r][c] == 'E')
                {
                    end = (c, r);
                    grid[c, r] = '.';
                }
                else
                {
                    grid[c, r] = lines[r][c];
                }
            }
        }

        Render(grid);

        PathToEnd(grid, start, heading, end);


        long result = 0;

        System.Console.WriteLine("Part 1 = {0}", result);



        System.Console.WriteLine("Part 2 = {0}", result);

    }

    private void PathToEnd(char[,] grid, Point2d start, Point2d initialHeading, Point2d end)
    {
        var queue = new Queue<State>();
        queue.Enqueue(new State(start, initialHeading, 0, 0, []));
        //HashSet<(Point2d, Point2d)> visited = new();

        int minCost = int.MaxValue;

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();

            var current = state.Position;
            var heading = state.Heading;
            var visited = state.visited;
            if (!visited.Add((current, heading)))
                continue;

            if (current == end)
            {
                int score = 1000 * state.turns + state.steps;

                if (score < minCost)
                {
                    minCost = score;
                    System.Console.WriteLine($"steps={state.steps} turns={state.turns} score={score}");
                }
            }

            Point2d ahead = (current.x + heading.x, current.y + heading.y);
            if (grid[ahead.x, ahead.y] == '.')
            {
                queue.Enqueue(new State(ahead, heading, state.steps + 1, state.turns, [.. visited]));
            }

            Point2d clockwise = (current.x + heading.y, current.y + heading.x);
            if (grid[clockwise.x, clockwise.y] == '.')
            {
                queue.Enqueue(new State(current, (heading.y, heading.x), state.steps, state.turns + 1, [.. visited]));
            }

            Point2d counterClockwise = (current.x - heading.y, current.y - heading.x);
            if (grid[counterClockwise.x, counterClockwise.y] == '.')
            {
                queue.Enqueue(new State(current, (-heading.y, -heading.x), state.steps, state.turns + 1, [.. visited]));
            }


        }




    }

    record State(Point2d Position, Point2d Heading, int steps, int turns, HashSet<(Point2d, Point2d)> visited);
    private void Render(char[,] grid)
    {
        System.Console.WriteLine(string.Empty.PadRight(50, '-'));
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                Console.Write(grid[c, r]);

            }
            System.Console.WriteLine();
        }
    }

    private string sample = """
#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
################# 
""";
}