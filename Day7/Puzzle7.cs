// #define Sample

public class Puzzle7 : IPuzzle
{

    public void Excute(FileInfo input)
    {
#if Sample
        var lines = sample.Split("\r\n");
#else
        var lines = File.ReadAllLines(input.FullName);
#endif


        long result = 0;
        foreach (var line in lines)
        {
            (var left, var right) = line.Split(':');

            var ans = long.Parse(left);
            var numbers = right.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            bool hasSolution = false;
            foreach (var solution in Solutions(ans, numbers))
            {
                System.Console.WriteLine(solution);
                hasSolution = true;
            }

            if (hasSolution)
                result += ans;
        }

        System.Console.WriteLine("Part 1 = {0}", result);

    }

    private IEnumerable<string> Solutions(long ans, int[] numbers)
    {
        var operatorsNeeded = numbers.Length - 1;
        // Only two operations so use binary
        var combinations = 1 << operatorsNeeded;

        for (int i = 0; i < combinations; i++)
        {
            long result = numbers[0];
            var solution = $"{ans} = {numbers[0]}";
            var bitPattern = i;
            for (int j = 0; j < operatorsNeeded; j++)
            {
                var bit = bitPattern & 1;
                bitPattern >>= 1;

                if (bit == 0)
                {
                    result += numbers[j + 1];
                    solution += " + ";
                }
                else
                {
                    result *= numbers[j + 1];
                    solution += " * ";
                }
                solution += numbers[j + 1];
            }

            if (result == ans)
            {
                yield return solution;
            }
        }


    }

    private string sample = """
190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20    
""";
}