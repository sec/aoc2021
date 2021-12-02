namespace aoc2021.src;

internal class Day01 : BaseDay
{
    protected int[] ParseInput() => ReadAllLines().Select(int.Parse).ToArray();

    protected override object Part1()
    {
        var input = ParseInput();

        return input.Where((c, i) => i > 0 && c > input[i - 1]).Count();
    }

    protected override object Part2()
    {
        var input = ParseInput();

        return input.Where((c, i) => (i > 0 && i < input.Length - 2) && input[i..(i + 3)].Sum() > input[(i - 1)..(i + 2)].Sum()).Count();
    }
}