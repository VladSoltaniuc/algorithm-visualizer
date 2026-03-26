import type { AlgorithmConfig } from '../types';

export const arrayConfig: AlgorithmConfig[] = [
  { name: 'Bubble Sort', slug: 'bubble-sort', endpoint: 'bubble-sort', category: 'array', defaultInput: '5,3,8,1,2', description: 'Repeatedly swaps adjacent elements if they are in the wrong order.' },
  { name: 'Quick Sort', slug: 'quick-sort', endpoint: 'quick-sort', category: 'array', defaultInput: '5,3,8,1,2,7,4', description: 'Picks a pivot, partitions the array, and recursively sorts the partitions. Lomuto partition scheme.' },
  { name: 'Merge Sort', slug: 'merge-sort', endpoint: 'merge-sort', category: 'array', defaultInput: '5,3,8,1,2,7,4', description: 'Divides the array in half, sorts each half, then merges them back together.' },
  { name: 'Binary Search', slug: 'binary-search', endpoint: 'binary-search', category: 'array', needsTarget: true, targetLabel: 'Search target', defaultInput: '1,3,5,7,9,11,13', defaultTarget: 7, description: 'Searches a sorted array by repeatedly dividing the search interval in half.' },
  { name: 'Linear Search', slug: 'linear-search', endpoint: 'linear-search', category: 'array', needsTarget: true, targetLabel: 'Search target', defaultInput: '5,3,8,1,2,7,4', defaultTarget: 7, description: 'Checks each element one by one until the target is found.' },
  { name: 'Insertion Sort', slug: 'insertion-sort', endpoint: 'insertion-sort', category: 'array', defaultInput: '5,3,8,1,2', description: 'Builds the sorted array one element at a time by inserting each into its correct position.' },
  { name: 'Selection Sort', slug: 'selection-sort', endpoint: 'selection-sort', category: 'array', defaultInput: '5,3,8,1,2', description: 'Finds the minimum element and places it at the beginning, repeating for the remaining array.' },
  { name: 'Two Pointers', slug: 'two-pointers', endpoint: 'two-pointers', category: 'array', needsTarget: true, targetLabel: 'Target sum', defaultInput: '1,2,3,4,5,6,7,8', defaultTarget: 9, description: 'Uses two pointers from both ends of a sorted array to find a pair that sums to a target.' },
  { name: 'Sliding Window', slug: 'sliding-window', endpoint: 'sliding-window', category: 'array', needsWindowSize: true, defaultInput: '2,1,5,1,3,2', defaultWindowSize: 3, description: 'Slides a fixed-size window across the array to find the maximum sum subarray.' },
  { name: "Kadane's Algorithm", slug: 'kadane', endpoint: 'kadane', category: 'array', defaultInput: '-2,1,-3,4,-1,2,1,-5,4', description: 'Finds the contiguous subarray with the maximum sum.' },
];

export const stringConfig: AlgorithmConfig[] = [
  { name: 'Linear Search in String', slug: 'linear-search', endpoint: 'linear-search', category: 'string', inputType: 'text', inputLabel: 'Text', defaultInput: 'hello world', needsPattern: true, patternLabel: 'Search char', defaultPattern: 'o', description: 'Scans each character of the string to find a target character.' },
  { name: 'KMP', slug: 'kmp', endpoint: 'kmp', category: 'string', inputType: 'text', inputLabel: 'Text', defaultInput: 'ABABDABACDABABCABAB', needsPattern: true, patternLabel: 'Pattern', defaultPattern: 'ABABCABAB', description: 'This algorithm scans text linearly, never going backwards. On a mismatch, it takes the substring of the pattern matched so far and checks: does a suffix of this match also appear as a prefix? Yes: the prefix is already aligned, matching resumes from after it. No: the pattern resets and scanning continues from the next character in the text.' },
  { name: 'Boyer-Moore', slug: 'boyer-moore', endpoint: 'boyer-moore', category: 'string', inputType: 'text', inputLabel: 'Text', defaultInput: 'HERE IS A SIMPLE EXAMPLE', needsPattern: true, patternLabel: 'Pattern', defaultPattern: 'EXAMPLE', description: 'Pattern matching that scans right to left using bad-character heuristic.' },
  { name: 'Rabin-Karp', slug: 'rabin-karp', endpoint: 'rabin-karp', category: 'string', inputType: 'text', inputLabel: 'Text', defaultInput: 'GEEKS FOR GEEKS', needsPattern: true, patternLabel: 'Pattern', defaultPattern: 'GEEK', description: 'Pattern matching using rolling hash for fast comparison.' },
  { name: 'Longest Common Subsequence', slug: 'lcs', endpoint: 'lcs', category: 'string', inputType: 'text', inputLabel: 'Text 1', defaultInput: 'ABCBDAB', needsPattern: true, patternLabel: 'Text 2', defaultPattern: 'BDCAB', description: 'Finds the longest subsequence common to two strings.' },
  { name: 'Longest Palindromic Substring', slug: 'longest-palindrome', endpoint: 'longest-palindrome', category: 'string', inputType: 'text', inputLabel: 'Text', defaultInput: 'babad', description: 'Finds the longest substring that reads the same forwards and backwards.' },
  { name: 'Anagram Detection', slug: 'anagram-detection', endpoint: 'anagram-detection', category: 'string', inputType: 'text', inputLabel: 'Text', defaultInput: 'cbaebabacd', needsPattern: true, patternLabel: 'Pattern', defaultPattern: 'abc', description: 'Finds all starting indices of anagrams of the pattern in the text.' },
  { name: 'String Reversal', slug: 'reversal', endpoint: 'reversal', category: 'string', inputType: 'text', inputLabel: 'Text', defaultInput: 'hello', description: 'Reverses a string in-place using two pointers.' },
  { name: 'Run-Length Encoding', slug: 'run-length-encoding', endpoint: 'run-length-encoding', category: 'string', inputType: 'text', inputLabel: 'Text', defaultInput: 'aaabbccdddd', description: 'Compresses a string by counting consecutive characters.' },
  { name: 'Levenshtein Distance', slug: 'levenshtein', endpoint: 'levenshtein', category: 'string', inputType: 'text', inputLabel: 'Text 1', defaultInput: 'kitten', needsPattern: true, patternLabel: 'Text 2', defaultPattern: 'sitting', description: 'Computes the minimum number of edits to transform one string into another.' },
];

export const treeConfig: AlgorithmConfig[] = [
  { name: 'Inorder Traversal', slug: 'inorder', endpoint: 'inorder', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8', description: 'Visits nodes in left-root-right order (produces sorted output for BST).' },
  { name: 'Preorder Traversal', slug: 'preorder', endpoint: 'preorder', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8', description: 'Visits nodes in root-left-right order.' },
  { name: 'Postorder Traversal', slug: 'postorder', endpoint: 'postorder', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8', description: 'Visits nodes in left-right-root order.' },
  { name: 'Level Order (BFS)', slug: 'level-order', endpoint: 'level-order', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8', description: 'Visits nodes level by level using a queue.' },
  { name: 'BST Insert / Search', slug: 'bst-insert-search', endpoint: 'bst-insert-search', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8', needsTarget: true, targetLabel: 'Search for', defaultTarget: 4, description: 'Inserts values into a BST, then searches for a target.' },
  { name: 'Tree Height', slug: 'height', endpoint: 'height', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8', description: 'Computes the height of a binary tree.' },
  { name: 'Lowest Common Ancestor', slug: 'lca', endpoint: 'lca', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8', needsTarget: true, targetLabel: 'Node A', defaultTarget: 1, needsPattern: true, patternLabel: 'Node B', defaultPattern: '4', description: 'Finds the lowest common ancestor of two nodes in a BST.' },
  { name: 'Invert Binary Tree', slug: 'invert', endpoint: 'invert', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8', description: 'Swaps left and right children of every node.' },
  { name: 'Validate BST', slug: 'validate-bst', endpoint: 'validate-bst', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8', description: 'Checks if the tree satisfies BST property using inorder traversal.' },
  { name: 'Diameter of Binary Tree', slug: 'diameter', endpoint: 'diameter', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8', description: 'Finds the longest path between any two nodes.' },
];

export const graphConfig: AlgorithmConfig[] = [
  { name: 'BFS', slug: 'bfs', endpoint: 'bfs', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to;...)', defaultInput: '6;0,1;0,2;1,3;2,3;3,4;4,5', needsTarget: true, targetLabel: 'Start node', defaultTarget: 0, description: 'Explores all neighbors at current depth before moving deeper.' },
  { name: 'DFS', slug: 'dfs', endpoint: 'dfs', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to;...)', defaultInput: '6;0,1;0,2;1,3;2,3;3,4;4,5', needsTarget: true, targetLabel: 'Start node', defaultTarget: 0, description: 'Explores as deep as possible along each branch before backtracking.' },
  { name: "Dijkstra's Shortest Path", slug: 'dijkstra', endpoint: 'dijkstra', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to,weight;...)', defaultInput: '5;0,1,4;0,2,1;1,3,1;2,1,2;2,3,5;3,4,3', needsTarget: true, targetLabel: 'Start node', defaultTarget: 0, description: 'Finds shortest paths from a source node using a greedy approach.' },
  { name: 'Bellman-Ford', slug: 'bellman-ford', endpoint: 'bellman-ford', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to,weight;...)', defaultInput: '5;0,1,6;0,2,7;1,2,8;1,3,5;1,4,-4;2,3,-3;2,4,9;3,1,-2;4,3,7', needsTarget: true, targetLabel: 'Start node', defaultTarget: 0, description: 'Handles negative edge weights, detects negative cycles.' },
  { name: 'Floyd-Warshall', slug: 'floyd-warshall', endpoint: 'floyd-warshall', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to,weight;...)', defaultInput: '4;0,1,3;0,3,7;1,0,8;1,2,2;2,0,5;2,3,1;3,0,2', description: 'Computes shortest paths between all pairs of nodes.' },
  { name: 'Topological Sort', slug: 'topological-sort', endpoint: 'topological-sort', category: 'graph', inputType: 'graph', inputLabel: 'DAG (nodes;from,to;...)', defaultInput: '6;5,2;5,0;4,0;4,1;2,3;3,1', description: 'Orders nodes such that every edge goes from earlier to later.' },
  { name: 'Cycle Detection', slug: 'cycle-detection', endpoint: 'cycle-detection', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to;...)', defaultInput: '4;0,1;1,2;2,3;3,1', description: 'Detects cycles in a directed graph using DFS coloring.' },
  { name: 'Union-Find', slug: 'union-find', endpoint: 'union-find', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to;...)', defaultInput: '5;0,1;1,2;3,4', description: 'Disjoint set data structure for tracking connected components.' },
  { name: "Kruskal's MST", slug: 'kruskal', endpoint: 'kruskal', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to,weight;...)', defaultInput: '4;0,1,10;0,2,6;0,3,5;1,3,15;2,3,4', description: 'Builds minimum spanning tree by adding cheapest edges first.' },
  { name: "Prim's MST", slug: 'prim', endpoint: 'prim', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to,weight;...)', defaultInput: '4;0,1,10;0,2,6;0,3,5;1,3,15;2,3,4', needsTarget: true, targetLabel: 'Start node', defaultTarget: 0, description: 'Builds MST by growing from a start node, always adding the cheapest edge.' },
];

export const dpConfig: AlgorithmConfig[] = [
  { name: 'Fibonacci', slug: 'fibonacci', endpoint: 'fibonacci', category: 'dp', inputType: 'number', inputLabel: 'n', defaultInput: '10', description: 'Computes Fibonacci numbers using dynamic programming (memoization intro).' },
  { name: '0/1 Knapsack', slug: 'knapsack', endpoint: 'knapsack', category: 'dp', inputLabel: 'Weights', defaultInput: '1,3,4,5', needsTarget: true, targetLabel: 'Capacity', defaultTarget: 7, needsPattern: true, patternLabel: 'Values', defaultPattern: '1,4,5,7', description: 'Maximizes value without exceeding weight capacity.' },
  { name: 'Longest Common Subsequence', slug: 'lcs', endpoint: 'lcs', category: 'dp', inputType: 'text', inputLabel: 'Text 1', defaultInput: 'ABCBDAB', needsPattern: true, patternLabel: 'Text 2', defaultPattern: 'BDCAB', description: 'Finds the longest subsequence common to two strings using DP.' },
  { name: 'Longest Increasing Subsequence', slug: 'lis', endpoint: 'lis', category: 'dp', inputLabel: 'Array', defaultInput: '10,22,9,33,21,50,41,60', description: 'Finds the length of the longest strictly increasing subsequence.' },
  { name: 'Coin Change', slug: 'coin-change', endpoint: 'coin-change', category: 'dp', inputLabel: 'Coins', defaultInput: '1,5,10,25', needsTarget: true, targetLabel: 'Amount', defaultTarget: 30, description: 'Finds the minimum number of coins to make a given amount.' },
  { name: 'Matrix Chain Multiplication', slug: 'matrix-chain', endpoint: 'matrix-chain', category: 'dp', inputLabel: 'Dimensions', defaultInput: '10,20,30,40,30', description: 'Finds optimal parenthesization to minimize scalar multiplications.' },
  { name: 'Edit Distance', slug: 'edit-distance', endpoint: 'edit-distance', category: 'dp', inputType: 'text', inputLabel: 'Text 1', defaultInput: 'kitten', needsPattern: true, patternLabel: 'Text 2', defaultPattern: 'sitting', description: 'Minimum edits (insert, delete, replace) to transform one string into another.' },
  { name: 'Rod Cutting', slug: 'rod-cutting', endpoint: 'rod-cutting', category: 'dp', inputLabel: 'Prices', defaultInput: '1,5,8,9,10,17,17,20', description: 'Maximizes revenue by cutting a rod into pieces with given prices.' },
  { name: 'Subset Sum', slug: 'subset-sum', endpoint: 'subset-sum', category: 'dp', inputLabel: 'Set', defaultInput: '3,34,4,12,5,2', needsTarget: true, targetLabel: 'Target', defaultTarget: 9, description: 'Determines if a subset sums to a given target.' },
  { name: 'Climbing Stairs', slug: 'climbing-stairs', endpoint: 'climbing-stairs', category: 'dp', inputType: 'number', inputLabel: 'Stairs (n)', defaultInput: '8', description: 'Counts the number of ways to climb n stairs (1 or 2 steps at a time).' },
];

export const backtrackingConfig: AlgorithmConfig[] = [
  { name: 'N-Queens', slug: 'n-queens', endpoint: 'n-queens', category: 'backtracking', inputType: 'number', inputLabel: 'Board size (n)', defaultInput: '4', description: 'Places n queens on an n×n board so no two attack each other.' },
  { name: 'Sudoku Solver', slug: 'sudoku', endpoint: 'sudoku', category: 'backtracking', inputLabel: 'Grid (81 values, 0=empty)', defaultInput: '5,3,0,0,7,0,0,0,0,6,0,0,1,9,5,0,0,0,0,9,8,0,0,0,0,6,0,8,0,0,0,6,0,0,0,3,4,0,0,8,0,3,0,0,1,7,0,0,0,2,0,0,0,6,0,6,0,0,0,0,2,8,0,0,0,0,4,1,9,0,0,5,0,0,0,0,8,0,0,7,9', description: 'Solves a 9×9 Sudoku puzzle via backtracking.' },
  { name: 'Generate Permutations', slug: 'permutations', endpoint: 'permutations', category: 'backtracking', inputLabel: 'Array', defaultInput: '1,2,3', description: 'Generates all permutations of the input array.' },
  { name: 'Generate Subsets', slug: 'subsets', endpoint: 'subsets', category: 'backtracking', inputLabel: 'Array', defaultInput: '1,2,3', description: 'Generates all subsets (power set) of the input array.' },
  { name: 'Rat in a Maze', slug: 'rat-in-maze', endpoint: 'rat-in-maze', category: 'backtracking', inputLabel: 'Grid (1=open, 0=blocked)', defaultInput: '1,0,0,0,1,1,0,0,0,1,1,0,0,0,0,1', needsTarget: true, targetLabel: 'Grid size (n)', defaultTarget: 4, description: 'Finds a path from top-left to bottom-right through a maze.' },
  { name: "Knight's Tour", slug: 'knights-tour', endpoint: 'knights-tour', category: 'backtracking', inputType: 'number', inputLabel: 'Board size (n)', defaultInput: '5', description: 'Visits every square on an n×n board with a knight exactly once.' },
  { name: 'Word Search in Grid', slug: 'word-search', endpoint: 'word-search', category: 'backtracking', inputType: 'text', inputLabel: 'Grid (rows separated by ;)', defaultInput: 'ABCE;SFCS;ADEE', needsPattern: true, patternLabel: 'Word', defaultPattern: 'ABCCED', description: 'Searches for a word in a grid by moving in 4 directions.' },
  { name: 'Combination Sum', slug: 'combination-sum', endpoint: 'combination-sum', category: 'backtracking', inputLabel: 'Candidates', defaultInput: '2,3,6,7', needsTarget: true, targetLabel: 'Target', defaultTarget: 7, description: 'Finds all combinations of candidates that sum to a target.' },
  { name: 'Palindrome Partitioning', slug: 'palindrome-partitioning', endpoint: 'palindrome-partitioning', category: 'backtracking', inputType: 'text', inputLabel: 'Text', defaultInput: 'aab', description: 'Partitions a string so every part is a palindrome.' },
  { name: 'Graph Coloring', slug: 'graph-coloring', endpoint: 'graph-coloring', category: 'backtracking', inputType: 'graph', inputLabel: 'Graph (nodes;from,to;...)', defaultInput: '4;0,1;0,2;1,2;1,3;2,3', needsTarget: true, targetLabel: 'Colors', defaultTarget: 3, description: 'Assigns colors to graph nodes so no adjacent nodes share a color.' },
];

export const nrTheoryConfig: AlgorithmConfig[] = [
  { name: 'Sieve of Eratosthenes', slug: 'sieve', endpoint: 'sieve', category: 'number-theory', inputType: 'number', inputLabel: 'Upper bound (n)', defaultInput: '30', description: 'Finds all prime numbers up to n by iteratively marking composites.' },
  { name: 'Euclidean Algorithm (GCD)', slug: 'gcd', endpoint: 'gcd', category: 'number-theory', inputType: 'numbers', inputLabel: 'a, b', defaultInput: '48,18', description: 'Computes the greatest common divisor using repeated division.' },
  { name: 'Extended Euclidean', slug: 'extended-gcd', endpoint: 'extended-gcd', category: 'number-theory', inputType: 'numbers', inputLabel: 'a, b', defaultInput: '35,15', description: 'Finds x, y such that ax + by = gcd(a, b).' },
  { name: 'Fast Exponentiation', slug: 'fast-exponentiation', endpoint: 'fast-exp', category: 'number-theory', inputType: 'numbers', inputLabel: 'base, exponent, modulus', defaultInput: '2,10,1000', description: 'Computes base^exp mod m efficiently using binary exponentiation.' },
  { name: 'Modular Arithmetic', slug: 'modular-arithmetic', endpoint: 'modular', category: 'number-theory', inputType: 'numbers', inputLabel: 'a, b, mod', defaultInput: '7,3,5', description: 'Demonstrates modular addition, subtraction, and multiplication.' },
  { name: 'Prime Factorization', slug: 'prime-factorization', endpoint: 'prime-factorization', category: 'number-theory', inputType: 'number', inputLabel: 'n', defaultInput: '84', description: 'Decomposes a number into its prime factors.' },
  { name: 'Fibonacci (Matrix)', slug: 'fibonacci-matrix', endpoint: 'fibonacci-matrix', category: 'number-theory', inputType: 'number', inputLabel: 'n', defaultInput: '10', description: 'Computes Fibonacci numbers using matrix exponentiation in O(log n).' },
  { name: 'Chinese Remainder Theorem', slug: 'chinese-remainder', endpoint: 'chinese-remainder', category: 'number-theory', inputLabel: 'Remainders', defaultInput: '2,3,2', needsPattern: true, patternLabel: 'Moduli', defaultPattern: '3,5,7', description: 'Solves a system of simultaneous congruences.' },
  { name: 'Bit Manipulation', slug: 'bit-manipulation', endpoint: 'bit-manipulation', category: 'number-theory', inputType: 'number', inputLabel: 'n', defaultInput: '42', description: 'Demonstrates AND, OR, XOR tricks and bit counting.' },
  { name: 'Catalan Numbers', slug: 'catalan', endpoint: 'catalan', category: 'number-theory', inputType: 'number', inputLabel: 'n', defaultInput: '8', description: 'Computes Catalan numbers C(0)..C(n) using DP.' },
];

export const allConfigs: Record<string, AlgorithmConfig[]> = {
  array: arrayConfig,
  string: stringConfig,
  tree: treeConfig,
  graph: graphConfig,
  dp: dpConfig,
  backtracking: backtrackingConfig,
  'number-theory': nrTheoryConfig,
};
