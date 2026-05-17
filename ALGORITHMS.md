# Algorithm Usability Guide

Extracted from all controllers in `AlgorithmVisualizer.Api`. Each algorithm is rated on a **1–5 scale** for how likely you are to reach for it when solving a real problem.

> **Scale:** ⭐ Rarely applicable · ⭐⭐ Niche use · ⭐⭐⭐ Situational · ⭐⭐⭐⭐ Broadly useful · ⭐⭐⭐⭐⭐ Reach for it constantly

---

## Array — `ArrayController`

| Algorithm | Usability | Notes |
|---|---|---|
| Binary Search | ⭐⭐⭐⭐⭐ | Reaches for this constantly — any sorted data lookup, range query, or monotonic condition check |
| Two Pointers | ⭐⭐⭐⭐⭐ | Core pattern for pair sum, removing duplicates in-place, container with most water, and more |
| Sliding Window | ⭐⭐⭐⭐⭐ | Core pattern for max-sum subarray of size k, longest substring without repeat, minimum window |
| Kadane's Algorithm | ⭐⭐⭐⭐⭐ | The go-to whenever you need the best contiguous range in a sequence |
| Merge Sort | ⭐⭐⭐⭐ | Stable sort of choice when guaranteed O(n log n) is required, or when sorting linked lists |
| Quick Sort | ⭐⭐⭐⭐ | Default sort in most runtimes; understanding partitioning also unlocks quickselect problems |
| Insertion Sort | ⭐⭐ | Useful for nearly-sorted data or as the small-array cutoff inside Timsort and Introsort |
| Bubble Sort | ⭐⭐ | Practical only on nearly-sorted tiny datasets where simplicity beats speed |
| Selection Sort | ⭐ | Only worth reaching for when write operations are extremely expensive |
| Linear Search | ⭐⭐ | Appears as a sub-step whenever a sorted structure isn't available |

---

## Number Theory — `NrTheoryController`

| Algorithm | Usability | Notes |
|---|---|---|
| Bit Manipulation | ⭐⭐⭐⭐ | Used regularly for flag fields, power-of-two checks, XOR tricks, and low-level optimizations |

---

## Backtracking — `BacktrackingController`

| Algorithm | Usability | Notes |
|---|---|---|
| Permutations | ⭐⭐⭐⭐ | Whenever you need all orderings of a set — scheduling, anagram generation, combinatorial search |
| Subsets / Power Set | ⭐⭐⭐⭐ | Feature selection, exhaustive combinatorial search, bitmask DP seeds |
| Combination Sum | ⭐⭐⭐⭐ | Used when exploring subsets that hit a target — coin systems, tile problems, knapsack variants |
| Palindrome Partitioning | ⭐⭐⭐ | Splits a sequence into valid chunks — text segmentation and partition DP problems |

---

## Dynamic Programming — `DynamicProgController`

| Algorithm | Usability | Notes |
|---|---|---|
| Climbing Stairs | ⭐⭐⭐⭐⭐ | Entry-point DP pattern. Every slight variation (k steps, cost array) maps to a real sub-problem |
| Fibonacci (memoized) | ⭐⭐⭐⭐⭐ | Gateway DP pattern — top-down memoization vs bottom-up tabulation |
| Coin Change | ⭐⭐⭐⭐⭐ | Classic unbounded knapsack variant — models currency, tile, and resource-allocation minimization |
| Longest Common Subsequence | ⭐⭐⭐⭐ | Measures similarity between sequences — diff tools, DNA analysis, plagiarism detection |
| Longest Increasing Subsequence | ⭐⭐⭐⭐ | Used in patience sorting, chain scheduling, and sequence analysis |
| 0/1 Knapsack | ⭐⭐⭐⭐ | Archetype for bounded resource allocation — maximizing value under a weight or budget constraint |

---

## Graph — `GraphController`

| Algorithm | Usability | Notes |
|---|---|---|
| BFS | ⭐⭐⭐⭐⭐ | Shortest path in unweighted graphs, level-order traversal, flood fill, multi-source spreading |
| DFS | ⭐⭐⭐⭐⭐ | Connected components, cycle detection, topological ordering, path enumeration |
| Topological Sort | ⭐⭐⭐⭐ | Dependency resolution — build systems, course prerequisites, task scheduling |
| Cycle Detection | ⭐⭐⭐⭐ | Validates DAGs, detects deadlocks, checks for circular dependencies |
| Dijkstra's Algorithm | ⭐⭐⭐⭐ | Shortest path in weighted graphs — route planning, network latency, map navigation |
| Kruskal's (MST) | ⭐⭐⭐ | Minimum spanning tree — network cabling, clustering, cheapest full graph connection |
| Prim's (MST) | ⭐⭐ | Alternative MST approach favored in dense graphs with adjacency matrix representation |

---

## String — `StringController`

| Algorithm | Usability | Notes |
|---|---|---|
| Longest Palindrome | ⭐⭐⭐⭐⭐ | Expand-around-center or Manacher's — substring analysis, symmetry detection, string DP |
| Anagram Detection | ⭐⭐⭐⭐⭐ | Frequency map comparison — grouping, substring matching, permutation-in-string checks |
| Longest Common Subsequence | ⭐⭐⭐⭐ | Sequence similarity — diff tools, DNA analysis, and edit-distance variants |
| KMP (Knuth-Morris-Pratt) | ⭐⭐⭐ | Efficient single-pattern search — avoids redundant comparisons using the failure function |
| Rabin-Karp | ⭐⭐⭐ | Rolling hash enables multi-pattern matching and plagiarism detection over large corpora |
| String Reversal | ⭐⭐⭐ | In-place string flip — foundational step in rotation, palindrome, and word-order problems |
| Boyer-Moore | ⭐⭐ | Fastest practical string search in many real-world scenarios; used in text editors and grep |
| Linear Search (naive) | ⭐⭐ | Naïve brute-force scan — baseline for understanding why efficient string algorithms exist |

---

## Tree — `TreeController`

| Algorithm | Usability | Notes |
|---|---|---|
| Inorder Traversal | ⭐⭐⭐⭐⭐ | Produces sorted output from a BST — range queries, floor/ceil lookups, k-th element |
| Preorder Traversal | ⭐⭐⭐⭐⭐ | Root-first — tree serialization, cloning, expression tree evaluation |
| Level Order Traversal | ⭐⭐⭐⭐⭐ | BFS on trees — right/left side view, average per level, zigzag traversal |
| BST Insert & Search | ⭐⭐⭐⭐⭐ | Foundation for ordered maps, sets, and auto-complete structures |
| Invert Binary Tree | ⭐⭐⭐⭐⭐ | Tests recursive tree manipulation; common in UI layout transformations |
| Validate BST | ⭐⭐⭐⭐⭐ | Ensures BST invariant holds — critical before using BST-dependent algorithms |
| Lowest Common Ancestor (LCA) | ⭐⭐⭐⭐ | Used in tree distance queries, routing, and hierarchical data lookups |
| Tree Height / Max Depth | ⭐⭐⭐⭐ | Fundamental recursive computation — used in balance checks and depth-limited search |
| Tree Diameter | ⭐⭐⭐⭐ | Longest path in tree — used in network latency analysis and tree structure metrics |
| Postorder Traversal | ⭐⭐⭐⭐ | Process children before parent — tree deletion, expression evaluation, bottom-up DP |
| Huffman Coding | ⭐⭐ | Greedy prefix coding — used in compression algorithms like ZIP, JPEG, and MP3 |

---

## Top 10 Most Broadly Applicable Algorithms

1. Binary Search
2. BFS & DFS (graphs + trees)
3. Two Pointers
4. Sliding Window
5. Kadane's Algorithm
6. Coin Change / Climbing Stairs (DP foundations)
7. Tree Traversals (all four, iterative + recursive)
8. Validate BST + Invert Tree
9. Anagram Detection + Longest Palindrome
10. Topological Sort
