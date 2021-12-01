namespace aoc2021.src;

internal class Day01 : BaseDay
{
    protected override string Part1()
    {
        var input = this.ReadAllLines().Select(int.Parse).ToList();
        var part1 = 0;
        var prev = input.First();

        foreach (var m in input)
        {
            part1 += m > prev ? 1 : 0;
            prev = m;
        }

        return part1.ToString();
    }

    protected override string Part2()
    {
        var input = this.ReadAllLines().Select(int.Parse).ToList();
        var part2 = 0;
        var prev = input.First();

        for (int i = 0; i < input.Count - 2; i++)
        {
            var sum = input.Skip(i).Take(3).Sum();
            if (sum > prev)
            {
                part2++;
            }
            prev = sum;
        }

        return part2.ToString();
    }
}