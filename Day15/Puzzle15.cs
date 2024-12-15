// #define Sample

using System.Diagnostics;
using Point2d = (int x, int y);
public class Puzzle15 : IPuzzle
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

        var grid = ParseGrid(lines, out var robot);

        var instructions = ParseInstructions(lines);

        Render(grid, robot, "Start");

        foreach (var instr in instructions)
        {
            robot = Move(grid, robot, instr);
        }

        Render(grid, robot, "End");




        long result = Score(grid);

        System.Console.WriteLine("Part 1 = {0}", result);



        System.Console.WriteLine("Part 2 = {0}", result);

    }

    private long Score(List<char[]> grid)
    {
        long score = 0;
        for (int r = 0; r < _rows; r++)
            for (int c = 0; c < _cols; c++)
                if (grid[r][c] == 'O')
                    score += r * 100 + c;

        return score;
    }

    private Point2d Move(List<char[]> grid, Point2d robot, char instr)
    {
        var direction = instr switch
        {
            '^' => N,
            '>' => E,
            'v' => S,
            '<' => W,
            _ => throw new UnreachableException(),
        };

        if (!TryMove(direction, out int steps))
            return robot;

        for (int i = steps; i >= 0; i--)
        {
            Point2d to = (robot.x + i * direction.x, robot.y + i * direction.y);
            Point2d from = (robot.x + (i - 1) * direction.x, robot.y + (i - 1) * direction.y);

            grid[to.y][to.x] = grid[from.y][from.x];

        }
        grid[robot.y][robot.x] = '.';
        return (robot.x + direction.x, robot.y + direction.y);



        bool TryMove((int x, int y) direction, out int steps)
        {
            steps = 0;
            for (int i = 1; ; i++)
            {
                (var x, var y) = (robot.x + i * direction.x, robot.y + i * direction.y);
                char v = grid[y][x];
                if (v == '#')
                {
                    // Blocked
                    return false;
                }
                else if (v == '.')
                {
                    // Done
                    steps = i;
                    return true;
                }
                else if (v == 'O')
                {
                    // Keep Going
                }
            }
        }
    }

    private List<char[]> ParseGrid(string[] lines, out Point2d robot)
    {
        robot = (0, 0);
        _cols = lines[0].Length;
        var grid = new List<char[]>();

        for (int r = 0; r < lines.Length; r++)
        {
            string line = lines[r];
            if (line == string.Empty)
                break;

            grid.Add(line.ToArray());

            int c = line.IndexOf('@');
            if (c >= 0)
            {
                robot = (c, r);
                grid[r][c] = '.';
            }

        }
        _rows = grid.Count;

        return grid;
    }

    private List<char> ParseInstructions(string[] lines)
    {
        var instructions = new List<char>();
        for (int i = _rows + 1; i < lines.Length; i++)
        {
            instructions.AddRange(lines[i].ToArray());
        }

        return instructions;
    }

    private void Render(List<char[]> grid, Point2d robot, string caption)
    {
        System.Console.WriteLine(caption);
        System.Console.WriteLine(string.Empty.PadRight(50, '-'));

        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                if (robot.y == r && robot.x == c)
                {
                    var clr = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write('@');
                    Console.ForegroundColor = clr;
                }
                else
                    Console.Write(grid[r][c]);
            }
            Console.WriteLine();
        }
    }

    private string sample = """
##########
#..O..O.O#
#......O.#
#.OO..O.O#
#..O@..O.#
#O#..O...#
#O..O..O.#
#.OO.O.OO#
#....O...#
##########

<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
""";
}