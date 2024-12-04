// #define Sample

public class Puzzle4 : IPuzzle
{

    int rows;
    int cols;

    public void Excute(FileInfo input)
    {
#if Sample
        var lines = sample.Split("\r\n");
#else
        var lines = File.ReadAllLines(input.FullName);
#endif

        rows = lines.Length;
        cols = lines[0].Length;

        int result = Part1(lines);

        System.Console.WriteLine("Part 1 = {0}", result);


        result = Part2(lines);

        System.Console.WriteLine("Part 1 = {0}", result);
    }

    private int Part1(string[] lines)
    {
        int[][] directions = [
            [0,-1],
            [1,-1],
            [1,0],
            [1,1],
            [0,1],
            [-1,1],
            [-1,0],
            [-1,-1]
        ];

        const string xmas = "XMAS";
        int result = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                for (int d = 0; d < directions.Length; d++)
                {
                    (var dx, var dy) = directions[d];

                    if (Find(lines, xmas, y, x, dx, dy))
                        result++;
                }
            }
        }

        return result;
    }

    private bool Find(string[] lines, string xmas, int y, int x, int dx, int dy, int offset = 0)
    {
        for (int j = 0; j < xmas.Length; j++)
        {
            int i = j - offset;
            if (x + i * dx < 0 || x + i * dx >= cols)
            {
                return false;
            }

            if (y + i * dy < 0 || y + i * dy >= rows)
            {
                return false;
            }

            if (lines[y + i * dy][x + i * dx] != xmas[j])
            {
                return false;
            }
        }

        return true;
    }

    private int Part2(string[] lines)
    {
        int[][] directions = [
            [1,-1],
            [1,1],
            [-1,1],
            [-1,-1]
        ];

        const string xmas = "MAS";
        int result = 0;


        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                int matches = 0;

                for (int d = 0; d < directions.Length; d++)
                {
                    (var dx, var dy) = directions[d];

                    if (Find(lines, xmas, y, x, dx, dy, 1))
                        matches++;
                }

                // Two matches in different directions
                if (matches == 2)
                {
                    result++;
                }
            }
        }

        return result;
    }

    private string sample = """
MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX
""";
}