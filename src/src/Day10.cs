namespace aoc2021.src;

internal class Day10 : BaseDay
{
    static readonly List<int> _syntaxScore = new[] { 3, 57, 1197, 25137 }.ToList();
    static readonly List<int> _closeScore = new[] { 1, 2, 3, 4 }.ToList();

    static readonly List<char> _open = new[] { '(', '[', '{', '<' }.ToList();
    static readonly List<char> _close = new[] { ')', ']', '}', '>' }.ToList();

    static bool IsOpening(char c) => _open.Contains(c);

    static bool IsClosing(char c) => _close.Contains(c);

    static bool BracketsMatch(char open, char close) => close == _close[_open.IndexOf(open)];

    static bool OpenCloseMatch(string chunk) => _close.Select(chunk.CountChar).SequenceEqual(_open.Select(chunk.CountChar));

    static long CloseScore(List<char> chunk) => chunk.Aggregate(0L, (score, s) => score * 5 + _closeScore[_close.IndexOf(s)]);

    static bool IsCorrupted(string chunk, out char wrong)
    {
        var s = new Stack<char>();

        for (var i = 0; i < chunk.Length; i++)
        {
            var c = chunk[i];

            if (IsOpening(c))
            {
                s.Push(c);
            }
            else if (IsClosing(c))
            {
                if (s.Count == 0 || !BracketsMatch(s.Pop(), c))
                {
                    wrong = c;
                    return true;
                }
            }
        }

        wrong = ' ';
        return false;
    }

    long Run(bool isPartTwo)
    {
        var input = ReadAllLines(true).ToList();
        var part1 = 0L;
        var part2 = new List<long>();

        foreach (var chunk in input)
        {
            if (IsCorrupted(chunk, out var wrong))
            {
                part1 += _syntaxScore[_close.IndexOf(wrong)];
            }
            else if (isPartTwo)
            {
                var newChunk = chunk;
                var missing = new List<char>();

                // just brute force...
                while (!OpenCloseMatch(newChunk))
                {
                    foreach (var maybe in _close)
                    {
                        if (!IsCorrupted(newChunk + maybe, out var _))
                        {
                            missing.Add(maybe);
                            newChunk += maybe;
                        }
                    }
                }
                part2.Add(CloseScore(missing));
            }
        }

        return isPartTwo ? part2.OrderBy(x => x).ToList()[part2.Count / 2] : part1;
    }

    protected override object Part1() => Run(false);

    protected override object Part2() => Run(true);
}