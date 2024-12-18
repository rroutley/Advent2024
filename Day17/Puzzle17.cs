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

        int a = 44374556, b = 0, c = 0;
        int[] code = [2, 4, 1, 5, 7, 5, 1, 6, 0, 3, 4, 1, 5, 5, 3, 0];



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
                    a = a / (int)(Math.Pow(2, combo));
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
                    System.Console.Write(combo % 8);
                    System.Console.Write(',');
                    break;
                case 6: // bdv
                    b = a / (int)(Math.Pow(2, combo));
                    break;
                case 7: // cdv
                    c = a / (int)(Math.Pow(2, combo));
                    break;
            }


        }
        System.Console.WriteLine();

        System.Console.WriteLine($"a={a} b={b} c={c}");






        long result = 0;

        System.Console.WriteLine("Part 1 = {0}", result);



        System.Console.WriteLine("Part 2 = {0}", result);

    }


    private string sample = """
Register A: 729
Register B: 0
Register C: 0

Program: 0,1,5,4,3,0    
""";
}