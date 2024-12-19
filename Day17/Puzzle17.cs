#define Sample

public class Puzzle17 : IPuzzle
{

    public void Excute(FileInfo input)
    {
#if Sample
        var lines = sample.Split("\r\n");
#else
        var lines = File.ReadAllLines(input.FullName);
#endif

        int[] code = [2, 4, 1, 5, 7, 5, 1, 6, 0, 3, 4, 1, 5, 5, 3, 0];


        var result = string.Join(',', NewMethod(44374556, code));
        System.Console.WriteLine("Part 1 = {0}", result);


        Parallel.For(int.MaxValue, long.MaxValue, (a, state) =>
        {
            var output = NewMethod(a, code);
            if (output.SequenceEqual(code))
            {
                System.Console.WriteLine("Part 2 = {0}", a);
                state.Break();
            }
        });

    }

    private static IEnumerable<int> NewMethod(long input, int[] code)
    {
        long a = input, b = 0, c = 0;

        int ip = 0;
        while (ip < code.Length)
        {
            var instr = code[ip];
            var operand = code[ip + 1];
            ip += 2;
            var literal = operand;
            var combo = operand switch
            {
                4 => a,
                5 => b,
                6 => c,
                7 => operand,
                _ => operand
            };

            switch (instr)
            {
                case 0: // adv
                    a = a / (int)Math.Pow(2, combo);
                    break;
                case 1: // bxl
                    b ^= literal;
                    break;
                case 2: // bst
                    b = combo % 8;
                    break;
                case 3: // jnz
                    if (a != 0)
                    {
                        ip = literal;
                    }
                    break;
                case 4: // bxc
                    b = b ^ c;
                    break;
                case 5: // out
                    yield return (int)(combo % 8);
                    break;
                case 6: // bdv
                    b = a / (int)Math.Pow(2, combo);
                    break;
                case 7: // cdv
                    c = a / (int)Math.Pow(2, combo);
                    break;
            }


        }
    }

    private string sample = """
Register A: 729
Register B: 0
Register C: 0

Program: 0,1,5,4,3,0    
""";
}