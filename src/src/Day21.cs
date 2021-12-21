namespace aoc2021.src;

internal class Day21 : BaseDay
{
    static readonly (int, int, int)[] _diracDiceRolls = new[] { (1, 1, 1), (1, 1, 2), (1, 1, 3), (1, 2, 1), (1, 2, 2), (1, 2, 3), (1, 3, 1), (1, 3, 2), (1, 3, 3), (2, 1, 1), (2, 1, 2), (2, 1, 3), (2, 2, 1), (2, 2, 2), (2, 2, 3), (2, 3, 1), (2, 3, 2), (2, 3, 3), (3, 1, 1), (3, 1, 2), (3, 1, 3), (3, 2, 1), (3, 2, 2), (3, 2, 3), (3, 3, 1), (3, 3, 2), (3, 3, 3) };
    static readonly Dictionary<(int, int, int, int), (long, long)> _cache = new();

    int[] StartPosition => ReadAllLinesSplit(" ").Select(x => int.Parse(x.Last()) - 1).ToArray();

    int Practice()
    {
        var roll = 0;
        var scores = new int[2];
        var position = StartPosition.ToArray();

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

    (long P1, long P2) PlayDice(int p1score, int p2score, int p1pos, int p2pos)
    {
        var cacheKey = (p1score, p2score, p1pos, p2pos);

        if (_cache.TryGetValue(cacheKey, out var cache))
        {
            return cache;
        }

        if (p1score >= 21)
        {
            return (1, 0);
        }

        if (p2score >= 21)
        {
            return (0, 1);
        }

        var score1 = 0L;
        var score2 = 0L;

        foreach (var (a, b, c) in _diracDiceRolls)
        {
            var new_pos = (p1pos + a + b + c) % 10;
            var new_score = p1score + new_pos + 1;

            var (p2, p1) = PlayDice(p2score, new_score, p2pos, new_pos);

            score1 += p1;
            score2 += p2;
        }

        return _cache[cacheKey] = (score1, score2);
    }

    protected override object Part1() => Practice();

    protected override object Part2() => PlayDice(0, 0, StartPosition[0], StartPosition[1]).P1;
}