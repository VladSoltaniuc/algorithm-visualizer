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

    // 6. Topological Sort (Kahn's Algorithm)
    // Time: O(V + E)
    // Space: O(V)
    public List<AlgorithmStep> TopologicalSort(GraphRequest req)
    {
        int n = req.NodeCount;
        var adj = BuildAdj(n, req.Edges, directed: true);
        var inDeg = new int[n];
        foreach (var e in req.Edges)
            inDeg[e[1]]++;

        var steps = new List<AlgorithmStep>();
        int stepNum = 0;

        // Step 0: show initial in-degrees
        var initialReady = Enumerable.Range(0, n).Where(i => inDeg[i] == 0).ToList();
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum++,
                Array = (int[])inDeg.Clone(),
                Description =
                    $"In-degrees computed. Nodes with in-degree 0 are ready to process: [{string.Join(", ", initialReady)}].",
                HighlightIndices = initialReady.ToArray(),
                Notes = Enumerable.Range(0, n).Select(i => $"in:{inDeg[i]}").ToArray(),
            }
        );

        var queue = new Queue<int>();
        foreach (var z in initialReady)
            queue.Enqueue(z);

        var result = new List<int>();
        var done = new List<int>();

        while (queue.Count > 0)
        {
            int u = queue.Dequeue();
            result.Add(u);
            done.Add(u);

            var newlyReady = new List<int>();
            foreach (int v in adj[u])
            {
                inDeg[v]--;
                if (inDeg[v] == 0)
                {
                    queue.Enqueue(v);
                    newlyReady.Add(v);
                }
            }

            string neighborDesc =
                adj[u].Count > 0
                    ? $" Edges processed: {string.Join(", ", adj[u].Select(v => $"{u}→{v} (in-deg now {inDeg[v]})"))}."
                    : " No outgoing edges.";
            string readyDesc =
                newlyReady.Count > 0 ? $" Newly ready: [{string.Join(", ", newlyReady)}]." : "";

            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = stepNum++,
                    Array = (int[])inDeg.Clone(),
                    Description =
                        $"Dequeue node {u} → topological position #{result.Count}.{neighborDesc}{readyDesc}",
                    HighlightIndices = new[] { u }.Concat(newlyReady).ToArray(),
                    SortedIndices = done.ToArray(),
                    Notes = Enumerable.Range(0, n).Select(i => $"in:{inDeg[i]}").ToArray(),
                }
            );
        }

        bool valid = result.Count == n;
        var finalNotes = new string[n];
        for (int i = 0; i < result.Count; i++)
            finalNotes[result[i]] = $"#{i + 1}";
        for (int i = 0; i < n; i++)
            if (finalNotes[i] == null)
                finalNotes[i] = "×";

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum,
                Array = (int[])inDeg.Clone(),
                Description = valid
                    ? $"Done! Topological order: [{string.Join(" → ", result)}]."
                    : "Cycle detected — no valid topological order exists.",
                SortedIndices = valid ? Enumerable.Range(0, n).ToArray() : [],
                Notes = finalNotes,
            }
        );

        return steps;
    }

    // 7. Cycle Detection (DFS coloring for directed graphs)
    // Time: O(V + E)
    // Space: O(V)
    public List<AlgorithmStep> CycleDetection(GraphRequest req)
    {
        var adj = BuildAdj(req.NodeCount, req.Edges, directed: true);
        int n = req.NodeCount;
        var color = new int[n]; // 0=unvisited, 1=on DFS stack (gray), 2=fully done (black)
        var dfsStack = new List<int>();
        var steps = new List<AlgorithmStep>();
        int stepNum = 0;
        bool hasCycle = false;

        for (int u = 0; u < n && !hasCycle; u++)
            if (color[u] == 0)
                hasCycle = CycleDfs(u, adj, color, dfsStack, steps, ref stepNum, n);

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum,
                Array = (int[])color.Clone(),
                Description = hasCycle
                    ? "Cycle detected — a back edge was found. This graph is not a DAG."
                    : $"No cycle — all {n} nodes fully explored without finding a back edge.",
                SortedIndices = hasCycle ? [] : Enumerable.Range(0, n).ToArray(),
                Notes = DfsNotes(color),
                Labels = DfsLabels(color),
            }
        );
        return steps;
    }

    private static string[] DfsNotes(int[] c) =>
        c.Select(ci =>
                ci switch
                {
                    0 => "?",
                    1 => "stack",
                    _ => "\u2713",
                }
            )
            .ToArray();

    private static string[] DfsLabels(int[] c) =>
        c.Select(ci =>
                ci switch
                {
                    0 => "unvisited",
                    1 => "stack",
                    _ => "done",
                }
            )
            .ToArray();

    private static bool CycleDfs(
        int u,
        List<List<int>> adj,
        int[] color,
        List<int> dfsStack,
        List<AlgorithmStep> steps,
        ref int stepNum,
        int n
    )
    {
        color[u] = 1;
        dfsStack.Add(u);
        string pathStr = string.Join(" → ", dfsStack);

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum++,
                Array = (int[])color.Clone(),
                Description = $"Enter node {u} — mark as on-stack (orange). DFS path: {pathStr}.",
                HighlightIndices = [u],
                SortedIndices = Enumerable.Range(0, n).Where(i => color[i] == 2).ToArray(),
                Notes = DfsNotes(color),
                Labels = DfsLabels(color),
            }
        );

        foreach (int v in adj[u])
        {
            if (color[v] == 1) // back edge → cycle
            {
                int cycleStart = dfsStack.IndexOf(v);
                var cyclePath = dfsStack.Skip(cycleStart).ToList();
                string cycleStr = string.Join(" → ", cyclePath) + $" → {v}";
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = stepNum++,
                        Array = (int[])color.Clone(),
                        Description =
                            $"Back edge {u}→{v}: node {v} is already on the DFS stack (orange)! "
                            + $"Cycle found: {cycleStr}.",
                        HighlightIndices = cyclePath.ToArray(),
                        SortedIndices = Enumerable.Range(0, n).Where(i => color[i] == 2).ToArray(),
                        Notes = DfsNotes(color),
                        Labels = DfsLabels(color),
                    }
                );
                return true;
            }
            if (color[v] == 0)
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = stepNum++,
                        Array = (int[])color.Clone(),
                        Description =
                            $"Edge {u}→{v}: node {v} is unvisited (blue) — recurse into it.",
                        HighlightIndices = [u, v],
                        SortedIndices = Enumerable.Range(0, n).Where(i => color[i] == 2).ToArray(),
                        Notes = DfsNotes(color),
                        Labels = DfsLabels(color),
                    }
                );
                if (CycleDfs(v, adj, color, dfsStack, steps, ref stepNum, n))
                    return true;
            }
            else // color[v] == 2 — already fully explored, safe
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = stepNum++,
                        Array = (int[])color.Clone(),
                        Description =
                            $"Edge {u}→{v}: node {v} is already fully explored (green) — no back edge here, safe to skip.",
                        HighlightIndices = [u, v],
                        SortedIndices = Enumerable.Range(0, n).Where(i => color[i] == 2).ToArray(),
                        Notes = DfsNotes(color),
                        Labels = DfsLabels(color),
                    }
                );
            }
        }

        color[u] = 2;
        dfsStack.RemoveAt(dfsStack.Count - 1);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum++,
                Array = (int[])color.Clone(),
                Description =
                    $"Backtrack from node {u} — all neighbors explored, no cycle. Mark {u} as fully done (green).",
                SortedIndices = Enumerable.Range(0, n).Where(i => color[i] == 2).ToArray(),
                Notes = DfsNotes(color),
                Labels = DfsLabels(color),
            }
        );
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
        int n = req.NodeCount;
        int componentCount = n;

        // Non-mutating root finder — used only for label generation
        static int FindRootPure(int[] p, int x)
        {
            while (p[x] != x)
                x = p[x];
            return x;
        }

        string[] MakeLabels() =>
            Enumerable.Range(0, n).Select(i => FindRootPure(parent, i).ToString()).ToArray();

        string[] MakeNotes() =>
            Enumerable.Range(0, n).Select(i => parent[i] == i ? "root" : $"→{parent[i]}").ToArray();

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])parent.Clone(),
                Labels = MakeLabels(),
                Notes = MakeNotes(),
                Description =
                    $"Start: {n} nodes, each in its own component (parent[i] = i). "
                    + $"{componentCount} separate component(s).",
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
                        Labels = MakeLabels(),
                        Notes = MakeNotes(),
                        Description =
                            $"Edge {u}–{v}: both nodes already share root {pu} — same component. "
                            + "This edge would form a cycle, skipping it.",
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
            componentCount--;
            int newRoot = FindRootPure(parent, u);

            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])parent.Clone(),
                    Labels = MakeLabels(),
                    Notes = MakeNotes(),
                    Description =
                        $"Union({u}, {v}): merged component of {pu} into component of {pv} — "
                        + $"new root is {newRoot}. {componentCount} component(s) remaining.",
                    HighlightIndices = [u, v],
                }
            );
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])parent.Clone(),
                Labels = MakeLabels(),
                Notes = MakeNotes(),
                Description =
                    $"Done. {componentCount} connected component(s). "
                    + $"Parent array: [{string.Join(", ", parent)}].",
                SortedIndices = Enumerable.Range(0, n).ToArray(),
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
