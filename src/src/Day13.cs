namespace aoc2021.src;

internal class Day13 : BaseDay
{
    record Dot(int X, int Y);
    record Instruction(char P, int N);

    readonly static Dictionary<char, Func<Dot, Instruction, Dot>> _map = new()
    {
        { 'x', (d, fold) => d.X > fold.N ? d with { X = d.X - 2 * (d.X - fold.N) } : d },
        { 'y', (d, fold) => d.Y > fold.N ? d with { Y = d.Y - 2 * (d.Y - fold.N) } : d }
    };

    object Run(bool isPartTwo)
    {
        var dots = new HashSet<Dot>();
        var fold = new List<Instruction>();

        foreach (var input in ReadAllLines(true))
        {
            if (input.StartsWith("fold"))
            {
                var instr = input.Split('=');
                fold.Add(new Instruction(instr[0].Last(), int.Parse(instr[1])));
            }
            else
            {
                var xy = input.Split(',');
                dots.Add(new Dot(int.Parse(xy[0]), int.Parse(xy[1])));
            }
        }

        return isPartTwo ? Print(fold.Select(i => dots = Fold(dots, i)).ToList().Last()) : Fold(dots, fold.First()).Distinct().Count();
    }

    private static HashSet<Dot> Fold(HashSet<Dot> dots, Instruction fold) => dots.Select(i => _map[fold.P](i, fold)).ToHashSet();

    private static string Print(HashSet<Dot> dots)
    {
        var sb = new StringBuilder();

        sb.AppendLine();
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 8 * 5; x++)
            {
                sb.Append(dots.Contains(new Dot(x, y)) ? "#" : " ");
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    protected override object Part1() => Run(false);

    protected override object Part2() => Run(true);
}