namespace aoc2021;

internal static class Ext
{
    internal static IEnumerable<string> Split(this string src, char c) => src.Split(new[] { c }, StringSplitOptions.RemoveEmptyEntries);

    internal static IEnumerable<string> Permutate(this string source)
    {
        if (source.Length == 1)
        {
            return new List<string> { source };
        }

        var permutations = from c in source
                           from p in Permutate(new string(source.Where(x => x != c).ToArray()))
                           select c + p;

        return permutations;
    }

    internal static IEnumerable<IEnumerable<T>> GetCombinations<T>(this IEnumerable<T> items, int count)
    {
        int i = 0;
        foreach (var item in items)
        {
            if (count == 1)
            {
                yield return new T[] { item };
            }
            else
            {
                foreach (var result in GetCombinations(items.Skip(i + 1), count - 1))
                {
                    yield return new T[] { item }.Concat(result);
                }
            }
            ++i;
        }
    }

    internal static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int length)
    {
        if (length == 1)
        {
            return list.Select(t => new T[] { t });
        }

        return GetPermutations(list, length - 1).SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat(new T[] { t2 }));
    }

    internal static int ManhattanDistance(int x1, int y1, int x2, int y2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);

    internal static int ManhattanDistance(int x1, int y1, int z1, int x2, int y2, int z2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2) + Math.Abs(z1 - z2);

    internal static IEnumerable<IEnumerable<T>> GetConsecutive<T>(this IEnumerable<T> src, int length)
    {
        for (int i = 0; i < src.Count() - length + 1; i++)
        {
            yield return src.Skip(i).Take(length);
        }
    }

    internal static IEnumerable<T> GetColumn<T>(this T[][] src, int columnIndex)
    {
        for (var i = 0; i < src.GetLength(0); i++)
        {
            yield return src[i][columnIndex];
        }
    }

    internal static void Iterate<T>(this T[][] src, Action<int, int, T> func)
    {
        for (var i = 0; i < src.GetLength(0); i++)
        {
            for (var j = 0; j < src[i].GetLength(0); j++)
            {
                func(i, j, src[i][j]);
            }
        }
    }

    internal static IEnumerable<(int row, int column, T value)> Flat<T>(this T[][] src)
    {
        for (var i = 0; i < src.GetLength(0); i++)
        {
            for (var j = 0; j < src[i].GetLength(0); j++)
            {
                yield return (i, j, src[i][j]);
            }
        }
    }
}