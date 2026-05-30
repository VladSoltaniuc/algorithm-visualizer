export const algorithmPseudocode: Record<string, string> = {
  "bubble-sort":
`void BubbleSort(int[] arr, int n) {
  for (int i = 0; i < n-1; i++) {
    bool swapped = false;
    for (int j = 0; j < n-i-1; j++)
      if (arr[j] > arr[j+1]) {
        (arr[j], arr[j+1]) = (arr[j+1], arr[j]);
        swapped = true;
      }
    if (!swapped) break;
  }
}`,

  "quick-sort":
`void Quicksort(int[] a, int lo, int hi) {
  if (lo >= hi) return;
  int p = Partition(a, lo, hi);
  Quicksort(a, lo, p-1);
  Quicksort(a, p+1, hi);
}
int Partition(int[] a, int lo, int hi) {
  int pivot = a[hi], i = lo-1;
  for (int j = lo; j < hi; j++)
    if (a[j] <= pivot) Swap(a, ++i, j);
  Swap(a, i+1, hi); return i+1;
}`,

  "merge-sort":
`int[] MergeSort(int[] a) {
  if (a.Length <= 1) return a;
  int mid = a.Length / 2;
  var L = MergeSort(a[..mid]);
  var R = MergeSort(a[mid..]);
  return Merge(L, R);
}
int[] Merge(int[] L, int[] R) {
  var res = new List<int>();
  int i = 0, j = 0;
  while (i < L.Length && j < R.Length)
    res.Add(L[i] <= R[j] ? L[i++] : R[j++]);
  while (i < L.Length) res.Add(L[i++]);
  while (j < R.Length) res.Add(R[j++]);
  return [..res];
}`,

  "insertion-sort":
`void InsertionSort(int[] arr, int n) {
  for (int i = 1; i < n; i++) {
    int key = arr[i], j = i-1;
    while (j >= 0 && arr[j] > key)
      arr[j+1] = arr[j--];
    arr[j+1] = key;
  }
}`,

  "selection-sort":
`void SelectionSort(int[] arr, int n) {
  for (int i = 0; i < n-1; i++) {
    int min = i;
    for (int j = i+1; j < n; j++)
      if (arr[j] < arr[min]) min = j;
    (arr[i], arr[min]) = (arr[min], arr[i]);
  }
}`,

  "binary-search":
`int BinarySearch(int[] arr, int n, int target) {
  int lo = 0, hi = n-1;
  while (lo <= hi) {
    int mid = (lo + hi) / 2;
    if (arr[mid] == target) return mid;
    if (arr[mid] < target)  lo = mid+1;
    else                    hi = mid-1;
  }
  return -1;
}`,

  "two-pointers":
`List<(int,int)> TwoPointers(int[] arr, int n, int target) {
  var result = new List<(int,int)>();
  int l = 0, r = n-1;
  while (l < r) {
    int sum = arr[l] + arr[r];
    if (sum == target) { result.Add((l,r)); l++; r--; }
    else if (sum < target) l++;
    else r--;
  }
  return result;
}`,

  "sliding-window":
`int SlidingWindow(int[] arr, int n, int k) {
  int window = arr[..k].Sum(), max = window;
  for (int i = k; i < n; i++) {
    window += arr[i] - arr[i-k];
    max = Math.Max(max, window);
  }
  return max;
}`,

  "kadane":
`int Kadane(int[] arr, int n) {
  int here = arr[0], best = arr[0];
  for (int i = 1; i < n; i++) {
    here = Math.Max(arr[i], here + arr[i]);
    best = Math.Max(best, here);
  }
  return best;
}`,

  "bfs":
`void Bfs(List<int>[] graph, int start) {
  var q = new Queue<int>(); q.Enqueue(start);
  var seen = new HashSet<int> { start };
  while (q.Count > 0) {
    int node = q.Dequeue();
    foreach (int nbr in graph[node])
      if (seen.Add(nbr)) q.Enqueue(nbr);
  }
}`,

  "dfs":
`void Dfs(int node, HashSet<int> seen) {
  seen.Add(node);
  Process(node);
  foreach (int nbr in graph[node])
    if (!seen.Contains(nbr))
      Dfs(nbr, seen);
}`,

  "dijkstra":
`int[] Dijkstra(List<(int v, int w)>[] graph, int n, int src) {
  var dist = new int[n]; Array.Fill(dist, int.MaxValue);
  dist[src] = 0;
  var pq = new PriorityQueue<int,int>(); pq.Enqueue(src, 0);
  while (pq.Count > 0) {
    int u = pq.Dequeue();
    foreach ((int v, int w) in graph[u])
      if (dist[u]+w < dist[v]) {
        dist[v] = dist[u]+w; pq.Enqueue(v, dist[v]);
      }
  }
  return dist;
}`,

  "topological-sort":
`List<int> TopologicalSort(List<int>[] graph, int n) {
  int[] inDeg = new int[n];
  foreach (var nbrs in graph)
    foreach (int v in nbrs) inDeg[v]++;
  var q = new Queue<int>(
    Enumerable.Range(0,n).Where(v => inDeg[v]==0));
  var order = new List<int>();
  while (q.Count > 0) {
    int u = q.Dequeue(); order.Add(u);
    foreach (int v in graph[u])
      if (--inDeg[v] == 0) q.Enqueue(v);
  }
  return order;
}`,

  "cycle-detection":
`bool HasCycle(List<int>[] graph, int n) {
  int[] color = new int[n]; // 0 WHITE 1 GRAY 2 BLACK
  bool Dfs(int v) {
    color[v] = 1;
    foreach (int nbr in graph[v]) {
      if (color[nbr] == 1) return true;
      if (color[nbr] == 0 && Dfs(nbr)) return true;
    }
    color[v] = 2; return false;
  }
  for (int i = 0; i < n; i++)
    if (color[i] == 0 && Dfs(i)) return true;
  return false;
}`,

  "kruskal":
`List<(int u,int v,int w)> Kruskal(
    List<(int u,int v,int w)> edges, int n) {
  int[] par = Enumerable.Range(0, n).ToArray();
  int Find(int x) =>
    par[x] == x ? x : par[x] = Find(par[x]);
  void Union(int a, int b) =>
    par[Find(a)] = Find(b);
  edges.Sort((a, b) => a.w - b.w);
  var mst = new List<(int u,int v,int w)>();
  foreach (var (u, v, w) in edges)
    if (Find(u) != Find(v)) {
      Union(u, v); mst.Add((u, v, w));
    }
  return mst;
}`,

  "prim":
`int[] Prim(List<(int v,int w)>[] graph, int n, int src) {
  int[] key = new int[n]; Array.Fill(key, int.MaxValue);
  bool[] inMst = new bool[n]; key[src] = 0;
  for (int _ = 0; _ < n; _++) {
    int u = -1;
    for (int v = 0; v < n; v++)
      if (!inMst[v] && (u==-1 || key[v] < key[u])) u = v;
    inMst[u] = true;
    foreach ((int v, int w) in graph[u])
      if (!inMst[v] && w < key[v]) key[v] = w;
  }
  return key; // key[v] = MST edge weight connecting v
}`,

  "bst-insert-search":
`Node Insert(Node root, int val) {
  if (root == null) return new Node(val);
  if (val < root.val) root.left  = Insert(root.left,  val);
  else                root.right = Insert(root.right, val);
  return root;
}
Node Search(Node root, int val) {
  if (root == null || root.val == val) return root;
  return val < root.val
    ? Search(root.left,  val)
    : Search(root.right, val);
}`,

  "inorder":
`void Inorder(Node node) {
  if (node == null) return;
  Inorder(node.left);
  Visit(node);
  Inorder(node.right);
}`,

  "preorder":
`void Preorder(Node node) {
  if (node == null) return;
  Visit(node);
  Preorder(node.left);
  Preorder(node.right);
}`,

  "postorder":
`void Postorder(Node node) {
  if (node == null) return;
  Postorder(node.left);
  Postorder(node.right);
  Visit(node);
}`,

  "level-order":
`List<int> LevelOrder(Node root) {
  var result = new List<int>();
  var q = new Queue<Node>(); q.Enqueue(root);
  while (q.Count > 0) {
    Node node = q.Dequeue();
    result.Add(node.val);
    if (node.left  != null) q.Enqueue(node.left);
    if (node.right != null) q.Enqueue(node.right);
  }
  return result;
}`,

  "lca":
`Node Lca(Node root, int p, int q) {
  if (root == null) return null;
  if (root.val > p && root.val > q)
    return Lca(root.left, p, q);
  if (root.val < p && root.val < q)
    return Lca(root.right, p, q);
  return root; // split point: root is the LCA
}`,

  "diameter":
`int Diameter(Node root) {
  int best = 0;
  int Dfs(Node node) {
    if (node == null) return 0;
    int L = Dfs(node.left), R = Dfs(node.right);
    best = Math.Max(best, L + R);
    return 1 + Math.Max(L, R);
  }
  Dfs(root);
  return best;
}`,

  "validate-bst":
`bool IsValid(Node node,
             long lo = long.MinValue,
             long hi = long.MaxValue) {
  if (node == null) return true;
  if (node.val <= lo || node.val >= hi) return false;
  return IsValid(node.left,  lo,       node.val)
      && IsValid(node.right, node.val, hi);
}`,

  "invert":
`Node Invert(Node node) {
  if (node == null) return null;
  (node.left, node.right) = (
    Invert(node.right),
    Invert(node.left)
  );
  return node;
}`,

  "huffman":
`Node BuildHuffman(Dictionary<char,int> freqs) {
  var pq = new PriorityQueue<Node,int>();
  foreach (var (ch, freq) in freqs)
    pq.Enqueue(new Node(ch), freq);
  while (pq.Count > 1) {
    pq.TryDequeue(out var l, out int lf);
    pq.TryDequeue(out var r, out int rf);
    pq.Enqueue(new Node(l, r), lf + rf);
  }
  return pq.Peek();
}`,

  "fibonacci":
`int Fibonacci(int n) {
  int[] dp = new int[n+1];
  dp[0] = 0; dp[1] = 1;
  for (int i = 2; i <= n; i++)
    dp[i] = dp[i-1] + dp[i-2];
  return dp[n];
}`,

  "knapsack":
`int Knapsack(int[] weights, int[] values, int n, int W) {
  int[,] dp = new int[n+1, W+1];
  for (int i = 1; i <= n; i++)
    for (int w = 0; w <= W; w++) {
      dp[i,w] = dp[i-1,w];
      if (weights[i-1] <= w)
        dp[i,w] = Math.Max(dp[i,w],
          dp[i-1, w-weights[i-1]] + values[i-1]);
    }
  return dp[n,W];
}`,

  "lcs":
`int Lcs(string s1, string s2, int n, int m) {
  int[,] dp = new int[n+1, m+1];
  for (int i = 1; i <= n; i++)
    for (int j = 1; j <= m; j++)
      dp[i,j] = s1[i-1] == s2[j-1]
        ? dp[i-1,j-1] + 1
        : Math.Max(dp[i-1,j], dp[i,j-1]);
  return dp[n,m];
}`,

  "lis":
`int Lis(int[] arr, int n) {
  int[] dp = new int[n]; Array.Fill(dp, 1);
  for (int i = 1; i < n; i++)
    for (int j = 0; j < i; j++)
      if (arr[j] < arr[i])
        dp[i] = Math.Max(dp[i], dp[j]+1);
  return dp.Max();
}`,

  "coin-change":
`int CoinChange(int[] coins, int amount) {
  int[] dp = new int[amount+1];
  Array.Fill(dp, int.MaxValue); dp[0] = 0;
  foreach (int c in coins)
    for (int i = c; i <= amount; i++)
      if (dp[i-c] != int.MaxValue)
        dp[i] = Math.Min(dp[i], dp[i-c]+1);
  return dp[amount] == int.MaxValue ? -1 : dp[amount];
}`,

  "climbing-stairs":
`int ClimbingStairs(int n) {
  int[] dp = new int[n+1];
  dp[0] = 1; dp[1] = 1;
  for (int i = 2; i <= n; i++)
    dp[i] = dp[i-1] + dp[i-2];
  return dp[n];
}`,

  "permutations":
`List<List<int>> Permutations(int[] nums, int n) {
  var results = new List<List<int>>();
  void Backtrack(List<int> path, bool[] used) {
    if (path.Count == n) { results.Add([..path]); return; }
    for (int i = 0; i < n; i++) {
      if (used[i]) continue;
      used[i] = true; path.Add(nums[i]);
      Backtrack(path, used);
      used[i] = false; path.RemoveAt(path.Count-1);
    }
  }
  Backtrack(new List<int>(), new bool[n]);
  return results;
}`,

  "subsets":
`List<List<int>> Subsets(int[] nums, int n) {
  var results = new List<List<int>>();
  void Backtrack(int start, List<int> path) {
    results.Add([..path]);
    for (int i = start; i < n; i++) {
      path.Add(nums[i]);
      Backtrack(i+1, path);
      path.RemoveAt(path.Count-1);
    }
  }
  Backtrack(0, new List<int>());
  return results;
}`,

  "combination-sum":
`List<List<int>> CombinationSum(int[] candidates, int target) {
  int n = candidates.Length;
  Array.Sort(candidates); // required for the break to work
  var results = new List<List<int>>();
  void Backtrack(int start, List<int> path, int rem) {
    if (rem == 0) { results.Add([..path]); return; }
    for (int i = start; i < n; i++) {
      if (candidates[i] > rem) break;
      path.Add(candidates[i]);
      Backtrack(i, path, rem - candidates[i]);
      path.RemoveAt(path.Count-1);
    }
  }
  Backtrack(0, new List<int>(), target);
  return results;
}`,

  "palindrome-partitioning":
`List<List<string>> PalindromePartition(string s) {
  var results = new List<List<string>>();
  bool IsPalindrome(string t) {
    int l = 0, r = t.Length-1;
    while (l < r) if (t[l++] != t[r--]) return false;
    return true;
  }
  void Backtrack(int start, List<string> path) {
    if (start == s.Length) { results.Add([..path]); return; }
    for (int end = start+1; end <= s.Length; end++) {
      string sub = s[start..end];
      if (IsPalindrome(sub)) {
        path.Add(sub); Backtrack(end, path);
        path.RemoveAt(path.Count-1);
      }
    }
  }
  Backtrack(0, new List<string>());
  return results;
}`,

  "bit-manipulation":
`void BitTricks(int n, int k) {
  bool check = (n & (1 << k)) != 0;  // is bit k set?
  int  set    =  n | (1 << k);        // set bit k
  int  clear  =  n & ~(1 << k);       // clear bit k
  int  toggle =  n ^ (1 << k);        // toggle bit k
  int  count  =  BitOperations.PopCount((uint)n); // popcount
  bool pow2   = (n & (n-1)) == 0;     // is n power of 2?
}`,

  "linear-search":
`List<int> NaiveSearch(string text, string pattern, int n, int m) {
  var matches = new List<int>();
  for (int i = 0; i <= n-m; i++)
    if (text[i..(i+m)] == pattern)
      matches.Add(i);
  return matches;
}`,

  "kmp":
`List<int> Kmp(string text, string pattern, int n, int m) {
  var matches = new List<int>();
  // Build lps[] (failure / prefix) table
  int[] lps = new int[m];
  for (int len = 0, k = 1; k < m; ) {
    if (pattern[k] == pattern[len]) lps[k++] = ++len;
    else if (len > 0) len = lps[len-1];
    else lps[k++] = 0;
  }
  // Search
  int i = 0, j = 0;
  while (i < n) {
    if (text[i] == pattern[j]) { i++; j++; }
    if (j == m) { matches.Add(i-j); j = lps[j-1]; }
    else if (i < n && text[i] != pattern[j]) {
      if (j > 0) j = lps[j-1]; else i++;
    }
  }
  return matches;
}`,

  "boyer-moore":
`List<int> BoyerMoore(string text, string pattern, int n, int m) {
  var matches = new List<int>();
  // Bad-character table
  int[] bad = new int[256];
  Array.Fill(bad, -1);
  for (int i = 0; i < m; i++) bad[pattern[i]] = i;
  // Good-suffix table
  int[] gs = new int[m+1]; Array.Fill(gs, m);
  int[] border = new int[m+1]; border[m] = m+1;
  for (int i = m-1, j = m+1; i >= 0; i--, j--) {
    while (j <= m && pattern[i] != pattern[j-1])
      { if (gs[j] == m) gs[j] = j-i; j = border[j]; }
    border[i] = j;
  }
  for (int i = 0; i <= m; i++)
    if (gs[border[i]] == m) gs[border[i]] = i;
  // Search
  int s = 0;
  while (s <= n-m) {
    int j = m-1;
    while (j >= 0 && pattern[j] == text[s+j]) j--;
    if (j < 0) { matches.Add(s); s += gs[0]; }
    else s += Math.Max(gs[j+1], j - bad[text[s+j]]);
  }
  return matches;
}`,

  "rabin-karp":
`List<int> RabinKarp(string text, string pattern, int n, int m) {
  var matches = new List<int>();
  const int B = 256, MOD = 101;
  int h = 1;
  for (int i = 0; i < m-1; i++) h = h*B % MOD;
  int ph = 0, wh = 0;
  for (int i = 0; i < m; i++) {
    ph = (B*ph + pattern[i]) % MOD;
    wh = (B*wh + text[i])    % MOD;
  }
  for (int i = 0; i <= n-m; i++) {
    if (ph == wh && text[i..(i+m)] == pattern)
      matches.Add(i);
    if (i < n-m) {
      wh = (B*(wh - text[i]*h) + text[i+m]) % MOD;
      if (wh < 0) wh += MOD;
    }
  }
  return matches;
}`,

  "longest-palindrome":
`string LongestPalindrome(string s) {
  string t = "^#" + string.Join("#", s.ToCharArray()) + "#$";
  int n = t.Length;
  int[] p = new int[n];
  int c = 0, r = 0;
  for (int i = 1; i < n-1; i++) {
    int mirror = 2*c - i;
    if (i < r) p[i] = Math.Min(r-i, p[mirror]);
    while (t[i+p[i]+1] == t[i-p[i]-1]) p[i]++;
    if (i+p[i] > r) { c = i; r = i+p[i]; }
  }
  int maxLen = 0, centerIdx = 0;
  for (int i = 1; i < n-1; i++)
    if (p[i] > maxLen) { maxLen = p[i]; centerIdx = i; }
  int start = (centerIdx - maxLen) / 2;
  return s.Substring(start, maxLen);
}`,

  "anagram-detection":
`List<int> AnagramDetection(string text, string pattern) {
  var result = new List<int>();
  int[] pCount = new int[256], wCount = new int[256];
  foreach (char c in pattern) pCount[c]++;
  for (int i = 0; i < text.Length; i++) {
    wCount[text[i]]++;
    if (i >= pattern.Length) wCount[text[i - pattern.Length]]--;
    if (i >= pattern.Length-1 && pCount.SequenceEqual(wCount))
      result.Add(i - pattern.Length + 1);
  }
  return result;
}`,

  "reversal":
`string Reverse(string s) {
  char[] arr = s.ToCharArray();
  for (int l = 0, r = arr.Length-1; l < r; l++, r--)
    (arr[l], arr[r]) = (arr[r], arr[l]);
  return new string(arr);
}`,
};

// To add a shortcut: find the slug and replace its ##SHORTCUT_PLACEHOLDER## value
export const algorithmShortcuts: Record<string, string> = {
  "bubble-sort":            "(FFS) For For Swap",
  "quick-sort":             "(PQQ) Partition Quick Quick",
  "merge-sort":             "(M4) Mid MergeSort MergeSort Merge",
  "insertion-sort":         "##WORK_IN_PROGRESS##",
  "selection-sort":         "##WORK_IN_PROGRESS##",
  "binary-search":          "(WMII) While Mid If If",
  "two-pointers":           "##WORK_IN_PROGRESS##",
  "sliding-window":         "##WORK_IN_PROGRESS##",
  "kadane":                 "##WORK_IN_PROGRESS##",
  "bfs":                    "##WORK_IN_PROGRESS##",
  "dfs":                    "##WORK_IN_PROGRESS##",
  "dijkstra":               "##WORK_IN_PROGRESS##",
  "topological-sort":       "##WORK_IN_PROGRESS##",
  "cycle-detection":        "##WORK_IN_PROGRESS##",
  "kruskal":                "WILL_NOT_IMPLEMENT",
  "prim":                   "WILL_NOT_IMPLEMENT",
  "bst-insert-search":      "##WORK_IN_PROGRESS##",
  "inorder":                "##WORK_IN_PROGRESS##",
  "preorder":               "WILL_NOT_IMPLEMENT",
  "postorder":              "WILL_NOT_IMPLEMENT",
  "level-order":            "##WORK_IN_PROGRESS##",
  "lca":                    "##WORK_IN_PROGRESS##",
  "diameter":               "##WORK_IN_PROGRESS##",
  "validate-bst":           "##WORK_IN_PROGRESS##",
  "invert":                 "##WORK_IN_PROGRESS##",
  "huffman":                "##WORK_IN_PROGRESS##",
  "fibonacci":              "##WORK_IN_PROGRESS##",
  "knapsack":               "##WORK_IN_PROGRESS##",
  "lcs":                    "##WORK_IN_PROGRESS##",
  "lis":                    "##WORK_IN_PROGRESS##",
  "coin-change":            "##WORK_IN_PROGRESS##",
  "climbing-stairs":        "WILL_NOT_IMPLEMENT",
  "permutations":           "##WORK_IN_PROGRESS##",
  "subsets":                "WILL_NOT_IMPLEMENT",
  "combination-sum":        "##WORK_IN_PROGRESS##",
  "palindrome-partitioning":"WILL_NOT_IMPLEMENT",
  "bit-manipulation":       "##WORK_IN_PROGRESS##",
  "linear-search":          "##WORK_IN_PROGRESS##",
  "kmp":                    "##WORK_IN_PROGRESS##",
  "boyer-moore":            "##WORK_IN_PROGRESS##",
  "rabin-karp":             "##WORK_IN_PROGRESS##",
  "longest-palindrome":     "##WORK_IN_PROGRESS##",
  "anagram-detection":      "##WORK_IN_PROGRESS##",
  "reversal":               "(F) For",
};
