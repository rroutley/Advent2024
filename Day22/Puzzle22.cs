//#define Sample


public class Puzzle22 : IPuzzle
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
            var secret = long.Parse(line);

            for (int i = 0; i < 2000; i++)
            {
                secret = Secret(secret);
            }
            result += secret;

          //  System.Console.WriteLine("{0}: {1}", line, secret);

        }
        System.Console.WriteLine("Part 1 = {0}", result);


        var states = new List<State>();

        result = 0;
        foreach (var line in lines)
        {
            var secret = long.Parse(line);
            var buyer = secret;
            int price = (int)(secret % 10);
            var last = int.MinValue;

            var changeSequence = (int.MinValue, int.MinValue, int.MinValue, int.MinValue);
            for (int i = 0; i < 2000; i++)
            {
                last = price;
                secret = Secret(secret);
                price = (int)(secret % 10);

                changeSequence = (changeSequence.Item2, changeSequence.Item3, changeSequence.Item4, price - last);

                if (i >= 3)
                {
                    states.Add(new State(buyer, i, price, changeSequence));
                }
            }
        }

        var distinct = states.GroupBy(a => a.ChangeSequence).ToArray();

        var maxPrice = 0;
        foreach (var sequence in distinct)
        {
            var bananas = 0;
            foreach (var b in sequence.OrderBy(a => a.Index).GroupBy(a => a.Buyer))
            {
                bananas += b.First().Price;
            }

            if (bananas > maxPrice)
            {
                maxPrice = bananas;
                System.Console.WriteLine(sequence.Key);
            }

        }


        System.Console.WriteLine("Part 2 = {0}", maxPrice);
    }


    record State(long Buyer, int Index, int Price, (int, int, int, int) ChangeSequence);

    private long Secret(long v)
    {
        var a = v * 64;
        v = mixAndPrune(v, a);

        var b = v / 32;
        v = mixAndPrune(v, b);

        var c = v * 2048;
        v = mixAndPrune(v, c);
        return v;
    }


    private long mixAndPrune(long v, long a)
    {
        var x = v ^ a;
        return x % 16777216;
    }

    private string sample = """
1
2
3
2024
""";
}