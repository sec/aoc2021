namespace aoc2021;

internal abstract class BaseDay
{
    private string _input = string.Empty;

    private void Solve(bool test, Func<string> part)
    {
        string day = GetType().Name[^2..];
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Environment.CurrentDirectory;
        var inputpath = Path.Combine(test ? "input_test" : "input", $"day{day}.txt");
        var inputfile = Path.Combine(path, inputpath);

        if (File.Exists(inputfile))
        {
            _input = File.ReadAllText(inputfile);

            Console.WriteLine($"\t{(test ? "Test" : "Real")} Day {day} - {part()}");
            Console.WriteLine();
        }
    }

    private void RunSolve(byte n, Func<string> part)
    {
        Console.WriteLine($"Part {n}");
        Solve(true, part);
        Solve(false, part);
    }

    public void SolvePart1() => RunSolve(1, Part1);
    public void SolvePart2() => RunSolve(2, Part2);

    protected string ReadAllText() => _input;
    protected string[] ReadAllLines(bool removeEmpty = false) => _input.Split(Environment.NewLine, removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
    protected string[] ReadAllTextSplit(string chars) => ReadAllText().Split(chars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    protected IEnumerable<string[]> ReadAllLinesSplit(string chars) => ReadAllLines().Select(x => x.Split(chars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

    protected abstract string Part1();
    protected abstract string Part2();
}