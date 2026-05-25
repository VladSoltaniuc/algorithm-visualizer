export const algorithmPseudocode: Record<string, string> = {
  "bubble-sort":
`for (int i = 0; i < n-1; i++) {
  bool swapped = false;
  for (int j = 0; j < n-i-1; j++)
    if (arr[j] > arr[j+1]) {
      (arr[j], arr[j+1]) = (arr[j+1], arr[j]);
      swapped = true;
    }
  if (!swapped) break;
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
}`,

  "insertion-sort":
`for (int i = 1; i < n; i++) {
  int key = arr[i], j = i-1;
  while (j >= 0 && arr[j] > key)
    arr[j+1] = arr[j--];
  arr[j+1] = key;
}`,

  "selection-sort":
`for (int i = 0; i < n-1; i++) {
  int min = i;
  for (int j = i+1; j < n; j++)
    if (arr[j] < arr[min]) min = j;
  (arr[i], arr[min]) = (arr[min], arr[i]);
}`,

  "binary-search":
`int lo = 0, hi = n-1;
while (lo <= hi) {
  int mid = (lo + hi) / 2;
  if (arr[mid] == target) return mid;
  if (arr[mid] < target)  lo = mid+1;
  else                    hi = mid-1;
}
return -1;`,

  "two-pointers":
`int l = 0, r = n-1;
while (l < r) {
  int sum = arr[l] + arr[r];
  if (sum == target) { Record(l, r); l++; r--; }
  else if (sum < target) l++;
  else r--;
}`,

  "sliding-window":
`int window = arr[..k].Sum(), max = window;
for (int i = k; i < n; i++) {
  window += arr[i] - arr[i-k];
  max = Math.Max(max, window);
}
return max;`,

  "kadane":
`int here = arr[0], best = arr[0];
for (int i = 1; i < n; i++) {
  here = Math.Max(arr[i], here + arr[i]);
  best = Math.Max(best, here);
}
return best;`,

  "bfs":
`var q = new Queue<int>(); q.Enqueue(start);
var seen = new HashSet<int> { start };
while (q.Count > 0) {
  int node = q.Dequeue();
  foreach (int nbr in graph[node])
    if (seen.Add(nbr)) q.Enqueue(nbr);
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
`var dist = new int[n]; Array.Fill(dist, int.MaxValue);
dist[src] = 0;
var pq = new PriorityQueue<int,int>(); pq.Enqueue(src, 0);
while (pq.Count > 0) {
  int u = pq.Dequeue();
  foreach ((int v, int w) in graph[u])
    if (dist[u]+w < dist[v]) {
      dist[v] = dist[u]+w; pq.Enqueue(v, dist[v]);
    }
}`,

  "topological-sort":
`int[] inDeg = CountIncoming();
var q = new Queue<int>(
  Enumerable.Range(0,n).Where(v => inDeg[v]==0));
var order = new List<int>();
while (q.Count > 0) {
  int u = q.Dequeue(); order.Add(u);
  foreach (int v in graph[u])
    if (--inDeg[v] == 0) q.Enqueue(v);
}`,

  "cycle-detection":
`int[] color = new int[n]; // 0 WHITE 1 GRAY 2 BLACK
bool Dfs(int v) {
  color[v] = 1;
  foreach (int nbr in graph[v]) {
    if (color[nbr] == 1) return true;
    if (color[nbr] == 0 && Dfs(nbr)) return true;
  }
  color[v] = 2; return false;
}`,

  "kruskal":
`edges.Sort((a,b) => a.W - b.W);
var uf = new UnionFind(n);
var mst = new List<Edge>();
foreach (var (u,v,w) in edges)
  if (uf.Find(u) != uf.Find(v)) {
    uf.Union(u,v); mst.Add((u,v,w));
  }`,

  "prim":
`int[] key = new int[n]; Array.Fill(key, int.MaxValue);
bool[] inMst = new bool[n]; key[src] = 0;
for (int _ = 0; _ < n; _++) {
  int u = MinKey(key, inMst); inMst[u] = true;
  foreach ((int v, int w) in graph[u])
    if (!inMst[v] && w < key[v]) key[v] = w;
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
`var q = new Queue<Node>(); q.Enqueue(root);
while (q.Count > 0) {
  Node node = q.Dequeue();
  Visit(node);
  if (node.left  != null) q.Enqueue(node.left);
  if (node.right != null) q.Enqueue(node.right);
}`,

  "lca":
`Node Lca(Node root, Node p, Node q) {
  if (root == null || root == p || root == q)
    return root;
  Node left  = Lca(root.left,  p, q);
  Node right = Lca(root.right, p, q);
  if (left != null && right != null) return root;
  return left ?? right;
}`,

  "diameter":
`int best = 0;
int Dfs(Node node) {
  if (node == null) return 0;
  int L = Dfs(node.left), R = Dfs(node.right);
  best = Math.Max(best, L + R);
  return 1 + Math.Max(L, R);
}
Dfs(root); return best;`,

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
`var pq = new PriorityQueue<Node,int>();
foreach (var (ch, freq) in freqs)
  pq.Enqueue(new Node(ch), freq);
while (pq.Count > 1) {
  pq.TryDequeue(out var l, out int lf);
  pq.TryDequeue(out var r, out int rf);
  pq.Enqueue(new Node(l, r), lf + rf);
}
Node root = pq.Peek();`,

  "fibonacci":
`int[] dp = new int[n+1];
dp[0] = 0; dp[1] = 1;
for (int i = 2; i <= n; i++)
  dp[i] = dp[i-1] + dp[i-2];
return dp[n];`,

  "knapsack":
`int[,] dp = new int[n+1, W+1];
for (int i = 1; i <= n; i++)
  for (int w = 0; w <= W; w++) {
    dp[i,w] = dp[i-1,w];
    if (weights[i-1] <= w)
      dp[i,w] = Math.Max(dp[i,w],
        dp[i-1, w-weights[i-1]] + values[i-1]);
  }`,

  "lcs":
`int[,] dp = new int[n+1, m+1];
for (int i = 1; i <= n; i++)
  for (int j = 1; j <= m; j++)
    dp[i,j] = s1[i-1] == s2[j-1]
      ? dp[i-1,j-1] + 1
      : Math.Max(dp[i-1,j], dp[i,j-1]);`,

  "lis":
`int[] dp = new int[n]; Array.Fill(dp, 1);
for (int i = 1; i < n; i++)
  for (int j = 0; j < i; j++)
    if (arr[j] < arr[i])
      dp[i] = Math.Max(dp[i], dp[j]+1);
return dp.Max();`,

  "coin-change":
`int[] dp = new int[amount+1];
Array.Fill(dp, int.MaxValue); dp[0] = 0;
foreach (int c in coins)
  for (int i = c; i <= amount; i++)
    if (dp[i-c] != int.MaxValue)
      dp[i] = Math.Min(dp[i], dp[i-c]+1);
return dp[amount] == int.MaxValue ? -1 : dp[amount];`,

  "climbing-stairs":
`int[] dp = new int[n+1];
dp[0] = 1; dp[1] = 1;
for (int i = 2; i <= n; i++)
  dp[i] = dp[i-1] + dp[i-2];
return dp[n];`,

  "permutations":
`void Backtrack(List<int> path, bool[] used) {
  if (path.Count == n) { results.Add([..path]); return; }
  for (int i = 0; i < n; i++) {
    if (used[i]) continue;
    used[i] = true; path.Add(nums[i]);
    Backtrack(path, used);
    used[i] = false; path.RemoveAt(path.Count-1);
  }
}`,

  "subsets":
`void Backtrack(int start, List<int> path) {
  results.Add([..path]);
  for (int i = start; i < n; i++) {
    path.Add(nums[i]);
    Backtrack(i+1, path);
    path.RemoveAt(path.Count-1);
  }
}`,

  "combination-sum":
`void Backtrack(int start, List<int> path, int rem) {
  if (rem == 0) { results.Add([..path]); return; }
  for (int i = start; i < n; i++) {
    if (candidates[i] > rem) break;
    path.Add(candidates[i]);
    Backtrack(i, path, rem - candidates[i]);
    path.RemoveAt(path.Count-1);
  }
}`,

  "palindrome-partitioning":
`void Backtrack(int start, List<string> path) {
  if (start == s.Length) { results.Add([..path]); return; }
  for (int end = start+1; end <= s.Length; end++) {
    string sub = s[start..end];
    if (IsPalindrome(sub)) {
      path.Add(sub); Backtrack(end, path);
      path.RemoveAt(path.Count-1);
    }
  }
}`,

  "bit-manipulation":
`bool check = (n & (1 << k)) != 0;
int  set    =  n | (1 << k);
int  clear  =  n & ~(1 << k);
int  toggle =  n ^ (1 << k);
int  count  =  BitOperations.PopCount((uint)n);
bool pow2   = (n & (n-1)) == 0;`,

  "linear-search":
`for (int i = 0; i <= n-m; i++)
  if (text[i..(i+m)] == pattern)
    matches.Add(i);`,

  "kmp":
`// build lps[] failure function first
int i = 0, j = 0;
while (i < n) {
  if (text[i] == pattern[j]) { i++; j++; }
  if (j == m) { matches.Add(i-j); j = lps[j-1]; }
  else if (text[i] != pattern[j]) {
    if (j > 0) j = lps[j-1]; else i++;
  }
}`,

  "boyer-moore":
`// build badChar & goodSuffix tables first
int s = 0;
while (s <= n-m) {
  int j = m-1;
  while (j >= 0 && pattern[j] == text[s+j]) j--;
  if (j < 0) { matches.Add(s); s += goodSuffix[0]; }
  else s += Math.Max(goodSuffix[j],
                     j - badChar[text[s+j]]);
}`,

  "rabin-karp":
`int hPat = Hash(pattern), hWin = Hash(text[..m]);
for (int i = 0; i <= n-m; i++) {
  if (hWin == hPat && text[i..(i+m)] == pattern)
    matches.Add(i);
  if (i < n-m) hWin = Rehash(hWin, text[i], text[i+m]);
}`,

  "longest-palindrome":
`string res = "";
for (int i = 0; i < n; i++)
  foreach (var (l0,r0) in new[]{(i,i),(i,i+1)}) {
    int l = l0, r = r0;
    while (l>=0 && r<n && s[l]==s[r]) { l--; r++; }
    if (r-l-1 > res.Length) res = s[(l+1)..r];
  }`,

  "anagram-detection":
`int[] count = new int[26];
foreach (char c in s1) count[c-'a']++;
foreach (char c in s2) count[c-'a']--;
return count.All(x => x == 0);`,

  "reversal":
`char[] arr = s.ToCharArray();
for (int l = 0, r = arr.Length-1; l < r; l++, r--)
  (arr[l], arr[r]) = (arr[r], arr[l]);
return new string(arr);`,
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
