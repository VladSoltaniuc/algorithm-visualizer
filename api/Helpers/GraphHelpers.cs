using api.Models;

namespace api.Helpers;

internal static class GraphHelpers
{
    internal static List<List<int>> BuildAdj(int nodeCount, int[][] edges, bool directed = false)
    {
        var adj = Enumerable.Range(0, nodeCount).Select(_ => new List<int>()).ToList();
        foreach (var e in edges)
        {
            adj[e[0]].Add(e[1]);
            if (!directed) adj[e[1]].Add(e[0]);
        }
        return adj;
    }

    internal static List<List<int[]>> BuildWeightedAdj(int nodeCount, int[][] edges, bool directed = false)
    {
        var adj = Enumerable.Range(0, nodeCount).Select(_ => new List<int[]>()).ToList();
        foreach (var e in edges)
        {
            int u = e[0], v = e[1], w = e.Length > 2 ? e[2] : 1;
            adj[u].Add([v, w]);
            if (!directed) adj[v].Add([u, w]);
        }
        return adj;
    }

    internal static void DfsHelper(int node, List<List<int>> adj, int[] visited, List<AlgorithmStep> steps, ref int step)
    {
        visited[node] = 1;
        steps.Add(new AlgorithmStep
        {
            StepNumber = step++,
            Array = (int[])visited.Clone(),
            Description = $"Visit node {node}",
            HighlightIndices = [node],
            SortedIndices = Enumerable.Range(0, visited.Length).Where(i => visited[i] == 1).ToArray(),
        });
        foreach (int nb in adj[node])
            if (visited[nb] == 0)
                DfsHelper(nb, adj, visited, steps, ref step);
    }

    internal static int Find(int[] parent, int x)
    {
        while (parent[x] != x) { parent[x] = parent[parent[x]]; x = parent[x]; }
        return x;
    }

    internal static string[] CycleLabels(bool[] visited, bool[] pathVisited, int n)
    {
        var labels = new string[n];
        for (int i = 0; i < n; i++)
            labels[i] = pathVisited[i] ? "stack" : visited[i] ? "done" : "unvisited";
        return labels;
    }

    internal static int[][] CycleMatrix(bool[] visited, bool[] pathVisited) =>
    [
        visited.Select(v => v ? 1 : 0).ToArray(),
        pathVisited.Select(v => v ? 1 : 0).ToArray(),
    ];
}
