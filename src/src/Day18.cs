namespace aoc2021.src;

internal class Day18 : BaseDay
{
    private bool _wasChangeDone = false;

    class Node
    {
        public int? Number;

        public Node? Left = null;
        public Node? Right = null;
        public Node? Parent = null;

        public Node()
        {

        }

        public Node(int v)
        {
            Number = v;
        }

        public bool IsRegularPair => Left != null && Left.Number.HasValue && Right != null && Right.Number.HasValue;

        public int Magnitude
        {
            get
            {
                if (Number.HasValue)
                {
                    return Number.Value;
                }
                if (Left != null && Right != null)
                {
                    return 3 * Left.Magnitude + 2 * Right.Magnitude;
                }

                throw new NotImplementedException();
            }
        }
    }

    Node ParseNode(string input)
    {
        var node = new Node();

        // remove [ and ] from start/end
        if (input.StartsWith("["))
        {
            input = input.Substring(1);
        }
        if (input.EndsWith("]"))
        {
            input = input.Substring(0, input.Length - 1);
        }
        var commas = input.CountChar(',');

        // logic
        if (commas == 0)
        {
            node.Number = int.Parse(input);
        }
        else if (commas == 1)
        {
            var d = input.Split(',').Select(int.Parse).ToArray();

            node.Left = new Node(d[0]);
            node.Right = new Node(d[1]);
        }
        else
        {
            if (input[0] == '[')
            {
                var matching = FindClosing(0, input);

                var left = input[0..(1 + matching)];
                var right = input[(matching + 2)..];

                node.Left = ParseNode(left);
                node.Right = ParseNode(right);
            }
            else
            {
                var split = input.Split(',', 2);

                node.Left = ParseNode(split[0]);
                node.Right = ParseNode(split[1]);
            }
        }

        return node;
    }

    static int FindClosing(int startIndex, string input)
    {
        int count = 0;

        for (int i = startIndex; i < input.Length; i++)
        {
            if (input[i] == '[')
            {
                count++;
            }
            else if (input[i] == ']')
            {
                count--;
            }

            if (count == 0)
            {
                return i;
            }
        }
        return -1;
    }

    void ExplodeTree(Node node, int level)
    {
        if (node.Left != null)
        {
            ExplodeTree(node.Left, level + 1);
        }

        if (node.Right != null)
        {
            ExplodeTree(node.Right, level + 1);
        }

        if (level > 4 && node.IsRegularPair)
        {
            if (node.Left == null || node.Right == null || node.Left.Number == null || node.Right.Number == null)
            {
                throw new InvalidDataException();
            }

            var left = node.Left.Number.Value;
            var right = node.Right.Number.Value;

            var leftNode = FindLeft(node);
            if (leftNode != null)
            {
                leftNode.Number += left;
            }

            var rightNode = FindRight(node);
            if (rightNode != null)
            {
                rightNode.Number += right;
            }

            node.Left = null;
            node.Right = null;
            node.Number = 0;

            _wasChangeDone = true;
        }
    }

    private Node? FindLeft(Node node)
    {
        if (node.Parent == null)
        {
            return null;
        }

        if (node == node.Parent.Left)
        {
            return FindLeft(node.Parent);
        }

        var tmp = node.Parent.Left;
        while (tmp.Right != null)
        {
            tmp = tmp.Right;
        }

        return tmp;
    }

    private Node? FindRight(Node node)
    {
        if (node.Parent == null)
        {
            return null;
        }

        if (node == node.Parent.Right)
        {
            return FindRight(node.Parent);
        }

        var tmp = node.Parent.Right;
        while (tmp.Left != null)
        {
            tmp = tmp.Left;
        }

        return tmp;
    }

    void SplitTree(Node node, Node parent)
    {
        if (_wasChangeDone)
        {
            return;
        }

        if (node.Left != null)
        {
            SplitTree(node.Left, node);
        }

        if (node.Right != null)
        {
            SplitTree(node.Right, node);
        }

        if (node.Number.HasValue && node.Number.Value >= 10)
        {
            _wasChangeDone = true;

            var left = (int) Math.Floor(node.Number.Value / 2.0);
            var right = (int) Math.Ceiling(node.Number.Value / 2.0);

            var nnode = new Node()
            {
                Left = new Node(left),
                Right = new Node(right)
            };
            nnode.Parent = parent;

            if (parent.Left == node)
            {
                parent.Left = nnode;
            }
            else if (parent.Right == node)
            {
                parent.Right = nnode;
            }
            else
            {
                throw new InvalidDataException();
            }
        }
    }

    void FixParent(Node node)
    {
        if (node.Left != null)
        {
            node.Left.Parent = node;
            FixParent(node.Left);
        }
        if (node.Right != null)
        {
            node.Right.Parent = node;
            FixParent(node.Right);
        }
    }

    void ReduceTree(Node tree)
    {
        FixParent(tree);

        while (true)
        {
            _wasChangeDone = false;

            ExplodeTree(tree, 1);
            if (_wasChangeDone)
            {
                continue;
            }

            SplitTree(tree, tree);
            if (!_wasChangeDone)
            {
                break;
            }
        }
    }

    protected override object Part1()
    {
        var data = new Queue<Node>(ReadAllLines(true).Select(ParseNode));

        var one = data.Dequeue();

        while (data.Count > 0)
        {
            var two = data.Dequeue();

            var tree = new Node()
            {
                Left = one,
                Right = two
            };

            ReduceTree(tree);

            one = tree;
        }

        return one.Magnitude;
    }

    protected override object Part2()
    {
        return ReadAllLines(true).GetPermutations(2).Select(pair =>
        {
            var tree = new Node()
            {
                Left = ParseNode(pair.First()),
                Right = ParseNode(pair.Skip(1).First())
            };
            ReduceTree(tree);

            return tree.Magnitude;
        }).Max();
    }
}