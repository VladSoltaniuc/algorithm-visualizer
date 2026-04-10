using AlgorithmVisualizer.Api.Models;

namespace AlgorithmVisualizer.Api.Services;

public class TreeService
{
    private class TreeNode
    {
        public int Value;
        public TreeNode? Left,
            Right;

        public TreeNode(int v)
        {
            Value = v;
        }
    }

    private static TreeNode? Insert(TreeNode? node, int val)
    {
        if (node == null)
            return new TreeNode(val);
        if (val < node.Value)
            node.Left = Insert(node.Left, val);
        else
            node.Right = Insert(node.Right, val);
        return node;
    }

    private static TreeNode BuildBST(int[] values)
    {
        if (values.Length == 0)
            throw new ArgumentException("Provide a non-empty array.");
        TreeNode? root = null;
        foreach (var v in values)
            root = Insert(root, v);
        return root!;
    }

    private static int[] ToLevelOrder(TreeNode root)
    {
        var result = new List<int>();
        var queue = new Queue<TreeNode?>();
        queue.Enqueue(root);
        while (queue.Count > 0)
        {
            var n = queue.Dequeue();
            if (n == null)
                continue;
            result.Add(n.Value);
            queue.Enqueue(n.Left);
            queue.Enqueue(n.Right);
        }
        return result.ToArray();
    }

    // BFS level-order preserving null gaps so the frontend can reconstruct shape.
    // Index i has left child at 2i+1 and right child at 2i+2.
    private static int?[] ToLevelOrderWithNulls(TreeNode? root)
    {
        if (root == null)
            return [];
        var result = new List<int?>();
        var queue = new Queue<TreeNode?>();
        queue.Enqueue(root);
        while (queue.Count > 0)
        {
            var n = queue.Dequeue();
            if (n == null)
            {
                result.Add(null);
                continue;
            }
            result.Add(n.Value);
            queue.Enqueue(n.Left);
            queue.Enqueue(n.Right);
        }
        // Trim trailing nulls
        while (result.Count > 0 && result[^1] == null)
            result.RemoveAt(result.Count - 1);
        return [.. result];
    }

    // Time: O(n)
    // Space: O(h) where h = tree height
    public List<AlgorithmStep> Inorder(int[] values)
    {
        var tree = BuildBST(values);
        var steps = new List<AlgorithmStep>();
        var result = new List<int>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = ToLevelOrder(tree),
                Description = "BST built (level-order view)",
            }
        );
        InorderHelper(tree, steps, result, ref step);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = result.ToArray(),
                Description = $"Inorder complete: [{string.Join(", ", result)}]",
                SortedIndices = Enumerable.Range(0, result.Count).ToArray(),
            }
        );
        return steps;
    }

    private static void InorderHelper(
        TreeNode? node,
        List<AlgorithmStep> steps,
        List<int> result,
        ref int step
    )
    {
        if (node == null)
            return;
        InorderHelper(node.Left, steps, result, ref step);
        result.Add(node.Value);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = result.ToArray(),
                Description = $"Visit {node.Value}",
                HighlightIndices = [result.Count - 1],
            }
        );
        InorderHelper(node.Right, steps, result, ref step);
    }

    // 2. Preorder
    // Time: O(n)
    // Space: O(h) where h = tree height
    public List<AlgorithmStep> Preorder(int[] values)
    {
        var tree = BuildBST(values);
        var steps = new List<AlgorithmStep>();
        var result = new List<int>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = ToLevelOrder(tree),
                Description = "BST built (level-order view)",
            }
        );
        PreorderHelper(tree, steps, result, ref step);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = result.ToArray(),
                Description = $"Preorder complete: [{string.Join(", ", result)}]",
                SortedIndices = Enumerable.Range(0, result.Count).ToArray(),
            }
        );
        return steps;
    }

    private static void PreorderHelper(
        TreeNode? node,
        List<AlgorithmStep> steps,
        List<int> result,
        ref int step
    )
    {
        if (node == null)
            return;
        result.Add(node.Value);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = result.ToArray(),
                Description = $"Visit {node.Value}",
                HighlightIndices = [result.Count - 1],
            }
        );
        PreorderHelper(node.Left, steps, result, ref step);
        PreorderHelper(node.Right, steps, result, ref step);
    }

    // 3. Postorder
    // Time: O(n)
    // Space: O(h) where h = tree height
    public List<AlgorithmStep> Postorder(int[] values)
    {
        var tree = BuildBST(values);
        var steps = new List<AlgorithmStep>();
        var result = new List<int>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = ToLevelOrder(tree),
                Description = "BST built (level-order view)",
            }
        );
        PostorderHelper(tree, steps, result, ref step);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = result.ToArray(),
                Description = $"Postorder complete: [{string.Join(", ", result)}]",
                SortedIndices = Enumerable.Range(0, result.Count).ToArray(),
            }
        );
        return steps;
    }

    private static void PostorderHelper(
        TreeNode? node,
        List<AlgorithmStep> steps,
        List<int> result,
        ref int step
    )
    {
        if (node == null)
            return;
        PostorderHelper(node.Left, steps, result, ref step);
        PostorderHelper(node.Right, steps, result, ref step);
        result.Add(node.Value);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = result.ToArray(),
                Description = $"Visit {node.Value}",
                HighlightIndices = [result.Count - 1],
            }
        );
    }

    // 4. Level Order (BFS)
    // Time: O(n)
    // Space: O(n)
    public List<AlgorithmStep> LevelOrder(int[] values)
    {
        var tree = BuildBST(values);
        var steps = new List<AlgorithmStep>();
        var result = new List<int>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = ToLevelOrder(tree),
                Description = "BST built (level-order view)",
            }
        );

        var queue = new Queue<TreeNode>();
        queue.Enqueue(tree);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            result.Add(node.Value);
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = result.ToArray(),
                    Description = $"Visit {node.Value}",
                    HighlightIndices = [result.Count - 1],
                }
            );
            if (node.Left != null)
                queue.Enqueue(node.Left);
            if (node.Right != null)
                queue.Enqueue(node.Right);
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = result.ToArray(),
                Description = $"Level-order complete: [{string.Join(", ", result)}]",
                SortedIndices = Enumerable.Range(0, result.Count).ToArray(),
            }
        );
        return steps;
    }

    // 5. BST Insert / Search
    // Time: O(log n) avg, O(n) worst
    // Space: O(log n) avg, O(n) worst
    public List<AlgorithmStep> BstInsertSearch(int[] values, int target)
    {
        if (values.Length == 0)
            throw new ArgumentException("Provide a non-empty array.");
        var steps = new List<AlgorithmStep>();
        int step = 0;

        // Build the BST silently — only show the search
        TreeNode? root = null;
        foreach (var v in values)
            root = Insert(root, v);

        var lo = ToLevelOrder(root!);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = lo,
                Description = $"BST ready. Searching for {target}.",
            }
        );

        var node = root;
        while (node != null)
        {
            lo = ToLevelOrder(root!);
            int idx = Array.IndexOf(lo, node.Value);
            if (node.Value == target)
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step,
                        Array = lo,
                        Description = $"Found {target}!",
                        SortedIndices = idx >= 0 ? [idx] : [],
                    }
                );
                return steps;
            }
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = lo,
                    Description =
                        target < node.Value
                            ? $"At node {node.Value}: {target} < {node.Value} — go left"
                            : $"At node {node.Value}: {target} > {node.Value} — go right",
                    HighlightIndices = idx >= 0 ? [idx] : [],
                }
            );
            node = target < node.Value ? node.Left : node.Right;
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = ToLevelOrder(root!),
                Description = $"{target} not found in BST.",
            }
        );
        return steps;
    }

    // 6. Tree Height
    // Time: O(n)
    // Space: O(h) where h = tree height
    public List<AlgorithmStep> Height(int[] values)
    {
        var tree = BuildBST(values);
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = ToLevelOrder(tree),
                Description = "Computing tree height",
            }
        );

        int h = HeightHelper(tree, steps, ref step);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = ToLevelOrder(tree),
                Description = $"Tree height = {h}",
                SortedIndices = Enumerable.Range(0, ToLevelOrder(tree).Length).ToArray(),
            }
        );
        return steps;
    }

    private static int HeightHelper(TreeNode? node, List<AlgorithmStep> steps, ref int step)
    {
        if (node == null)
            return -1;
        int l = HeightHelper(node.Left, steps, ref step);
        int r = HeightHelper(node.Right, steps, ref step);
        int h = 1 + Math.Max(l, r);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = [node.Value, h],
                Description = $"Node {node.Value}: height = {h} (left={l + 1}, right={r + 1})",
                HighlightIndices = [0],
            }
        );
        return h;
    }

    // 7. Lowest Common Ancestor
    // Time: O(n)
    // Space: O(h) where h = tree height
    public List<AlgorithmStep> Lca(int[] values, int a, int b)
    {
        var tree = BuildBST(values);
        var steps = new List<AlgorithmStep>();
        int step = 0;
        var lo2 = ToLevelOrder(tree);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = lo2,
                Description = $"Finding LCA of {a} and {b}",
            }
        );

        var node = tree;
        while (node != null)
        {
            lo2 = ToLevelOrder(tree);
            int idx = Array.IndexOf(lo2, node.Value);
            if (a < node.Value && b < node.Value)
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = lo2,
                        Description = $"Both < {node.Value}, go left",
                        HighlightIndices = idx >= 0 ? [idx] : [],
                    }
                );
                node = node.Left;
            }
            else if (a > node.Value && b > node.Value)
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = lo2,
                        Description = $"Both > {node.Value}, go right",
                        HighlightIndices = idx >= 0 ? [idx] : [],
                    }
                );
                node = node.Right;
            }
            else
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step,
                        Array = lo2,
                        Description = $"LCA of {a} and {b} is {node.Value}",
                        SortedIndices = idx >= 0 ? [idx] : [],
                    }
                );
                return steps;
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = ToLevelOrder(tree),
                Description = "LCA not found",
            }
        );
        return steps;
    }

    // 8. Invert Binary Tree
    // Time: O(n)
    // Space: O(h) where h = tree height
    public List<AlgorithmStep> Invert(int[] values)
    {
        var tree = BuildBST(values);
        var steps = new List<AlgorithmStep>();
        int step = 0;

        var initialLo = ToLevelOrder(tree);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = initialLo,
                TreeLevelOrder = ToLevelOrderWithNulls(tree),
                Description =
                    $"Original BST: [{string.Join(", ", initialLo)}]. "
                    + "We will visit every node top-down and swap its left and right children.",
            }
        );

        InvertHelper(tree, tree, steps, ref step);

        var finalLo = ToLevelOrder(tree);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = finalLo,
                TreeLevelOrder = ToLevelOrderWithNulls(tree),
                Description =
                    $"Done. Inverted tree: [{string.Join(", ", finalLo)}]. "
                    + "Every subtree is now a mirror of the original.",
                SortedIndices = Enumerable.Range(0, finalLo.Length).ToArray(),
            }
        );
        return steps;
    }

    private static void InvertHelper(
        TreeNode? node,
        TreeNode root,
        List<AlgorithmStep> steps,
        ref int step
    )
    {
        if (node == null)
            return;

        bool hasLeft = node.Left != null;
        bool hasRight = node.Right != null;

        if (hasLeft || hasRight)
        {
            // --- Before swap: highlight node (red) + both children (green) ---
            var preLo = ToLevelOrder(root);
            int nodeIdx = Array.IndexOf(preLo, node.Value);

            string leftLabel = hasLeft ? node.Left!.Value.ToString() : "∅";
            string rightLabel = hasRight ? node.Right!.Value.ToString() : "∅";

            var childIndices = new List<int>();
            if (hasLeft)
            {
                int li = Array.IndexOf(preLo, node.Left!.Value);
                if (li >= 0)
                    childIndices.Add(li);
            }

            if (hasRight)
            {
                int ri = Array.IndexOf(preLo, node.Right!.Value);
                if (ri >= 0)
                    childIndices.Add(ri);
            }

            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = preLo,
                    TreeLevelOrder = ToLevelOrderWithNulls(root),
                    Description =
                        $"Node {node.Value}: left child = {leftLabel}, right child = {rightLabel}. Swapping them.",
                    HighlightIndices = nodeIdx >= 0 ? [nodeIdx] : [],
                    SortedIndices = childIndices.ToArray(),
                }
            );
        }

        // Do the swap
        (node.Left, node.Right) = (node.Right, node.Left);

        if (hasLeft || hasRight)
        {
            // --- After swap: node is done (green), new children highlighted (red) ---
            var postLo = ToLevelOrder(root);
            int nodeIdx = Array.IndexOf(postLo, node.Value);

            string newLeftLabel = node.Left != null ? node.Left.Value.ToString() : "∅";
            string newRightLabel = node.Right != null ? node.Right.Value.ToString() : "∅";

            var newChildIndices = new List<int>();
            if (node.Left != null)
            {
                int li = Array.IndexOf(postLo, node.Left.Value);
                if (li >= 0)
                    newChildIndices.Add(li);
            }

            if (node.Right != null)
            {
                int ri = Array.IndexOf(postLo, node.Right.Value);
                if (ri >= 0)
                    newChildIndices.Add(ri);
            }

            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = postLo,
                    TreeLevelOrder = ToLevelOrderWithNulls(root),
                    Description =
                        $"Node {node.Value}: swap done — left is now {newLeftLabel}, right is now {newRightLabel}.",
                    HighlightIndices = newChildIndices.ToArray(),
                    SortedIndices = nodeIdx >= 0 ? [nodeIdx] : [],
                }
            );
        }

        InvertHelper(node.Left, root, steps, ref step);
        InvertHelper(node.Right, root, steps, ref step);
    }

    // 9. Validate BST
    // Time: O(n)
    // Space: O(h) where h = tree height
    public List<AlgorithmStep> ValidateBst(int[] values)
    {
        var tree = BuildBST(values);
        var steps = new List<AlgorithmStep>();
        var inorder = new List<int>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = ToLevelOrder(tree),
                Description = "Validating BST via inorder traversal",
            }
        );

        bool valid = ValidateBstHelper(tree, steps, inorder, ref step, int.MinValue, int.MaxValue);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = inorder.ToArray(),
                Description = valid ? "Valid BST ✓" : "Not a valid BST ✗",
                SortedIndices = valid ? Enumerable.Range(0, inorder.Count).ToArray() : [],
            }
        );
        return steps;
    }

    private static bool ValidateBstHelper(
        TreeNode? node,
        List<AlgorithmStep> steps,
        List<int> inorder,
        ref int step,
        int min,
        int max
    )
    {
        if (node == null)
            return true;
        if (node.Value <= min || node.Value >= max)
        {
            inorder.Add(node.Value);
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = inorder.ToArray(),
                    Description = $"Node {node.Value} violates BST property",
                    HighlightIndices = [inorder.Count - 1],
                }
            );
            return false;
        }
        if (!ValidateBstHelper(node.Left, steps, inorder, ref step, min, node.Value))
            return false;
        inorder.Add(node.Value);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = inorder.ToArray(),
                Description = $"Node {node.Value} in range ({min}, {max})",
                HighlightIndices = [inorder.Count - 1],
            }
        );
        return ValidateBstHelper(node.Right, steps, inorder, ref step, node.Value, max);
    }

    // 10. Diameter
    // Time: O(n)
    // Space: O(h) where h = tree height
    public List<AlgorithmStep> Diameter(int[] values)
    {
        var tree = BuildBST(values);
        var steps = new List<AlgorithmStep>();
        int step = 0;
        int diameter = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = ToLevelOrder(tree),
                Description = "Computing tree diameter",
            }
        );

        DiameterHelper(tree, steps, ref step, ref diameter);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = ToLevelOrder(tree),
                Description = $"Diameter = {diameter}",
                SortedIndices = Enumerable.Range(0, ToLevelOrder(tree).Length).ToArray(),
            }
        );
        return steps;
    }

    private static int DiameterHelper(
        TreeNode? node,
        List<AlgorithmStep> steps,
        ref int step,
        ref int diameter
    )
    {
        if (node == null)
            return 0;
        int l = DiameterHelper(node.Left, steps, ref step, ref diameter);
        int r = DiameterHelper(node.Right, steps, ref step, ref diameter);
        diameter = Math.Max(diameter, l + r);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = [node.Value, l, r, l + r],
                Description = $"Node {node.Value}: left={l}, right={r}, path={l + r}",
                HighlightIndices = [0],
            }
        );
        return 1 + Math.Max(l, r);
    }

    // 11. Huffman Coding
    // Time: O(n log n) where n = number of unique characters
    // Space: O(n)
    public List<AlgorithmStep> HuffmanCoding(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Provide non-empty text.");

        var steps = new List<AlgorithmStep>();
        int step = 0;

        // Count frequencies
        var freq = new Dictionary<char, int>();
        foreach (var ch in text)
            freq[ch] = freq.TryGetValue(ch, out int v) ? v + 1 : 1;

        // Show frequencies sorted descending
        var sortedChars = freq.OrderByDescending(kv => kv.Value).ThenBy(kv => kv.Key).ToList();
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = sortedChars.Select(kv => kv.Value).ToArray(),
                Labels = sortedChars
                    .Select(kv => kv.Key == ' ' ? "·" : kv.Key.ToString())
                    .ToArray(),
                Description =
                    $"Count how often each character appears. "
                    + $"Most frequent: '{sortedChars[0].Key}' ({sortedChars[0].Value}×). "
                    + $"{freq.Count} unique character(s) in \"{text}\".",
                HighlightIndices = Enumerable.Range(0, sortedChars.Count).ToArray(),
            }
        );

        // Build priority queue (min-heap via sorted list)
        var nodes = new List<(int weight, string label, HuffNode node)>();
        foreach (var kv in freq)
            nodes.Add(
                (kv.Value, kv.Key == ' ' ? "·" : kv.Key.ToString(), new HuffNode(kv.Key, kv.Value))
            );

        nodes.Sort(
            (a, b) =>
                a.weight != b.weight
                    ? a.weight.CompareTo(b.weight)
                    : string.Compare(a.label, b.label, StringComparison.Ordinal)
        );

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = nodes.Select(n => n.weight).ToArray(),
                Labels = nodes.Select(n => n.label).ToArray(),
                Description =
                    "Place every character in a priority queue sorted by frequency (lowest first). "
                    + "The two smallest nodes will always be merged next.",
            }
        );

        // Merge until one node remains
        while (nodes.Count > 1)
        {
            var left = nodes[0];
            var right = nodes[1];
            nodes.RemoveAt(0);
            nodes.RemoveAt(0);

            int merged = left.weight + right.weight;
            string mergedLabel = $"{left.label}+{right.label}";
            var parent = new HuffNode(null, merged) { Left = left.node, Right = right.node };

            int pos = nodes.FindIndex(n => n.weight >= merged);
            if (pos < 0)
                pos = nodes.Count;
            nodes.Insert(pos, (merged, mergedLabel, parent));

            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = nodes.Select(n => n.weight).ToArray(),
                    Labels = nodes.Select(n => n.label).ToArray(),
                    Description =
                        $"Take the two lightest nodes '{left.label}' ({left.weight}) and '{right.label}' ({right.weight}). "
                        + $"Combine them into a new internal node with weight {merged}. "
                        + $"Re-insert it at position {pos + 1} in the queue.",
                    HighlightIndices = [pos],
                }
            );
        }

        // Generate codes via DFS
        var codes = new Dictionary<char, string>();
        GenerateCodes(nodes[0].node, "", codes);

        var codeList = codes.OrderBy(kv => kv.Value.Length).ThenBy(kv => kv.Key).ToList();
        string encoded = string.Concat(text.Select(ch => codes[ch]));
        int originalBits = text.Length * 8;
        int huffBits = encoded.Length;

        string codesDesc = string.Join(
            ", ",
            codeList.Select(kv => $"'{(kv.Key == ' ' ? "·" : kv.Key.ToString())}'={kv.Value}")
        );
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = codeList.Select(kv => kv.Value.Length).ToArray(),
                Labels = codeList.Select(kv => kv.Key == ' ' ? "·" : kv.Key.ToString()).ToArray(),
                Notes = codeList.Select(kv => kv.Value).ToArray(),
                Description =
                    $"Assign codes by following the tree: left branch = 0, right branch = 1. "
                    + $"Frequent characters get shorter codes. {codesDesc}.",
                SortedIndices = Enumerable.Range(0, codeList.Count).ToArray(),
            }
        );

        int savedBits = originalBits - huffBits;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = [originalBits, huffBits],
                Labels = ["Original (ASCII)", "Huffman"],
                Notes =
                [
                    $"{text.Length} chars × 8 bits",
                    $"saved {savedBits} bits ({(double)savedBits / originalBits:P0})",
                ],
                Description =
                    $"Original size: {originalBits} bits ({text.Length} chars × 8 bits). "
                    + $"Huffman size: {huffBits} bits. "
                    + $"Saved {savedBits} bits — {(double)savedBits / originalBits:P1} smaller.",
                SortedIndices = [0, 1],
            }
        );

        return steps;
    }

    private class HuffNode
    {
        public char? Char;
        public int Weight;
        public HuffNode? Left,
            Right;

        public HuffNode(char? ch, int w)
        {
            Char = ch;
            Weight = w;
        }
    }

    private static void GenerateCodes(HuffNode node, string prefix, Dictionary<char, string> codes)
    {
        if (node.Left == null && node.Right == null && node.Char.HasValue)
        {
            codes[node.Char.Value] = prefix.Length > 0 ? prefix : "0";
            return;
        }
        if (node.Left != null)
            GenerateCodes(node.Left, prefix + "0", codes);
        if (node.Right != null)
            GenerateCodes(node.Right, prefix + "1", codes);
    }
}
