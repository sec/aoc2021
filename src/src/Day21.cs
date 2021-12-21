namespace aoc2021.src;

internal class Day21 : BaseDay
{
    int Practice()
    {
        var input = ReadAllLinesSplit(" ");
        var roll = 0;
        var scores = new int[2];
        var position = input.Select(x => int.Parse(x.Last()) - 1).ToArray();

        while (true)
        {
            for (int player = 0; player < 2; player++)
            {
                var sum = Enumerable.Range(0, 3).Sum(_ => (roll++ % 100) + 1);

                position[player] = (position[player] + sum) % 10;
                scores[player] += position[player] + 1;

                if (scores.Any(i => i >= 1000))
                {
                    return scores.Min() * roll;
                }
            }
        }
    }

    protected override object Part1() => Practice();

    protected override object Part2()
    {
        return -1;
    }
}