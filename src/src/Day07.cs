namespace aoc2021.src;

internal class Day07 : BaseDay
{
    object Run(bool isPartTwo)
    {
        var input = ReadAllTextSplit(",").Select(int.Parse).ToList();
        var result = new List<int>();
        var max = input.Max();

        var sums = new Dictionary<int, int>();
        for (var dest = 0; dest <= max; dest++)
        {
            sums[dest] = Enumerable.Range(1, dest).Sum();
        }

        for (var dest = 0; dest <= max; dest++)
        {
            var fuel = input
                .Select(crab => Math.Abs(crab - dest))
                .Select(change => isPartTwo ? sums[change] : change)
                .Sum();

            result.Add(fuel);
        }

        return result.Min();
    }

    protected override object Part1() => Run(false);

    protected override object Part2() => Run(true);
}