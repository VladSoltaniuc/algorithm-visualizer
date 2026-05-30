using api.Models;

namespace api.Helpers;

internal class TreeNode
{
    public int Value;
    public TreeNode? Left, Right;
    public TreeNode(int v) { Value = v; }
}

internal static class TreeHelpers
{
    internal static TreeNode? Insert(TreeNode? node, int val)
    {
        if (node == null) return new TreeNode(val);
        if (val < node.Value) node.Left = Insert(node.Left, val);
        else node.Right = Insert(node.Right, val);
        return node;
    }

    internal static TreeNode BuildBST(int[] values)
    {
        if (values.Length == 0)
            throw new ArgumentException("Provide a non-empty array.");
        TreeNode? root = null;
        foreach (var v in values)
            root = Insert(root, v);
        return root!;
    }

    internal static int[] ToLevelOrder(TreeNode root)
    {
        var result = new List<int>();
        var queue = new Queue<TreeNode?>();
        queue.Enqueue(root);
        while (queue.Count > 0)
        {
            var n = queue.Dequeue();
            if (n == null) continue;
            result.Add(n.Value);
            queue.Enqueue(n.Left);
            queue.Enqueue(n.Right);
        }
        return result.ToArray();
    }

    internal static int?[] ToLevelOrderWithNulls(TreeNode? root)
    {
        if (root == null) return [];
        var result = new List<int?>();
        var queue = new Queue<TreeNode?>();
        queue.Enqueue(root);
        while (queue.Count > 0)
        {
            var n = queue.Dequeue();
            if (n == null) { result.Add(null); continue; }
            result.Add(n.Value);
            queue.Enqueue(n.Left);
            queue.Enqueue(n.Right);
        }
        while (result.Count > 0 && result[^1] == null)
            result.RemoveAt(result.Count - 1);
        return [.. result];
    }

    internal static void InorderHelper(TreeNode? node, List<AlgorithmStep> steps, List<int> result, ref int step)
    {
        if (node == null) return;
        InorderHelper(node.Left, steps, result, ref step);
        result.Add(node.Value);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step++,
            Array = result.ToArray(),
            Description = $"Visit {node.Value}",
            HighlightIndices = [result.Count - 1],
        });
        InorderHelper(node.Right, steps, result, ref step);
    }

    internal static void PreorderHelper(TreeNode? node, List<AlgorithmStep> steps, List<int> result, ref int step)
    {
        if (node == null) return;
        result.Add(node.Value);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step++,
            Array = result.ToArray(),
            Description = $"Visit {node.Value}",
            HighlightIndices = [result.Count - 1],
        });
        PreorderHelper(node.Left, steps, result, ref step);
        PreorderHelper(node.Right, steps, result, ref step);
    }

    internal static void PostorderHelper(TreeNode? node, List<AlgorithmStep> steps, List<int> result, ref int step)
    {
        if (node == null) return;
        PostorderHelper(node.Left, steps, result, ref step);
        PostorderHelper(node.Right, steps, result, ref step);
        result.Add(node.Value);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step++,
            Array = result.ToArray(),
            Description = $"Visit {node.Value}",
            HighlightIndices = [result.Count - 1],
        });
    }

    internal static int HeightHelper(TreeNode? node, List<AlgorithmStep> steps, ref int step)
    {
        if (node == null) return -1;
        int l = HeightHelper(node.Left, steps, ref step);
        int r = HeightHelper(node.Right, steps, ref step);
        int h = 1 + Math.Max(l, r);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step++,
            Array = [node.Value, h],
            Description = $"Node {node.Value}: height = {h} (left={l + 1}, right={r + 1})",
            HighlightIndices = [0],
        });
        return h;
    }

    internal static void InvertHelper(TreeNode? node, TreeNode root, List<AlgorithmStep> steps, ref int step)
    {
        if (node == null) return;

        bool hasLeft = node.Left != null;
        bool hasRight = node.Right != null;

        if (hasLeft || hasRight)
        {
            var preLo = ToLevelOrder(root);
            int nodeIdx = Array.IndexOf(preLo, node.Value);
            string leftLabel = hasLeft ? node.Left!.Value.ToString() : "∅";
            string rightLabel = hasRight ? node.Right!.Value.ToString() : "∅";

            var childIndices = new List<int>();
            if (hasLeft) { int li = Array.IndexOf(preLo, node.Left!.Value); if (li >= 0) childIndices.Add(li); }
            if (hasRight) { int ri = Array.IndexOf(preLo, node.Right!.Value); if (ri >= 0) childIndices.Add(ri); }

            steps.Add(new AlgorithmStep
            {
                StepNumber = step++,
                Array = preLo,
                TreeLevelOrder = ToLevelOrderWithNulls(root),
                Description = $"Node {node.Value}: left child = {leftLabel}, right child = {rightLabel}. Swapping them.",
                HighlightIndices = nodeIdx >= 0 ? [nodeIdx] : [],
                SortedIndices = childIndices.ToArray(),
            });
        }

        (node.Left, node.Right) = (node.Right, node.Left);

        if (hasLeft || hasRight)
        {
            var postLo = ToLevelOrder(root);
            int nodeIdx = Array.IndexOf(postLo, node.Value);
            string newLeftLabel = node.Left != null ? node.Left.Value.ToString() : "∅";
            string newRightLabel = node.Right != null ? node.Right.Value.ToString() : "∅";

            var newChildIndices = new List<int>();
            if (node.Left != null) { int li = Array.IndexOf(postLo, node.Left.Value); if (li >= 0) newChildIndices.Add(li); }
            if (node.Right != null) { int ri = Array.IndexOf(postLo, node.Right.Value); if (ri >= 0) newChildIndices.Add(ri); }

            steps.Add(new AlgorithmStep
            {
                StepNumber = step++,
                Array = postLo,
                TreeLevelOrder = ToLevelOrderWithNulls(root),
                Description = $"Node {node.Value}: swap done - left is now {newLeftLabel}, right is now {newRightLabel}.",
                HighlightIndices = newChildIndices.ToArray(),
                SortedIndices = nodeIdx >= 0 ? [nodeIdx] : [],
            });
        }

        InvertHelper(node.Left, root, steps, ref step);
        InvertHelper(node.Right, root, steps, ref step);
    }

    internal static bool ValidateBstHelper(TreeNode? node, List<AlgorithmStep> steps, List<int> inorder, ref int step, int min, int max)
    {
        if (node == null) return true;
        if (node.Value <= min || node.Value >= max)
        {
            inorder.Add(node.Value);
            steps.Add(new AlgorithmStep
            {
                StepNumber = step++,
                Array = inorder.ToArray(),
                Description = $"Node {node.Value} violates BST property",
                HighlightIndices = [inorder.Count - 1],
            });
            return false;
        }
        if (!ValidateBstHelper(node.Left, steps, inorder, ref step, min, node.Value))
            return false;
        inorder.Add(node.Value);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step++,
            Array = inorder.ToArray(),
            Description = $"Node {node.Value} in range ({min}, {max})",
            HighlightIndices = [inorder.Count - 1],
        });
        return ValidateBstHelper(node.Right, steps, inorder, ref step, node.Value, max);
    }

    internal static int DiameterHelper(TreeNode? node, List<AlgorithmStep> steps, ref int step, ref int diameter)
    {
        if (node == null) return 0;
        int l = DiameterHelper(node.Left, steps, ref step, ref diameter);
        int r = DiameterHelper(node.Right, steps, ref step, ref diameter);
        diameter = Math.Max(diameter, l + r);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step++,
            Array = [node.Value, l, r, l + r],
            Description = $"Node {node.Value}: left={l}, right={r}, path={l + r}",
            HighlightIndices = [0],
        });
        return 1 + Math.Max(l, r);
    }

}
