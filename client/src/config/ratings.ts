export interface AlgorithmRating {
  stars: number;
  tooltip: string;
}

export const algorithmRatings: Record<string, AlgorithmRating> = {
  // Array
  "bubble-sort":     { stars: 2, tooltip: "Practical only on nearly-sorted tiny datasets where simplicity beats speed. Rarely the right tool in production." },
  "quick-sort":      { stars: 4, tooltip: "Default sort in most runtimes. Understanding partitioning also unlocks quickselect and k-th element problems." },
  "merge-sort":      { stars: 4, tooltip: "Stable sort of choice when guaranteed O(n log n) is required, or when sorting linked lists." },
  "insertion-sort":  { stars: 2, tooltip: "Useful for nearly-sorted data or as the small-array cutoff inside Timsort and Introsort." },
  "selection-sort":  { stars: 1, tooltip: "Rarely used in practice. Only worth reaching for when write operations are extremely expensive." },
  "binary-search":   { stars: 5, tooltip: "Reaches for this constantly — any sorted data lookup, range query, or monotonic condition check." },
  "two-pointers":    { stars: 5, tooltip: "Core pattern for pair sum, removing duplicates in-place, container with most water, and more." },
  "sliding-window":  { stars: 5, tooltip: "Core pattern for max-sum subarray of size k, longest substring without repeat, minimum window substring." },
  "kadane":          { stars: 5, tooltip: "The go-to whenever you need the best contiguous range in a sequence. Generalizes to 2D and circular variants." },

  // Number Theory
  "bit-manipulation":     { stars: 4, tooltip: "Used regularly for flag fields, power-of-two checks, XOR tricks, and low-level bitwise optimizations." },

  // Backtracking
  "permutations":              { stars: 4, tooltip: "Whenever you need all orderings of a set — scheduling, anagram generation, combinatorial search." },
  "subsets":                   { stars: 4, tooltip: "Power-set enumeration — feature selection, exhaustive combinatorial search, bitmask DP seeds." },
  "combination-sum":           { stars: 4, tooltip: "Used when exploring subsets that hit a target — coin systems, tile problems, knapsack variants with repetition." },
  "palindrome-partitioning":   { stars: 3, tooltip: "Splits a sequence into valid chunks — useful in text segmentation and partition DP problems." },

  // Dynamic Programming
  "fibonacci":      { stars: 5, tooltip: "Gateway DP pattern — top-down memoization vs bottom-up tabulation. Base of many recurrence-based problems." },
  "knapsack":       { stars: 4, tooltip: "Archetype for bounded resource allocation — maximizing value under a weight or budget constraint." },
  "lcs":            { stars: 4, tooltip: "Measures similarity between sequences — used in diff tools, DNA analysis, and plagiarism detection." },
  "lis":            { stars: 4, tooltip: "Used in patience sorting, chain scheduling, and sequence analysis. O(n log n) variant is worth knowing." },
  "coin-change":    { stars: 5, tooltip: "Classic unbounded knapsack variant — models currency systems, tile problems, and resource-allocation minimization." },
  "climbing-stairs": { stars: 5, tooltip: "Entry-point DP problem. Every slight variation (k steps, cost array) maps to a real sub-problem pattern." },

  // Graph
  "bfs":               { stars: 5, tooltip: "Shortest path in unweighted graphs, level-order traversal, flood fill, multi-source spreading." },
  "dfs":               { stars: 5, tooltip: "Connected components, cycle detection, topological ordering, path enumeration." },
  "dijkstra":          { stars: 4, tooltip: "Shortest path in weighted graphs — route planning, network latency, map navigation." },
  "topological-sort":  { stars: 4, tooltip: "Dependency resolution — build systems, course prerequisites, task scheduling with ordering constraints." },
  "cycle-detection":   { stars: 4, tooltip: "Validates DAGs, detects deadlocks, and checks for circular dependencies in directed and undirected graphs." },
  "kruskal":           { stars: 3, tooltip: "Minimum spanning tree — network cabling, clustering, finding the cheapest full graph connection." },
  "prim":              { stars: 2, tooltip: "Alternative MST approach favored in dense graphs where an adjacency matrix representation dominates." },

  // Tree
  "bst-insert-search":  { stars: 5, tooltip: "Foundation for ordered maps, sets, and auto-complete structures. Core BST insert and lookup operations." },
  "inorder":            { stars: 5, tooltip: "Produces sorted output from a BST — used in range queries, floor/ceil lookups, and k-th element problems." },
  "preorder":           { stars: 5, tooltip: "Root-first traversal — used in tree serialization, cloning, and expression tree evaluation." },
  "postorder":          { stars: 4, tooltip: "Process children before parent — used in tree deletion, expression evaluation, and bottom-up DP on trees." },
  "level-order":        { stars: 5, tooltip: "BFS on trees — right/left side view, average value per level, zigzag traversal, and more." },
  "lca":                { stars: 4, tooltip: "Lowest common ancestor — used in tree distance queries, routing, and hierarchical data lookups." },
  "diameter":           { stars: 4, tooltip: "Longest path in a tree — used in network latency analysis and tree structure metrics." },
  "validate-bst":       { stars: 5, tooltip: "Ensures the BST invariant holds with min/max bounds — critical before using any BST-dependent algorithm." },
  "invert":             { stars: 5, tooltip: "Mirrors a binary tree recursively — tests tree manipulation; also appears in UI layout transformations." },
  "huffman":            { stars: 2, tooltip: "Greedy prefix coding — the basis of compression algorithms like ZIP, JPEG, and MP3." },

  // String
  "linear-search":          { stars: 2, tooltip: "Naïve brute-force pattern scan — baseline for understanding why efficient string algorithms matter." },
  "kmp":                    { stars: 3, tooltip: "Efficient single-pattern search — avoids redundant comparisons using the failure function." },
  "boyer-moore":            { stars: 2, tooltip: "Fastest practical string search in many real-world scenarios; used in text editors and grep implementations." },
  "rabin-karp":             { stars: 3, tooltip: "Rolling hash enables multi-pattern matching and plagiarism detection over large corpora." },
  "longest-palindrome":     { stars: 5, tooltip: "Expand-around-center or Manacher's — used in substring analysis, symmetry detection, and string DP." },
  "anagram-detection":      { stars: 5, tooltip: "Frequency map comparison — appears in grouping, substring matching, and permutation-in-string checks." },
  "reversal":               { stars: 3, tooltip: "In-place string flip — foundational step in rotation, palindrome verification, and word-order problems." },
};
