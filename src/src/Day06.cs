namespace aoc2021.src;

internal class Day06 : BaseDay
{
    long Calc(long days)
    {
        var input = ReadAllTextSplit(",").Select(int.Parse).ToList();

        var state = new long[9];
        foreach (var i in input)
        {
            state[i]++;
        }

        while (days-- > 0)
        {
            var temp = state[0];
            for (var i = 1; i < state.Length; i++)
            {
                state[i - 1] = state[i];
            }
            state[6] += temp;
            state[state.Length - 1] = temp;
        }

        return state.Sum();
    }

    protected override object Part1() => Calc(80);

    protected override object Part2() => Calc(256);
}