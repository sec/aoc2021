namespace aoc2021.src;

internal class Day08 : BaseDay
{
    int Run(bool isPartTwo)
    {
        var input = ReadAllLines().Select(x => x.Split("|", 2, StringSplitOptions.RemoveEmptyEntries)).ToList();
        var result = 0;

        foreach (var note in input)
        {
            var left = note[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var right = note[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (isPartTwo)
            {
                var one = left.Single(x => x.Length == 2);
                var four = left.Single(x => x.Length == 4);
                var seven = left.Single(x => x.Length == 3);
                var eight = left.Single(x => x.Length == 7);

                var three = left.Single(x => x.Length == 5 && one.All(x.Contains)); // 5 + 1
                var five = left.Single(x => x.Length == 5 && four.Except(one).All(x.Contains)); // 5 + 4 - 1
                var two = left.Single(x => x.Length == 5 && x != three && x != five); // 5 + (not 3 and not 5)

                var nine = left.Single(x => x.Length == 6 && four.All(x.Contains)); // 6 + 4
                var zero = left.Single(x => x.Length == 6 && one.All(x.Contains) && x != nine); // 6 + 1 and not 9
                var six = left.Single(x => x.Length == 6 && x != nine && x != zero); // 6 and not 9 and not 0

                var digits = new[] { zero, one, two, three, four, five, six, seven, eight, nine }.ToList();
                var magic = right.Select(d => digits.IndexOf(digits.Single(digit => digit.Length == d.Length && d.All(digit.Contains)))).ToList();

                result += int.Parse(string.Join(string.Empty, magic));
            }
            else
            {
                result += right.Count(x => new[] { 2, 3, 4, 7 }.Contains(x.Length));
            }
        }

        return result;
    }

    protected override object Part1() => Run(false);

    protected override object Part2() => Run(true);
}