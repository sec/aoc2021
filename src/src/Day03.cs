namespace aoc2021.src;

internal class Day03 : BaseDay
{
    (List<int[]>, int) ParseInput()
    {
        var input = ReadAllLines()
            .Select(x => x.Select(c => int.Parse(c.ToString())).ToArray())
            .ToList();

        return (input, input[0].Length);
    }

    static int GetInt<T>(IEnumerable<T> list) => Convert.ToInt32(string.Join(string.Empty, list), 2);

    protected override object Part1()
    {
        var most = new List<char>();
        var least = new List<char>();

        FindMost(true, Logic1);

        return GetInt(most) * GetInt(least);

        void Logic1(bool m, int j, int[] cnt, List<int[]> input)
        {
            most.Add(cnt[1] > cnt[0] ? '1' : '0');
            least.Add(cnt[0] < cnt[1] ? '0' : '1');
        }
    }

    protected override object Part2()
    {
        return FindMost(true, Logic) * FindMost(false, Logic);

        void Logic(bool most, int j, int[] cnt, List<int[]> input)
        {
            var c = most ? cnt[1] >= cnt[0] ? 1 : 0 : cnt[0] <= cnt[1] ? 0 : 1;
            input.RemoveAll(x => x[j] != c);
        }
    }

    int FindMost(bool most, Action<bool, int, int[], List<int[]>> func)
    {
        var (input, size) = ParseInput();

        for (int j = 0; j < size; j++)
        {
            if (input.Count == 1)
            {
                break;
            }

            var cnt = new int[2];
            for (int i = 0; i < input.Count; i++)
            {
                cnt[input[i][j]]++;
            }

            func(most, j, cnt, input);
        }

        return GetInt(input.First());
    }
}