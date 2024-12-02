using System.Reflection;

var lastPuzzle = Assembly.GetEntryAssembly()
                             .GetTypes()
                             .Where(t => typeof(IPuzzle).IsAssignableFrom(t) && t.IsClass)
                             .Select(type => (type, num: int.Parse(type.Name[6..])))
                             .OrderByDescending(t => t.num)
                             .First();

var puzzle = (IPuzzle)Activator.CreateInstance(lastPuzzle.type);

var input = new FileInfo($"Day{lastPuzzle.num}\\input.txt");
System.Console.WriteLine("Running {0} with input {1}", lastPuzzle.type.Name, input.FullName);

puzzle.Excute(input);