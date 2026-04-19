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
        var processed = new HashSet<int>();
        var result = new List<int>();

        // Labels: "pending" | "ready" | "removed"
        string[] MakeLabels()
        {
            var l = new string[n];
            for (int i = 0; i < n; i++)
                l[i] =
                    processed.Contains(i) ? "removed"
                    : inDeg[i] == 0 ? "ready"
                    : "pending";
            return l;
        }

        // Notes: current in-degree (blank for removed nodes)
        string[] MakeNotes() =>
            Enumerable
                .Range(0, n)
                .Select(i => processed.Contains(i) ? "" : inDeg[i].ToString())
                .ToArray();

        var initialReady = Enumerable.Range(0, n).Where(i => inDeg[i] == 0).ToList();

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum++,
                Array = (int[])inDeg.Clone(),
                Description =
                    $"Compute in-degrees. Nodes with in-degree 0 are ready (no dependencies): [{string.Join(", ", initialReady)}].",
                HighlightIndices = initialReady.ToArray(),
                SortedIndices = [],
                Labels = MakeLabels(),
                Notes = MakeNotes(),
            }
        );

        var queue = new Queue<int>();
        foreach (var z in initialReady)
            queue.Enqueue(z);

        while (queue.Count > 0)
        {
            int u = queue.Dequeue();
            result.Add(u);

            // First step: highlight the chosen node before removing its edges
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = stepNum++,
                    Array = (int[])inDeg.Clone(),
                    Description =
                        $"Node {u} has in-degree 0 — pick it as topo position #{result.Count}. Now remove its outgoing edges.",
                    HighlightIndices = [u],
                    SortedIndices = result.ToArray(),
                    Labels = MakeLabels(),
                    Notes = MakeNotes(),
                }
            );

            processed.Add(u);

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

            string edgeDesc =
                adj[u].Count > 0
                    ? $" Removed edges from {u}; updated in-degrees."
                    : " No outgoing edges.";
            string readyDesc =
                newlyReady.Count > 0
                    ? $" Newly at in-degree 0: [{string.Join(", ", newlyReady)}]."
                    : "";

            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = stepNum++,
                    Array = (int[])inDeg.Clone(),
                    Description = $"Node {u} removed from graph.{edgeDesc}{readyDesc}",
                    HighlightIndices = newlyReady.ToArray(),
                    SortedIndices = result.ToArray(),
                    Labels = MakeLabels(),
                    Notes = MakeNotes(),
                }
            );
        }

        bool valid = result.Count == n;
        if (valid)
        {
            // Final step: show full topo order with position labels
            var finalNotes = new string[n];
            for (int i = 0; i < result.Count; i++)
                finalNotes[result[i]] = $"#{i + 1}";

            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = stepNum,
                    Array = (int[])inDeg.Clone(),
                    Description = $"Done! Topological order: {string.Join(" → ", result)}.",
                    SortedIndices = result.ToArray(),
                    Labels = Enumerable.Range(0, n).Select(_ => "placed").ToArray(),
                    Notes = finalNotes,
                }
            );
        }
        else
        {
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = stepNum,
                    Array = (int[])inDeg.Clone(),
                    Description = "Cycle detected — no valid topological order exists.",
                    SortedIndices = result.ToArray(),
                    Labels = MakeLabels(),
                    Notes = MakeNotes(),
                }
            );
        }

        return steps;
    }

    // 7. Cycle Detection — DFS with visited[] + pathVisited[] (Striver G-19)
    // Time: O(V + E), Space: O(V)
    public List<AlgorithmStep> CycleDetection(GraphRequest req)
    {
        var adj = BuildAdj(req.NodeCount, req.Edges, directed: true);
        int n = req.NodeCount;

        var visited = new bool[n];
        var pathVisited = new bool[n];
        int[] nodeIds = Enumerable.Range(0, n).ToArray();
        var steps = new List<AlgorithmStep>();
        int stepNum = 0;

        AlgorithmStep MakeStep(
            string desc,
            int[]? highlight = null,
            int[]? sorted = null,
            int changedNode = -1
        ) =>
            new()
            {
                StepNumber = stepNum++,
                Array = (int[])nodeIds.Clone(),
                Description = desc,
                HighlightIndices = highlight ?? [],
                SortedIndices = sorted ?? [],
                Labels = CycleLabels(visited, pathVisited, n),
                Notes = new string[n],
                DpMatrix = CycleMatrix(visited, pathVisited),
                RowLabels = ["visited", "pathVisited"],
                ColLabels = Enumerable.Range(0, n).Select(i => i.ToString()).ToArray(),
                HighlightCol = changedNode,
            };

        steps.Add(
            MakeStep("Initialize: visited[] and pathVisited[] are both false for all nodes.")
        );

        List<int>? cycleNodes = null;

        for (int start = 0; start < n && cycleNodes == null; start++)
        {
            if (visited[start])
                continue;

            steps.Add(
                MakeStep($"Node {start} is not visited — start DFS from here.", highlight: [start])
            );

            // Iterative DFS with explicit stack — (node, neighborIndex) pairs
            var stack = new Stack<(int node, int nextIdx)>();
            visited[start] = true;
            pathVisited[start] = true;
            stack.Push((start, 0));

            steps.Add(
                MakeStep(
                    $"Mark node {start}: visited = true, pathVisited = true.",
                    highlight: [start],
                    changedNode: start
                )
            );

            while (stack.Count > 0 && cycleNodes == null)
            {
                var (u, idx) = stack.Pop();

                if (idx < adj[u].Count)
                {
                    int v = adj[u][idx];
                    stack.Push((u, idx + 1));

                    if (!visited[v])
                    {
                        // Not visited → mark visited + pathVisited, push onto stack
                        visited[v] = true;
                        pathVisited[v] = true;
                        stack.Push((v, 0));

                        steps.Add(
                            MakeStep(
                                $"Edge {u} → {v}: node {v} not visited — mark visited and pathVisited.",
                                highlight: [u, v],
                                changedNode: v
                            )
                        );
                    }
                    else if (pathVisited[v])
                    {
                        // Already in current path → back edge → cycle!
                        steps.Add(
                            MakeStep(
                                $"Edge {u} → {v}: node {v} is visited AND on current path — back edge found! Cycle!",
                                highlight: [u, v],
                                changedNode: v
                            )
                        );

                        // Reconstruct cycle from the DFS stack
                        cycleNodes = [v];
                        foreach (var (sn, _) in stack)
                        {
                            cycleNodes.Add(sn);
                            if (sn == v)
                                break;
                        }
                        cycleNodes.Reverse();
                    }
                    else
                    {
                        // Visited but not in current path → already fully processed, skip
                        steps.Add(
                            MakeStep(
                                $"Edge {u} → {v}: node {v} already processed (not on current path) — skip.",
                                highlight: [u],
                                sorted: [v]
                            )
                        );
                    }
                }
                else
                {
                    // All neighbors done → backtrack: unmark pathVisited
                    pathVisited[u] = false;

                    steps.Add(
                        MakeStep(
                            $"Node {u}: done exploring — backtrack, remove from current path.",
                            sorted: [u],
                            changedNode: u
                        )
                    );
                }
            }
        }

        // Final step
        if (cycleNodes != null)
        {
            steps.Add(
                MakeStep(
                    $"Cycle found: {string.Join(" → ", cycleNodes)}.",
                    highlight: cycleNodes.Distinct().ToArray()
                )
            );
        }
        else
        {
            steps.Add(
                MakeStep(
                    "DFS complete — no cycle detected. The graph is a DAG.",
                    sorted: Enumerable.Range(0, n).ToArray()
                )
            );
        }

        return steps;
    }

    private static string[] CycleLabels(bool[] visited, bool[] pathVisited, int n)
    {
        var labels = new string[n];
        for (int i = 0; i < n; i++)
        {
            if (pathVisited[i])
                labels[i] = "stack"; // in current DFS path
            else if (visited[i])
                labels[i] = "done"; // processed
            else
                labels[i] = "unvisited";
        }
        return labels;
    }

    private static int[][] CycleMatrix(bool[] visited, bool[] pathVisited)
    {
        return
        [
            visited.Select(v => v ? 1 : 0).ToArray(),
            pathVisited.Select(v => v ? 1 : 0).ToArray(),
        ];
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
