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

    // 1. Inorder
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
        TreeNode? root = null;

        foreach (var v in values)
        {
            root = Insert(root, v);
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = ToLevelOrder(root),
                    Description = $"Inserted {v}",
                    HighlightIndices = [ToLevelOrder(root).ToList().IndexOf(v)],
                }
            );
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = ToLevelOrder(root!),
                Description = $"BST complete. Searching for {target}",
            }
        );

        var node = root;
        while (node != null)
        {
            var lo = ToLevelOrder(root!);
            int idx = Array.IndexOf(lo, node.Value);
            if (node.Value == target)
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step,
                        Array = lo,
                        Description = $"Found {target}",
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
                        $"At node {node.Value}, go {(target < node.Value ? "left" : "right")}",
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
                Description = $"{target} not found in BST",
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
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = ToLevelOrder(tree),
                Description = "Original BST",
            }
        );

        InvertHelper(tree, steps, ref step);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = ToLevelOrder(tree),
                Description = "Tree inverted",
                SortedIndices = Enumerable.Range(0, ToLevelOrder(tree).Length).ToArray(),
            }
        );
        return steps;
    }

    private static void InvertHelper(TreeNode? node, List<AlgorithmStep> steps, ref int step)
    {
        if (node == null)
            return;
        (node.Left, node.Right) = (node.Right, node.Left);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = [node.Value],
                Description = $"Swapped children of {node.Value}",
                HighlightIndices = [0],
            }
        );
        InvertHelper(node.Left, steps, ref step);
        InvertHelper(node.Right, steps, ref step);
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
}
