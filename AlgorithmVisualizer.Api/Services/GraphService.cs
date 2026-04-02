using AlgorithmVisualizer.Api.Models;

namespace AlgorithmVisualizer.Api.Services;

public class GraphService
{
    private static List<List<int[]>> BuildWeightedAdj(
        int nodeCount,
        int[][] edges,
        bool directed = false
    )
    {
        var adj = Enumerable.Range(0, nodeCount).Select(_ => new List<int[]>()).ToList();
        foreach (var e in edges)
        {
            int u = e[0],
                v = e[1],
                w = e.Length > 2 ? e[2] : 1;
            adj[u].Add([v, w]);
            if (!directed)
                adj[v].Add([u, w]);
        }
        return adj;
    }

    private static List<List<int>> BuildAdj(int nodeCount, int[][] edges, bool directed = false)
    {
        var adj = Enumerable.Range(0, nodeCount).Select(_ => new List<int>()).ToList();
        foreach (var e in edges)
        {
            adj[e[0]].Add(e[1]);
            if (!directed)
                adj[e[1]].Add(e[0]);
        }
        return adj;
    }

    // 1. BFS
    // Time: O(V + E)
    // Space: O(V)
    public List<AlgorithmStep> Bfs(GraphRequest req)
    {
        var adj = BuildAdj(req.NodeCount, req.Edges);
        var visited = new int[req.NodeCount];
        var steps = new List<AlgorithmStep>();
        int step = 0;
        var queue = new Queue<int>();
        queue.Enqueue(req.StartNode);
        visited[req.StartNode] = 1;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])visited.Clone(),
                Description = $"BFS from node {req.StartNode}",
            }
        );

        while (queue.Count > 0)
        {
            int node = queue.Dequeue();
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])visited.Clone(),
                    Description = $"Process node {node}",
                    HighlightIndices = [node],
                }
            );

            foreach (int nb in adj[node])
            {
                if (visited[nb] == 0)
                {
                    visited[nb] = 1;
                    queue.Enqueue(nb);
                    steps.Add(
                        new AlgorithmStep
                        {
                            StepNumber = step++,
                            Array = (int[])visited.Clone(),
                            Description = $"Visit node {nb} from {node}",
                            HighlightIndices = [nb],
                            SortedIndices = Enumerable
                                .Range(0, req.NodeCount)
                                .Where(i => visited[i] == 1)
                                .ToArray(),
                        }
                    );
                }
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])visited.Clone(),
                Description = "BFS complete",
                SortedIndices = Enumerable
                    .Range(0, req.NodeCount)
                    .Where(i => visited[i] == 1)
                    .ToArray(),
            }
        );
        return steps;
    }

    // 2. DFS
    // Time: O(V + E)
    // Space: O(V)
    public List<AlgorithmStep> Dfs(GraphRequest req)
    {
        var adj = BuildAdj(req.NodeCount, req.Edges);
        var visited = new int[req.NodeCount];
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])visited.Clone(),
                Description = $"DFS from node {req.StartNode}",
            }
        );
        DfsHelper(req.StartNode, adj, visited, steps, ref step, req.NodeCount);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])visited.Clone(),
                Description = "DFS complete",
                SortedIndices = Enumerable
                    .Range(0, req.NodeCount)
                    .Where(i => visited[i] == 1)
                    .ToArray(),
            }
        );
        return steps;
    }

    private static void DfsHelper(
        int node,
        List<List<int>> adj,
        int[] visited,
        List<AlgorithmStep> steps,
        ref int step,
        int n
    )
    {
        visited[node] = 1;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])visited.Clone(),
                Description = $"Visit node {node}",
                HighlightIndices = [node],
            }
        );
        foreach (int nb in adj[node])
            if (visited[nb] == 0)
                DfsHelper(nb, adj, visited, steps, ref step, n);
    }

    // 3. Dijkstra
    // Time: O((V + E) log V)
    // Space: O(V)
    public List<AlgorithmStep> Dijkstra(GraphRequest req)
    {
        var adj = BuildWeightedAdj(req.NodeCount, req.Edges);
        var dist = new int[req.NodeCount];
        Array.Fill(dist, int.MaxValue);
        dist[req.StartNode] = 0;
        var done = new bool[req.NodeCount];
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = dist.Select(d => d == int.MaxValue ? -1 : d).ToArray(),
                Description = $"Dijkstra from node {req.StartNode}",
            }
        );

        for (int i = 0; i < req.NodeCount; i++)
        {
            int u = -1;
            for (int v = 0; v < req.NodeCount; v++)
                if (!done[v] && (u == -1 || dist[v] < dist[u]))
                    u = v;
            if (u == -1 || dist[u] == int.MaxValue)
                break;
            done[u] = true;

            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = dist.Select(d => d == int.MaxValue ? -1 : d).ToArray(),
                    Description = $"Finalize node {u} (dist={dist[u]})",
                    HighlightIndices = [u],
                    SortedIndices = Enumerable
                        .Range(0, req.NodeCount)
                        .Where(j => done[j])
                        .ToArray(),
                }
            );

            foreach (var edge in adj[u])
            {
                int v = edge[0],
                    w = edge[1];
                if (dist[u] + w < dist[v])
                {
                    dist[v] = dist[u] + w;
                    steps.Add(
                        new AlgorithmStep
                        {
                            StepNumber = step++,
                            Array = dist.Select(d => d == int.MaxValue ? -1 : d).ToArray(),
                            Description = $"Relax edge {u}→{v}: dist={dist[v]}",
                            HighlightIndices = [v],
                        }
                    );
                }
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = dist.Select(d => d == int.MaxValue ? -1 : d).ToArray(),
                Description =
                    $"Dijkstra complete. Distances: [{string.Join(", ", dist.Select(d => d == int.MaxValue ? "∞" : d.ToString()))}]",
                SortedIndices = Enumerable.Range(0, req.NodeCount).ToArray(),
            }
        );
        return steps;
    }

    // 4. Bellman-Ford
    // Time: O(V · E)
    // Space: O(V)
    public List<AlgorithmStep> BellmanFord(GraphRequest req)
    {
        var dist = new int[req.NodeCount];
        Array.Fill(dist, int.MaxValue);
        dist[req.StartNode] = 0;
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = dist.Select(d => d == int.MaxValue ? -1 : d).ToArray(),
                Description = $"Bellman-Ford from node {req.StartNode}",
            }
        );

        for (int i = 0; i < req.NodeCount - 1; i++)
        {
            bool updated = false;
            foreach (var e in req.Edges)
            {
                int u = e[0],
                    v = e[1],
                    w = e.Length > 2 ? e[2] : 1;
                if (dist[u] != int.MaxValue && dist[u] + w < dist[v])
                {
                    dist[v] = dist[u] + w;
                    updated = true;
                    steps.Add(
                        new AlgorithmStep
                        {
                            StepNumber = step++,
                            Array = dist.Select(d => d == int.MaxValue ? -1 : d).ToArray(),
                            Description = $"Iteration {i + 1}: relax {u}→{v}, dist[{v}]={dist[v]}",
                            HighlightIndices = [v],
                        }
                    );
                }
            }
            if (!updated)
                break;
        }

        // Check negative cycle
        bool negativeCycle = false;
        foreach (var e in req.Edges)
        {
            int u = e[0],
                v = e[1],
                w = e.Length > 2 ? e[2] : 1;
            if (dist[u] != int.MaxValue && dist[u] + w < dist[v])
            {
                negativeCycle = true;
                break;
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = dist.Select(d => d == int.MaxValue ? -1 : d).ToArray(),
                Description = negativeCycle
                    ? "Negative cycle detected!"
                    : $"Bellman-Ford complete. Distances: [{string.Join(", ", dist.Select(d => d == int.MaxValue ? "∞" : d.ToString()))}]",
                SortedIndices = Enumerable.Range(0, req.NodeCount).ToArray(),
            }
        );
        return steps;
    }
    // 6. Topological Sort
    // Time: O(V + E)
    // Space: O(V)
    public List<AlgorithmStep> TopologicalSort(GraphRequest req)
    {
        var adj = BuildAdj(req.NodeCount, req.Edges, directed: true);
        var inDeg = new int[req.NodeCount];
        foreach (var e in req.Edges)
            inDeg[e[1]]++;
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])inDeg.Clone(),
                Description = "In-degrees computed",
            }
        );

        var queue = new Queue<int>();
        for (int i = 0; i < req.NodeCount; i++)
            if (inDeg[i] == 0)
                queue.Enqueue(i);

        var result = new List<int>();
        while (queue.Count > 0)
        {
            int u = queue.Dequeue();
            result.Add(u);
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = result.ToArray(),
                    Description = $"Process node {u} (in-degree 0)",
                    HighlightIndices = [result.Count - 1],
                }
            );

            foreach (int v in adj[u])
            {
                inDeg[v]--;
                if (inDeg[v] == 0)
                    queue.Enqueue(v);
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = result.ToArray(),
                Description =
                    result.Count == req.NodeCount
                        ? $"Topological order: [{string.Join(", ", result)}]"
                        : "Cycle detected — no valid topological order",
                SortedIndices = Enumerable.Range(0, result.Count).ToArray(),
            }
        );
        return steps;
    }

    // 7. Cycle Detection
    // Time: O(V + E)
    // Space: O(V)
    public List<AlgorithmStep> CycleDetection(GraphRequest req)
    {
        var adj = BuildAdj(req.NodeCount, req.Edges, directed: true);
        var color = new int[req.NodeCount]; // 0=white, 1=gray, 2=black
        var steps = new List<AlgorithmStep>();
        int step = 0;
        bool hasCycle = false;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])color.Clone(),
                Description = "Cycle detection (0=unvisited, 1=in-stack, 2=done)",
            }
        );

        for (int u = 0; u < req.NodeCount && !hasCycle; u++)
            if (color[u] == 0)
                hasCycle = CycleDfs(u, adj, color, steps, ref step, ref hasCycle);

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])color.Clone(),
                Description = hasCycle ? "Cycle detected!" : "No cycle found",
                SortedIndices = Enumerable.Range(0, req.NodeCount).ToArray(),
            }
        );
        return steps;
    }

    private static bool CycleDfs(
        int u,
        List<List<int>> adj,
        int[] color,
        List<AlgorithmStep> steps,
        ref int step,
        ref bool found
    )
    {
        color[u] = 1;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])color.Clone(),
                Description = $"Enter node {u} (gray)",
                HighlightIndices = [u],
            }
        );
        foreach (int v in adj[u])
        {
            if (color[v] == 1)
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])color.Clone(),
                        Description = $"Back edge {u}→{v}: cycle!",
                        HighlightIndices = [u, v],
                    }
                );
                found = true;
                return true;
            }
            if (color[v] == 0 && CycleDfs(v, adj, color, steps, ref step, ref found))
                return true;
        }
        color[u] = 2;
        return false;
    }

    // 8. Union-Find
    // Time: O(α(n)) per operation (nearly O(1) amortized)
    // Space: O(V)
    public List<AlgorithmStep> UnionFind(GraphRequest req)
    {
        var parent = Enumerable.Range(0, req.NodeCount).ToArray();
        var rank = new int[req.NodeCount];
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])parent.Clone(),
                Description = "Union-Find: each node is its own parent",
            }
        );

        foreach (var e in req.Edges)
        {
            int u = e[0],
                v = e[1];
            int pu = Find(parent, u),
                pv = Find(parent, v);
            if (pu == pv)
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])parent.Clone(),
                        Description = $"Edge {u}-{v}: same set (root={pu}), cycle!",
                        HighlightIndices = [u, v],
                    }
                );
                continue;
            }
            if (rank[pu] < rank[pv])
                parent[pu] = pv;
            else if (rank[pu] > rank[pv])
                parent[pv] = pu;
            else
            {
                parent[pv] = pu;
                rank[pu]++;
            }
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])parent.Clone(),
                    Description = $"Union({u},{v}): merge sets of {pu} and {pv}",
                    HighlightIndices = [u, v],
                }
            );
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])parent.Clone(),
                Description = $"Union-Find complete. Parent: [{string.Join(", ", parent)}]",
                SortedIndices = Enumerable.Range(0, req.NodeCount).ToArray(),
            }
        );
        return steps;
    }

    private static int Find(int[] parent, int x)
    {
        while (parent[x] != x)
        {
            parent[x] = parent[parent[x]];
            x = parent[x];
        }
        return x;
    }

    // 9. Kruskal
    // Time: O(E log E)
    // Space: O(V + E)
    public List<AlgorithmStep> Kruskal(GraphRequest req)
    {
        var edges = req
            .Edges.Select(e => new
            {
                u = e[0],
                v = e[1],
                w = e.Length > 2 ? e[2] : 1,
            })
            .OrderBy(e => e.w)
            .ToArray();
        var parent = Enumerable.Range(0, req.NodeCount).ToArray();
        var rank = new int[req.NodeCount];
        var mstWeights = new int[req.NodeCount];
        var steps = new List<AlgorithmStep>();
        int step = 0,
            totalWeight = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])mstWeights.Clone(),
                Description = "Kruskal's MST: edges sorted by weight",
            }
        );

        foreach (var e in edges)
        {
            int pu = Find(parent, e.u),
                pv = Find(parent, e.v);
            if (pu != pv)
            {
                if (rank[pu] < rank[pv])
                    parent[pu] = pv;
                else if (rank[pu] > rank[pv])
                    parent[pv] = pu;
                else
                {
                    parent[pv] = pu;
                    rank[pu]++;
                }
                totalWeight += e.w;
                mstWeights[e.u] = Math.Max(mstWeights[e.u], e.w);
                mstWeights[e.v] = Math.Max(mstWeights[e.v], e.w);
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])mstWeights.Clone(),
                        Description = $"Add edge {e.u}-{e.v} (weight {e.w}), total={totalWeight}",
                        HighlightIndices = [e.u, e.v],
                    }
                );
            }
            else
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])mstWeights.Clone(),
                        Description = $"Skip edge {e.u}-{e.v} (weight {e.w}): would form cycle",
                    }
                );
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])mstWeights.Clone(),
                Description = $"MST total weight = {totalWeight}",
                SortedIndices = Enumerable.Range(0, req.NodeCount).ToArray(),
            }
        );
        return steps;
    }

    // 10. Prim
    // Time: O((V + E) log V)
    // Space: O(V)
    public List<AlgorithmStep> Prim(GraphRequest req)
    {
        var adj = BuildWeightedAdj(req.NodeCount, req.Edges);
        var inMst = new bool[req.NodeCount];
        var key = new int[req.NodeCount];
        Array.Fill(key, int.MaxValue);
        key[req.StartNode] = 0;
        var steps = new List<AlgorithmStep>();
        int step = 0,
            totalWeight = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = key.Select(k => k == int.MaxValue ? -1 : k).ToArray(),
                Description = $"Prim's MST from node {req.StartNode}",
            }
        );

        for (int count = 0; count < req.NodeCount; count++)
        {
            int u = -1;
            for (int v = 0; v < req.NodeCount; v++)
                if (!inMst[v] && (u == -1 || key[v] < key[u]))
                    u = v;
            if (u == -1 || key[u] == int.MaxValue)
                break;
            inMst[u] = true;
            totalWeight += key[u];

            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = key.Select(k => k == int.MaxValue ? -1 : k).ToArray(),
                    Description = $"Add node {u} (key={key[u]}), MST weight={totalWeight}",
                    HighlightIndices = [u],
                    SortedIndices = Enumerable
                        .Range(0, req.NodeCount)
                        .Where(i => inMst[i])
                        .ToArray(),
                }
            );

            foreach (var edge in adj[u])
            {
                int v = edge[0],
                    w = edge[1];
                if (!inMst[v] && w < key[v])
                {
                    key[v] = w;
                    steps.Add(
                        new AlgorithmStep
                        {
                            StepNumber = step++,
                            Array = key.Select(k => k == int.MaxValue ? -1 : k).ToArray(),
                            Description = $"Update key[{v}] = {w} via edge {u}-{v}",
                            HighlightIndices = [v],
                        }
                    );
                }
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = key.Select(k => k == int.MaxValue ? -1 : k).ToArray(),
                Description = $"Prim's MST complete. Total weight = {totalWeight}",
                SortedIndices = Enumerable.Range(0, req.NodeCount).ToArray(),
            }
        );
        return steps;
    }
}
