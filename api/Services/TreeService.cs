using api.Helpers;
using api.Models;

namespace api.Services;

public class TreeService
{
    public List<AlgorithmStep> Inorder(int[] values)
    {
        var tree = TreeHelpers.BuildBST(values);
        var steps = new List<AlgorithmStep>();
        var result = new List<int>();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = TreeHelpers.ToLevelOrder(tree), Description = "BST built (level-order view)" });
        TreeHelpers.InorderHelper(tree, steps, result, ref step);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step,
            Array = result.ToArray(),
            Description = $"Inorder complete: [{string.Join(", ", result)}]",
            SortedIndices = Enumerable.Range(0, result.Count).ToArray(),
        });
        return steps;
    }

    public List<AlgorithmStep> Preorder(int[] values)
    {
        var tree = TreeHelpers.BuildBST(values);
        var steps = new List<AlgorithmStep>();
        var result = new List<int>();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = TreeHelpers.ToLevelOrder(tree), Description = "BST built (level-order view)" });
        TreeHelpers.PreorderHelper(tree, steps, result, ref step);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step,
            Array = result.ToArray(),
            Description = $"Preorder complete: [{string.Join(", ", result)}]",
            SortedIndices = Enumerable.Range(0, result.Count).ToArray(),
        });
        return steps;
    }

    public List<AlgorithmStep> Postorder(int[] values)
    {
        var tree = TreeHelpers.BuildBST(values);
        var steps = new List<AlgorithmStep>();
        var result = new List<int>();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = TreeHelpers.ToLevelOrder(tree), Description = "BST built (level-order view)" });
        TreeHelpers.PostorderHelper(tree, steps, result, ref step);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step,
            Array = result.ToArray(),
            Description = $"Postorder complete: [{string.Join(", ", result)}]",
            SortedIndices = Enumerable.Range(0, result.Count).ToArray(),
        });
        return steps;
    }

    public List<AlgorithmStep> LevelOrder(int[] values)
    {
        var tree = TreeHelpers.BuildBST(values);
        var steps = new List<AlgorithmStep>();
        var result = new List<int>();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = TreeHelpers.ToLevelOrder(tree), Description = "BST built (level-order view)" });

        var queue = new Queue<TreeNode>();
        queue.Enqueue(tree);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            result.Add(node.Value);
            steps.Add(new AlgorithmStep
            {
                StepNumber = step++,
                Array = result.ToArray(),
                Description = $"Visit {node.Value}",
                HighlightIndices = [result.Count - 1],
            });
            if (node.Left != null) queue.Enqueue(node.Left);
            if (node.Right != null) queue.Enqueue(node.Right);
        }

        steps.Add(new AlgorithmStep
        {
            StepNumber = step,
            Array = result.ToArray(),
            Description = $"Level-order complete: [{string.Join(", ", result)}]",
            SortedIndices = Enumerable.Range(0, result.Count).ToArray(),
        });
        return steps;
    }

    public List<AlgorithmStep> BstInsertSearch(int[] values, int target)
    {
        if (values.Length == 0)
            throw new ArgumentException("Provide a non-empty array.");
        var steps = new List<AlgorithmStep>();
        int step = 0;

        TreeNode? root = null;
        foreach (var v in values)
            root = TreeHelpers.Insert(root, v);

        var lo = TreeHelpers.ToLevelOrder(root!);
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = lo, Description = $"BST ready. Searching for {target}." });

        var node = root;
        while (node != null)
        {
            lo = TreeHelpers.ToLevelOrder(root!);
            int idx = Array.IndexOf(lo, node.Value);
            if (node.Value == target)
            {
                steps.Add(new AlgorithmStep
                {
                    StepNumber = step,
                    Array = lo,
                    Description = $"Found {target}!",
                    SortedIndices = idx >= 0 ? [idx] : [],
                });
                return steps;
            }
            steps.Add(new AlgorithmStep
            {
                StepNumber = step++,
                Array = lo,
                Description = target < node.Value
                    ? $"At node {node.Value}: {target} < {node.Value} - go left"
                    : $"At node {node.Value}: {target} > {node.Value} - go right",
                HighlightIndices = idx >= 0 ? [idx] : [],
            });
            node = target < node.Value ? node.Left : node.Right;
        }

        steps.Add(new AlgorithmStep { StepNumber = step, Array = TreeHelpers.ToLevelOrder(root!), Description = $"{target} not found in BST." });
        return steps;
    }

    public List<AlgorithmStep> Height(int[] values)
    {
        var tree = TreeHelpers.BuildBST(values);
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = TreeHelpers.ToLevelOrder(tree), Description = "Computing tree height" });

        int h = TreeHelpers.HeightHelper(tree, steps, ref step);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step,
            Array = TreeHelpers.ToLevelOrder(tree),
            Description = $"Tree height = {h}",
            SortedIndices = Enumerable.Range(0, TreeHelpers.ToLevelOrder(tree).Length).ToArray(),
        });
        return steps;
    }

    public List<AlgorithmStep> Lca(int[] values, int a, int b)
    {
        var tree = TreeHelpers.BuildBST(values);
        var steps = new List<AlgorithmStep>();
        int step = 0;
        var lo2 = TreeHelpers.ToLevelOrder(tree);
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = lo2, Description = $"Finding LCA of {a} and {b}" });

        var node = tree;
        while (node != null)
        {
            lo2 = TreeHelpers.ToLevelOrder(tree);
            int idx = Array.IndexOf(lo2, node.Value);
            if (a < node.Value && b < node.Value)
            {
                steps.Add(new AlgorithmStep { StepNumber = step++, Array = lo2, Description = $"Both < {node.Value}, go left", HighlightIndices = idx >= 0 ? [idx] : [] });
                node = node.Left;
            }
            else if (a > node.Value && b > node.Value)
            {
                steps.Add(new AlgorithmStep { StepNumber = step++, Array = lo2, Description = $"Both > {node.Value}, go right", HighlightIndices = idx >= 0 ? [idx] : [] });
                node = node.Right;
            }
            else
            {
                steps.Add(new AlgorithmStep { StepNumber = step, Array = lo2, Description = $"LCA of {a} and {b} is {node.Value}", SortedIndices = idx >= 0 ? [idx] : [] });
                return steps;
            }
        }

        steps.Add(new AlgorithmStep { StepNumber = step, Array = TreeHelpers.ToLevelOrder(tree), Description = "LCA not found" });
        return steps;
    }

    public List<AlgorithmStep> Invert(int[] values)
    {
        var tree = TreeHelpers.BuildBST(values);
        var steps = new List<AlgorithmStep>();
        int step = 0;

        var initialLo = TreeHelpers.ToLevelOrder(tree);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step++,
            Array = initialLo,
            TreeLevelOrder = TreeHelpers.ToLevelOrderWithNulls(tree),
            Description = $"Original BST: [{string.Join(", ", initialLo)}]. We will visit every node top-down and swap its left and right children.",
        });

        TreeHelpers.InvertHelper(tree, tree, steps, ref step);

        var finalLo = TreeHelpers.ToLevelOrder(tree);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step,
            Array = finalLo,
            TreeLevelOrder = TreeHelpers.ToLevelOrderWithNulls(tree),
            Description = $"Done. Inverted tree: [{string.Join(", ", finalLo)}]. Every subtree is now a mirror of the original.",
            SortedIndices = Enumerable.Range(0, finalLo.Length).ToArray(),
        });
        return steps;
    }

    public List<AlgorithmStep> ValidateBst(int[] values)
    {
        var tree = TreeHelpers.BuildBST(values);
        var steps = new List<AlgorithmStep>();
        var inorder = new List<int>();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = TreeHelpers.ToLevelOrder(tree), Description = "Validating BST via inorder traversal" });

        bool valid = TreeHelpers.ValidateBstHelper(tree, steps, inorder, ref step, int.MinValue, int.MaxValue);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step,
            Array = inorder.ToArray(),
            Description = valid ? "Valid BST ✓" : "Not a valid BST ✗",
            SortedIndices = valid ? Enumerable.Range(0, inorder.Count).ToArray() : [],
        });
        return steps;
    }

    public List<AlgorithmStep> Diameter(int[] values)
    {
        var tree = TreeHelpers.BuildBST(values);
        var steps = new List<AlgorithmStep>();
        int step = 0, diameter = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = TreeHelpers.ToLevelOrder(tree), Description = "Computing tree diameter" });

        TreeHelpers.DiameterHelper(tree, steps, ref step, ref diameter);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step,
            Array = TreeHelpers.ToLevelOrder(tree),
            Description = $"Diameter = {diameter}",
            SortedIndices = Enumerable.Range(0, TreeHelpers.ToLevelOrder(tree).Length).ToArray(),
        });
        return steps;
    }

}
