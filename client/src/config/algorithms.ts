import type { AlgorithmConfig } from '../types';

export const arrayConfig: AlgorithmConfig[] = [
  {
    name: 'Bubble Sort', slug: 'bubble-sort', endpoint: 'bubble-sort', category: 'array', defaultInput: '5,3,8,1,2',
    description: "Imagine your friend lined up their DVD collection in a shelf, but the titles are all out of order. You start from the left, look at two DVDs sitting next to each other, and if the right one should come before the left one, you swap them. You keep walking the shelf doing this swap after swap, pass after pass, until no more swaps are needed — the shelf is sorted. The heaviest out-of-place DVDs slowly \"bubble up\" to their correct spot, which is how it got its name.",
    usecase: 'Educational purposes, tiny datasets, or arrays that are already nearly sorted — where its simplicity and best-case performance shine.',
    pros: [
      'O(n) time best case — when the array is already sorted, one pass confirms it',
      'O(1) space — sorts in place with no extra memory',
      'Stable sort — equal elements keep their original order',
      'Dead simple to implement and understand',
    ],
    cons: [
      'O(n²) time average and worst case — painfully slow on large or reverse-sorted data',
      'Far more swaps than most other sorting algorithms',
      'Almost never used in practice for real workloads',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=xli_FI7CuzA',
  },
  {
    name: 'Quick Sort', slug: 'quick-sort', endpoint: 'quick-sort', category: 'array', defaultInput: '5,3,8,1,2,7,4',
    description: "Imagine you're organizing a pile of receipts on your desk. You grab one receipt at random — that's your pivot. You then go through the rest of the pile putting everything cheaper than the pivot on the left and everything more expensive on the right. Now you have two smaller piles. You repeat the same process on each pile, picking a new pivot each time, until every pile is just one receipt. The Lomuto scheme always picks the last receipt in the current pile as the pivot, then walks through from left to right moving cheaper ones to the front as it goes.",
    usecase: 'General-purpose sorting for large datasets. The default sort in many standard libraries due to excellent real-world performance.',
    pros: [
      'O(n log n) time average — one of the fastest comparison sorts in practice',
      'O(log n) space average — only the recursion stack',
      'In-place sorting — no large auxiliary arrays needed',
      'Cache-friendly — sequential memory access patterns',
    ],
    cons: [
      'O(n²) time worst case — happens when the pivot is consistently the smallest or largest element',
      'Not stable — equal elements may be reordered',
      'O(n) space worst case — deep recursion on pathological inputs',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=MZaf_9IZCrc',
  },
  {
    name: 'Merge Sort', slug: 'merge-sort', endpoint: 'merge-sort', category: 'array', defaultInput: '5,3,8,1,2,7,4',
    description: "Imagine you and a friend are sorting a huge pile of numbered cards. You split the pile in half and each take one. You both keep splitting your halves again and again until everyone is holding just one card — one card is always sorted. Now you start merging back: you and your friend hold your piles face up and compare the top cards one by one, always placing the smaller one into the final sorted pile first. You keep merging piles this way until all the cards are back in one fully sorted stack.",
    usecase: 'When you need a guaranteed O(n log n) sort, a stable sort, or are sorting linked lists and external data (files too large for memory).',
    pros: [
      'O(n log n) time always — best, average, and worst case are all the same',
      'Stable sort — equal elements preserve their original order',
      'Predictable performance — no pathological worst cases',
    ],
    cons: [
      'O(n) extra space — needs a full auxiliary array for merging',
      'Slower in practice than Quick Sort due to memory allocation overhead',
      'Not in-place — the extra memory can be a problem for very large arrays',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=4VqmGXwpLqc',
  },
  {
    name: 'Binary Search', slug: 'binary-search', endpoint: 'binary-search', category: 'array', needsTarget: true, targetLabel: 'Search target', defaultInput: '1,3,5,7,9,11,13', defaultTarget: 7,
    description: "Imagine you're looking for a friend's number in an old phone book. You wouldn't start from page 1 — you'd flip to the middle. If your friend's name comes before the middle page, you tear the book in half and only keep the left half. If it comes after, you keep the right half. You keep flipping to the middle of whatever is left, throwing away the half that can't contain the name, until you find it. Binary Search does exactly this — but with a sorted list of numbers.",
    usecase: 'Searching sorted data — database indexes, dictionary lookups, finding boundaries (lower/upper bounds), and any search-in-sorted-range problems.',
    pros: [
      'O(log n) time — halves the search space every step, extremely fast even on millions of elements',
      'O(1) space — no extra memory needed',
    ],
    cons: [
      'Requires sorted input — if unsorted, sorting first costs O(n log n)',
      'Only works on random-access structures (arrays) — not linked lists',
      'Off-by-one errors make it notoriously tricky to implement correctly',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=MFhxShGxHWc',
  },
  {
    name: 'Insertion Sort', slug: 'insertion-sort', endpoint: 'insertion-sort', category: 'array', defaultInput: '5,3,8,1,2',
    description: "Imagine you're being dealt playing cards one at a time. Each time you receive a new card, you slide it into the correct position among the cards already in your hand — shifting the others to make room. You're not sorting your whole hand at once, you're just finding where the new card belongs and inserting it. By the time all cards are dealt, your hand is already fully sorted.",
    usecase: 'Small arrays (under ~20 elements), nearly sorted data, or as the base case inside hybrid sorts like Timsort and Introsort.',
    pros: [
      'O(n) time best case — nearly sorted data runs in near-linear time',
      'O(1) space — sorts in place',
      'Stable sort — preserves equal-element order',
      'Adaptive — takes advantage of existing order in the data',
      'Online — can sort elements as they arrive one by one',
    ],
    cons: [
      'O(n²) time average and worst case — too slow for large unsorted data',
      'Lots of shifting — each insertion may move many elements',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=JU767SDMDvA',
  },
  {
    name: 'Selection Sort', slug: 'selection-sort', endpoint: 'selection-sort', category: 'array', defaultInput: '5,3,8,1,2',
    description: "Imagine you have a row of numbered boxes all mixed up and you want to sort them from smallest to biggest. You scan the entire row to find the smallest box, then drag it to the very beginning. Now you ignore that first box and scan the rest again for the next smallest, dragging it to position two. You keep doing this — each time scanning what's left and pulling the smallest to the front — until the whole row is sorted.",
    usecase: 'When the number of swaps must be minimized (e.g., writing to flash memory is expensive), or for educational purposes.',
    pros: [
      'O(1) space — sorts in place with no extra memory',
      'At most O(n) swaps — fewest swaps of any comparison sort',
      'Simple to implement and understand',
    ],
    cons: [
      'O(n²) time always — best, average, and worst case are all the same',
      'Not stable — swapping can reorder equal elements',
      'Not adaptive — already-sorted data takes just as long',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=g-PGLbMth_g',
  },
  {
    name: 'Two Pointers', slug: 'two-pointers', endpoint: 'two-pointers', category: 'array', needsTarget: true, targetLabel: 'Target sum', defaultInput: '1,2,3,4,5,6,7,8', defaultTarget: 9,
    description: "Imagine a sorted row of price tags and you're trying to find two items that together cost exactly $9. Instead of checking every possible pair, you place one finger on the cheapest item on the far left and another on the most expensive on the far right. If the two prices add up to too much, move the right finger one step left. If they add up to too little, move the left finger one step right. You keep closing in from both ends until you find the pair that adds up to exactly $9 — or run out of options.",
    usecase: 'Pair-sum problems, removing duplicates in sorted arrays, merging sorted arrays, and partitioning problems.',
    pros: [
      'O(n) time — single pass through the array',
      'O(1) space — just two index variables',
      'Elegant and efficient for the problems it fits',
    ],
    cons: [
      'Requires sorted input — sorting first adds O(n log n)',
      'Only applicable to specific problem patterns — not a general technique',
    ],
    ytTutorial: 'https://youtu.be/-gjxg6Pln50?t=215',
  },
  {
    name: 'Sliding Window', slug: 'sliding-window', endpoint: 'sliding-window', category: 'array', needsWindowSize: true, defaultInput: '2,1,5,1,3,2', defaultWindowSize: 3,
    description: "Imagine you're looking out of a train window that shows exactly 3 houses at a time as the train moves. You want to find the stretch of 3 houses with the highest total value. As the train moves one house forward, the window drops the house at the back and picks up a new one at the front. You keep track of the total value in your current window at all times, updating it as you slide, until you've seen every possible group of 3.",
    usecase: 'Finding maximum/minimum sum subarrays, longest substrings with constraints, and any problem involving contiguous chunks of a sequence.',
    pros: [
      'O(n) time — processes each element at most twice (enter and leave the window)',
      'O(1) space for fixed-size windows',
      'Avoids redundant recalculation by incrementally updating the window state',
    ],
    cons: [
      'Only works on contiguous subarray/substring problems',
      'Variable-size windows can be harder to implement correctly',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=dOonV4byDEg',
  },
  {
    name: "Kadane's Algorithm", slug: 'kadane', endpoint: 'kadane', category: 'array', defaultInput: '-2,1,-3,4,-1,2,1,-5,4',
    description: "Imagine you're walking a trail and each step either gains or loses you altitude. You want to find the stretch of the trail where you gained the most altitude overall. As you walk, you keep a running total of your current gain. The moment your running total drops below zero, you know this stretch is dragging you down — so you reset and start fresh from the next step. You always remember the highest total you've ever seen, and that's your answer at the end.",
    usecase: 'Maximum subarray sum problems, stock buy/sell profit, and anywhere you need the best contiguous stretch in a sequence.',
    pros: [
      'O(n) time — single pass through the array',
      'O(1) space — only two variables (current sum and best sum)',
      'Optimal — no algorithm can do better for this problem',
    ],
    cons: [
      'Only finds the sum — tracking the actual subarray indices requires extra bookkeeping',
      'Requires at least one positive element for a meaningful result (all-negatives edge case needs special handling)',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=lq8KOs1Ujas',
  },
];

export const stringConfig: AlgorithmConfig[] = [
  {
    name: 'KMP', slug: 'kmp', endpoint: 'kmp', category: 'string', inputType: 'text', inputLabel: 'Text', defaultInput: 'ABABDABACDABABCABAB', needsPattern: true, patternLabel: 'Pattern', defaultPattern: 'ABABCABAB',
    description: 'Imagine you\'re searching for the word "ABABCABAB" hidden inside a long sentence, reading left to right. Most search methods would back up and start over every time they hit a mismatch. KMP never backs up. Instead, before searching, it studies the pattern itself and figures out: if a mismatch happens mid-pattern, how much of what I already matched can I reuse? For example if you matched "ABAB" before hitting a mismatch, KMP knows "AB" at the end of what you matched also appears at the start of the pattern — so it jumps back just enough to reuse that overlap, instead of starting from scratch.',
    usecase: 'Text editors (find/replace), DNA sequence matching, intrusion detection systems scanning network packets for known signatures.',
    pros: [
      'O(n + m) time guaranteed — linear in text length plus pattern length',
      'Never backtracks on the text — ideal for streaming data',
      'O(m) space for the failure/prefix table',
    ],
    cons: [
      'O(m) preprocessing time and space to build the failure table',
      'More complex to implement than naive search',
      'Slower in practice than Boyer-Moore for typical English text',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=V5-7GzOfADQ',
  },
  {
    name: 'Boyer-Moore', slug: 'boyer-moore', endpoint: 'boyer-moore', category: 'string', inputType: 'text', inputLabel: 'Text', defaultInput: 'HERE IS A SIMPLE EXAMPLE', needsPattern: true, patternLabel: 'Pattern', defaultPattern: 'EXAMPLE',
    description: 'Imagine you\'re scanning a page looking for the word "EXAMPLE". Instead of reading left to right like most people, Boyer-Moore holds the pattern up against the text and reads it backwards — from right to left. The moment it spots a mismatch, it checks: does this mismatched character even appear anywhere in the pattern? If not, the entire pattern can be skipped past that character in one jump. This is what makes it fast — it skips large chunks of text instead of checking every single character. This algorithm is a bit harder, check out the tutorial and learn about the bad-character and good-suffix rules that make those jumps possible. The whole ideea is to apply both rules and see the which one saves you the most steps (which one can jump further ahead).',
    usecase: 'Text search in editors (Ctrl+F), grep implementations, and any single-pattern search on large texts — especially with large alphabets.',
    pros: [
      'Sublinear in practice — often examines fewer than n characters by skipping ahead',
      'Best case O(n/m) time — the longer the pattern, the bigger the skips',
      'The go-to algorithm for fast text search in real software',
    ],
    cons: [
      'O(n × m) worst case — pathological inputs (e.g., searching "aaa" in "aaaaaaa")',
      'O(m + alphabet size) preprocessing space for the bad-character and good-suffix tables',
      'More complex to implement than KMP',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=4Xyhb72LCX4',
  },
  {
    name: 'Rabin-Karp', slug: 'rabin-karp', endpoint: 'rabin-karp', category: 'string', inputType: 'text', inputLabel: 'Text', defaultInput: 'GEEKS FOR GEEKS', needsPattern: true, patternLabel: 'Pattern', defaultPattern: 'GEEK',
    description: 'Imagine you\'re looking for the word "GEEK" in a long text. Instead of comparing letters one by one each time, Rabin-Karp converts "GEEK" into a number — called a hash. It then slides a same-sized window across the text and converts each window into a number too. If the numbers don\'t match, it moves on instantly without comparing any letters. If the numbers do match, only then does it actually check the letters to confirm. The clever part is that each new window\'s number is calculated by slightly adjusting the previous one, making it very fast.',
    usecase: 'Multi-pattern search (searching for many patterns at once), plagiarism detection, and fingerprinting-based substring matching.',
    pros: [
      'O(n + m) time average — rolling hash makes window advancement O(1)',
      'Easily extended to search for multiple patterns simultaneously',
      'Simple to implement compared to KMP and Boyer-Moore',
    ],
    cons: [
      'O(n × m) worst case — hash collisions force full character-by-character comparisons',
      'Performance depends heavily on hash function quality',
      'Spurious hits (false hash matches) waste time on unnecessary comparisons',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=qQ8vS2btsxI',
  },
  {
    name: 'LPS (Manacher)', slug: 'longest-palindrome', endpoint: 'longest-palindrome', category: 'string', inputType: 'text', inputLabel: 'Text', defaultInput: 'babad',
    description: 'Imagine a word that reads the same forwards and backwards — like "racecar". That\'s a palindrome. Now imagine you want to find the longest such stretch hiding inside a bigger string like "babad". Checking every possible stretch one by one would take forever. Manacher\'s algorithm is smarter — it keeps track of the biggest palindrome it has found so far and uses it as a mirror. Any new stretch that falls inside that mirror has a twin on the other side whose length is already known, so it can be copied instantly instead of rechecked. Only when a stretch reaches or goes beyond the mirror\'s edge does it need to explore new territory.',
    usecase: 'Finding the longest palindromic substring, DNA sequence analysis (biological palindromes), and text processing tasks requiring palindrome detection.',
    pros: [
      'O(n) time — optimal, finds all palindromes in a single linear pass',
      'Finds every palindromic substring, not just the longest one',
    ],
    cons: [
      'O(n) extra space for the radius array',
      'One of the hardest string algorithms to understand and implement correctly',
      'Requires string preprocessing (inserting separator characters)',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=1ir1eryUr80',
  },
  {
    name: 'Anagram Detection', slug: 'anagram-detection', endpoint: 'anagram-detection', category: 'string', inputType: 'text', inputLabel: 'Text', defaultInput: 'cbaebabacd', needsPattern: true, patternLabel: 'Pattern', defaultPattern: 'abc',
    description: 'Short answer, Sliding Window + Hashmaps. Long answer, imagine you\'re looking for all hidden rearrangements of the word "abc" inside the text "cbaebabacd". An anagram is just a reshuffling of the same letters — "bca", "cab", and "abc" are all anagrams of each other. Anagram Detection slides a window the same size as the pattern across the text, and at each position checks whether the window contains exactly the same letters in any order. Instead of rearranging and comparing every time, it keeps a running count of letters in the current window and updates it with each step — dropping the leftmost letter and picking up the new right one.',
    usecase: 'Word games, spell checking, genomic analysis (finding rearranged DNA subsequences), and text analysis tools.',
    pros: [
      'O(n) time — each character enters and leaves the window exactly once',
      'O(1) space — frequency array is bounded by alphabet size (26 for lowercase English)',
      'Simple sliding window approach — easy to implement',
    ],
    cons: [
      'Limited to fixed-length pattern matches — only finds anagrams of the exact pattern length',
      'Alphabet size affects constant factor — larger alphabets mean larger frequency arrays',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=IRN1VcA8CGc',
  },
  {
    name: 'String Reversal', slug: 'reversal', endpoint: 'reversal', category: 'string', inputType: 'text', inputLabel: 'Text', defaultInput: 'hello',
    description: 'Imagine a row of lettered tiles spelling "hello". You place one finger on the first tile and another on the last. You swap them, then move both fingers one step inward and swap again. You keep swapping and closing in until your two fingers meet in the middle — at which point the whole word is reversed. That\'s it. No extra tiles needed, everything happens in place.',
    usecase: 'Palindrome checks, reversing words in a sentence, and as a building block inside other string algorithms.',
    pros: [
      'O(n) time — exactly n/2 swaps',
      'O(1) space — fully in-place, no extra memory',
      'As simple as it gets — two pointers and a swap',
    ],
    cons: [
      'Trivially simple — limited standalone applications',
      'Doesn\'t handle multi-byte Unicode characters (e.g., emojis) without extra care',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=OD9PjLapAOQ',
  },
  {
    name: 'Run-Length Encoding', slug: 'run-length-encoding', endpoint: 'run-length-encoding', category: 'string', inputType: 'text', inputLabel: 'Text', defaultInput: 'aaabbccdddd',
    description: 'Imagine you\'re sending a text message describing a wall that goes "aaabbccdddd". Instead of typing every letter out, you write "3a2b2c4d" — meaning three a\'s, two b\'s, two c\'s, four d\'s. That\'s Run-Length Encoding. It scans the string from left to right, counts how many times each character repeats in a row, and writes the count followed by the character. It\'s one of the simplest forms of compression and works best when the same character repeats many times in a row.',
    usecase: 'Simple image compression (BMP, TIFF, fax machines), game map storage, and any data with long runs of repeated values.',
    pros: [
      'O(n) time — single pass through the string',
      'O(1) extra space — output is built directly',
      'Dead simple to implement — both encoding and decoding',
    ],
    cons: [
      'Can increase size — "abcd" becomes "1a1b1c1d" (doubles the length)',
      'Only effective when the same character repeats many times consecutively',
      'Far weaker compression than Huffman, LZ77, or other modern schemes',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=h-TGpvQYyDI',
  },
];

export const treeConfig: AlgorithmConfig[] = [
  {
    name: 'Inorder Traversal', slug: 'inorder', endpoint: 'inorder', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8',
    description: 'Imagine a family tree where every person on the left side has a smaller number than their parent, and everyone on the right has a larger one. Inorder Traversal visits the tree by always going as far left as possible first, then reading the current person, then moving right. If you follow this rule all the way through, you end up reading every number in the tree from smallest to biggest — like magic, it comes out perfectly sorted.',
    usecase: 'Reading BST elements in sorted order, evaluating expression trees, and any scenario where sorted output from a BST is needed.',
    pros: [
      'O(n) time — visits every node exactly once',
      'Produces sorted output for BSTs automatically',
      'O(h) space — where h is the tree height (recursion stack)',
    ],
    cons: [
      'O(n) space worst case — if the tree is completely skewed (like a linked list)',
      'Recursive implementation risks stack overflow on very deep trees',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=g_S5WuasWUE',
  },
  {
    name: 'Preorder Traversal', slug: 'preorder', endpoint: 'preorder', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8',
    description: 'Using the same family tree, Preorder Traversal visits the parent first, then goes left, then goes right. Think of it as a leader who always announces themselves before introducing their left side and then their right side. This order is useful when you want to copy or recreate the tree exactly, because you always know the parent before its children.',
    usecase: 'Copying/cloning a tree, serializing a tree for storage or transmission, and evaluating prefix expressions.',
    pros: [
      'O(n) time — visits every node exactly once',
      'O(h) space — recursion stack proportional to tree height',
      'Output can reconstruct the original BST exactly',
    ],
    cons: [
      'O(n) space worst case for skewed trees',
      'Does not produce sorted output',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=g_S5WuasWUE',
  },
  {
    name: 'Postorder Traversal', slug: 'postorder', endpoint: 'postorder', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8',
    description: 'Still in the same family tree, Postorder Traversal visits the left side first, then the right side, and only then reads the parent — children before parents, always. Think of it as cleaning up a tree of folders on your computer: you have to empty every subfolder before you can delete the parent folder. The root is always the very last thing visited.',
    usecase: 'Safely deleting/freeing a tree (children before parent), evaluating postfix expressions, and computing directory sizes.',
    pros: [
      'O(n) time — visits every node exactly once',
      'O(h) space — recursion stack proportional to tree height',
      'Guarantees children are processed before their parent — safe for deletion',
    ],
    cons: [
      'O(n) space worst case for skewed trees',
      'Less intuitive than preorder or inorder',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=g_S5WuasWUE',
  },
  {
    name: 'Level Order (BFS)', slug: 'level-order', endpoint: 'level-order', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8',
    description: 'Imagine the tree as a company org chart. Level Order visits every person floor by floor — the CEO first, then all managers on the second floor, then all team leads on the third, and so on. It uses a queue — a waiting line — to keep track of who\'s next. Each time it visits someone, it adds their left and right children to the back of the line, ensuring everyone on the current floor is visited before anyone on the next floor.',
    usecase: 'Printing trees level by level, finding the minimum depth, connecting nodes at the same level, and shortest-path problems in unweighted trees.',
    pros: [
      'O(n) time — visits every node exactly once',
      'Finds closest/shallowest nodes first — useful for level-wise processing',
      'Iterative — no recursion stack overflow risk',
    ],
    cons: [
      'O(w) space where w is the maximum width — up to n/2 for a perfect binary tree',
      'Uses more memory than DFS-based traversals on wide trees',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=6ZnyEApgFYg',
  },
  {
    name: 'BST Insert / Search', slug: 'bst-insert-search', endpoint: 'bst-insert-search', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8', needsTarget: true, targetLabel: 'Search for', defaultTarget: 4,
    description: 'Imagine a filing cabinet where every folder is numbered. Smaller numbers go in the left drawer, bigger numbers go in the right drawer — and this rule applies inside every drawer too. To insert a new number, you start at the top and keep going left if your number is smaller or right if it\'s bigger, until you find an empty spot. Searching works the same way — at every step you either find your number, go left, or go right. You never have to check every folder, just follow the rule.',
    usecase: 'Dictionaries, symbol tables, database indexes, and any key-value store where ordered data needs fast lookup.',
    pros: [
      'O(log n) time average for both insert and search in a balanced tree',
      'O(1) extra space for iterative search',
      'Maintains sorted order — supports range queries and ordered iteration',
    ],
    cons: [
      'O(n) time worst case — if the tree degenerates into a linked list (sorted insertions)',
      'No self-balancing — use AVL or Red-Black trees for guaranteed O(log n)',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=wcIRPqTR3Kc',
  },
  {
    name: 'Tree Height', slug: 'height', endpoint: 'height', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8',
    description: 'Imagine you\'re standing at the top of a tree and want to know how many floors it has. You send a scout down every branch, all the way to the last leaf. Each scout counts how many steps they took to reach the bottom and reports back. You take the longest report — that\'s the height of the tree. The algorithm does this by asking each node: how tall is your left side? How tall is your right side? Take the bigger one and add one for the current floor.',
    usecase: 'Checking if a tree is balanced, determining worst-case traversal depth, and calculating performance bounds for tree-based data structures.',
    pros: [
      'O(n) time — visits every node exactly once',
      'O(h) space — recursion stack proportional to height',
      'Simple and elegant recursive solution',
    ],
    cons: [
      'O(n) space worst case for skewed trees',
      'Must visit every single node — no shortcut possible',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=_O-yUiJHbNY',
  },
  {
    name: 'Lowest Common Ancestor', slug: 'lca', endpoint: 'lca', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8', needsTarget: true, targetLabel: 'Node A', defaultTarget: 1, needsPattern: true, patternLabel: 'Node B', defaultPattern: '4',
    description: 'Imagine two cousins in a family tree and you want to find the closest relative they share. Starting from the top of the tree, you ask at each person: are both cousins in my left side? Both in my right? Or do they split here — one left, one right? The moment they split, or the moment you land on one of them directly, that\'s the lowest common ancestor — the closest shared family member between the two.',
    usecase: 'Version control (finding the merge base), organizational hierarchies, phylogenetic trees, and network routing (finding the common gateway).',
    pros: [
      'O(h) time for BST — follows a single path from root to the LCA',
      'O(1) extra space with iterative implementation',
      'Leverages BST property to avoid visiting unnecessary nodes',
    ],
    cons: [
      'O(n) time worst case for skewed trees',
      'This BST approach doesn\'t work on general binary trees — those require O(n) algorithms',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=gs2LMfuOR9k',
  },
  {
    name: 'Invert Binary Tree', slug: 'invert', endpoint: 'invert', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8',
    description: 'Imagine looking at a tree in a mirror. Every left branch becomes a right branch, and every right branch becomes a left branch — all the way down to the last leaf. Invert Binary Tree does exactly this: starting at the top, it swaps the left and right children of every single node, then goes deeper and does the same for every node below, until the entire tree is flipped like a mirror image of itself.',
    usecase: 'Creating mirror images of trees, interview classic, and certain symmetry-based tree problems.',
    pros: [
      'O(n) time — swaps children at every node exactly once',
      'O(h) space — recursion stack proportional to height',
      'Elegant — just one swap per node',
    ],
    cons: [
      'Destroys the original tree structure — BST property is lost',
      'O(n) space worst case for skewed trees',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=OnSn2XEQ4MY',
  },
  {
    name: 'Validate BST', slug: 'validate-bst', endpoint: 'validate-bst', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8',
    description: 'Remember how Inorder Traversal reads a valid BST in perfectly sorted order? Validate BST uses that exact trick. It walks the tree in inorder — left, root, right — and checks that every number it reads is larger than the one before it. If the sequence ever goes backwards, the tree is not a valid BST. If it stays in order all the way to the end, the tree passes the test.',
    usecase: 'Data integrity checks after tree operations, debugging BST implementations, and verifying deserialized trees.',
    pros: [
      'O(n) time — visits every node exactly once',
      'O(h) space — recursion stack proportional to height',
      'Conclusive — gives a definitive yes/no answer',
    ],
    cons: [
      'Must visit every single node — cannot short-circuit in best case (unless invalid found early)',
      'O(n) space worst case for skewed trees',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=s6ATEkipzow',
  },
  {
    name: 'Diameter of Binary Tree', slug: 'diameter', endpoint: 'diameter', category: 'tree', inputLabel: 'BST values', defaultInput: '5,3,7,1,4,6,8',
    description: 'Imagine stretching a piece of string between two leaves in a tree, pulling it tight through whatever branches connect them. The diameter is the longest such string you can find — the longest path between any two nodes, anywhere in the tree. For every node, the algorithm asks: what\'s the longest path that passes through me, combining my deepest left branch and my deepest right branch? It keeps track of the longest answer seen across every node, and that\'s the diameter.',
    usecase: 'Network latency analysis (longest communication path), tree-based layout calculations, and graph theory problems on tree structures.',
    pros: [
      'O(n) time — single DFS traversal computes everything',
      'O(h) space — recursion stack proportional to height',
    ],
    cons: [
      'O(n) space worst case for skewed trees',
      'The longest path may not pass through the root — a common misunderstanding',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=bkxqA8Rfv04',
  },
  {
    name: 'Huffman Coding', slug: 'huffman', endpoint: 'huffman', category: 'tree', inputType: 'text', inputLabel: 'Text', defaultInput: 'huffman coding example',
    description: 'Imagine you\'re sending a long message and want to shrink it as much as possible. Letters that appear often should take up less space, and rare letters can afford to take up more. Huffman Coding counts how often each letter appears, then builds a tree where the most frequent letters end up closest to the top and the rarest sink to the bottom. Each left branch means "0" and each right branch means "1", so every letter gets its own unique binary code based on its path from the top. The result is a compressed version of the text where common letters are represented by just a few bits.',
    usecase: 'File compression (ZIP, GZIP internals), JPEG/MP3 compression as a final encoding stage, and data transmission over bandwidth-limited channels.',
    pros: [
      'Optimal prefix-free encoding — no other prefix code produces a shorter result',
      'O(n log n) time to build the tree (n = number of unique characters)',
      'Lossless — the original data is perfectly reconstructable',
    ],
    cons: [
      'O(n) space for the Huffman tree',
      'Requires two passes over the data — one to count frequencies, one to encode',
      'Overhead for storing the tree alongside the compressed data — not worth it for very small inputs',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=co4_ahEDCho',
  },
];

export const graphConfig: AlgorithmConfig[] = [
  {
    name: 'BFS', slug: 'bfs', endpoint: 'bfs', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to;...)', defaultInput: '6;0,1;0,2;1,3;2,3;3,4;4,5', needsTarget: true, targetLabel: 'Start node', defaultTarget: 0,
    description: 'Imagine you\'re standing in the middle of a city and want to explore every street, starting from your current corner. BFS explores the city floor by floor — first every street directly connected to you, then every street connected to those, then the next ring out, and so on. It uses a waiting line: every time you visit a corner, you add all its unvisited neighbors to the back of the line. You always visit the person at the front of the line next, which guarantees you explore everything close to you before venturing further away.',
    usecase: 'Shortest path in unweighted graphs, social network friend suggestions (degrees of separation), web crawling, and level-wise graph analysis.',
    pros: [
      'O(V + E) time — visits every vertex and edge once',
      'Finds the shortest path in unweighted graphs automatically',
      'Complete — guaranteed to find a solution if one exists',
    ],
    cons: [
      'O(V) space for the queue — must store all nodes at the current frontier',
      'Doesn\'t work with weighted edges — use Dijkstra for that',
      'Uses more memory than DFS on deep, narrow graphs',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=oDqjPvD54Ss',
  },
  {
    name: 'DFS', slug: 'dfs', endpoint: 'dfs', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to;...)', defaultInput: '6;0,1;0,2;1,3;2,3;3,4;4,5', needsTarget: true, targetLabel: 'Start node', defaultTarget: 0,
    description: 'Using the same city, DFS explores very differently. Instead of fanning out in all directions, it picks one street and follows it as far as it possibly can — turning down every new road it finds — until it hits a dead end. Only then does it backtrack to the last junction and try a different road. Think of it as exploring a maze by always hugging the left wall: you go deep into one path completely before trying another.',
    usecase: 'Topological sorting, cycle detection, maze solving, connected components, and strongly connected components (Tarjan/Kosaraju).',
    pros: [
      'O(V + E) time — visits every vertex and edge once',
      'O(V) space — only the current path is stored on the stack',
      'Memory-efficient on deep, narrow graphs compared to BFS',
      'Natural fit for backtracking problems',
    ],
    cons: [
      'Does not find the shortest path',
      'Can get stuck in deep or infinite branches without depth limits',
      'Recursive implementation risks stack overflow on very deep graphs',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=Urx87-NMm6c',
  },
  {
    name: "Dijkstra's Shortest Path", slug: 'dijkstra', endpoint: 'dijkstra', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to,weight;...)', defaultInput: '5;0,1,4;0,2,1;1,3,1;2,1,2;2,3,5;3,4,3', needsTarget: true, targetLabel: 'Start node', defaultTarget: 0,
    description: 'Imagine you\'re a GPS trying to find the fastest route from your home to every other location in a city. Each road has a travel time. Dijkstra\'s algorithm starts at your home and always visits the closest unvisited location next — the one with the smallest total travel time from home. When it visits a location, it checks whether going through it offers a faster route to any of its neighbors, and updates the estimate if so. It keeps doing this, always picking the cheapest unvisited stop, until it has found the shortest path to everywhere.',
    usecase: 'GPS navigation, network routing (OSPF protocol), game pathfinding (A* is an extension of Dijkstra), and any weighted shortest path problem.',
    pros: [
      'O((V + E) log V) time with a min-heap — efficient for sparse graphs',
      'Finds the shortest path from one source to all other nodes',
      'Greedy and optimal — always produces correct results for non-negative weights',
    ],
    cons: [
      'Cannot handle negative edge weights — gives incorrect results',
      'O(V) space for the distance array and priority queue',
      'Slower than BFS for unweighted graphs (priority queue overhead)',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=EFg3u_E6eHU',
  },
  {
    name: 'Bellman-Ford', slug: 'bellman-ford', endpoint: 'bellman-ford', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to,weight;...)', defaultInput: '5;0,1,6;0,2,7;1,2,8;1,3,5;1,4,-4;2,3,-3;2,4,9;3,1,-2;4,3,7', needsTarget: true, targetLabel: 'Start node', defaultTarget: 0,
    description: 'Like Dijkstra, Bellman-Ford finds the shortest path from a starting point to every other location. The difference is that some roads have negative costs — maybe a road gives you fuel instead of consuming it. Dijkstra can\'t handle this, but Bellman-Ford can. It works by going through every single road in the graph repeatedly — once for each node — and asking: can I find a cheaper way to reach this destination? It also detects a dangerous trap called a negative cycle, where a loop of roads keeps reducing your cost forever, making a true shortest path impossible.',
    usecase: 'Currency arbitrage detection (negative cycles), routing protocols (RIP/BGP), and any shortest-path problem allowing negative edge weights.',
    pros: [
      'Handles negative edge weights correctly',
      'Detects negative cycles — reports when no valid shortest path exists',
      'Simpler to implement than Dijkstra (no priority queue needed)',
    ],
    cons: [
      'O(V × E) time — much slower than Dijkstra for large graphs',
      'O(V) space for distance array',
      'Overkill if all edge weights are non-negative — use Dijkstra instead',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=obWXjtg0L64',
  },
  {
    name: 'Topological Sort', slug: 'topological-sort', endpoint: 'topological-sort', category: 'graph', inputType: 'graph', inputLabel: 'DAG (nodes;from,to;...)', defaultInput: '6;5,2;5,0;4,0;4,1;2,3;3,1',
    description: 'Imagine a list of university courses where some courses require you to take others first — you can\'t take Algorithms before taking Data Structures. Topological Sort arranges all the courses in a valid order so that every prerequisite always appears before the course that needs it. It only works on graphs with no circular dependencies — you can\'t have course A require course B which requires course A. The result is a flat ordered list where every arrow in the original graph always points forward, never backward.',
    usecase: 'Build systems (Makefiles), package managers (dependency resolution), task scheduling, course prerequisite planning, and spreadsheet cell evaluation order.',
    pros: [
      'O(V + E) time — linear in the size of the graph',
      'O(V) space for the output and auxiliary structures',
      'Gives a valid execution order for any dependency graph',
    ],
    cons: [
      'Only works on Directed Acyclic Graphs (DAGs) — fails if cycles exist',
      'Result is not unique — multiple valid orderings may exist',
      'Doesn\'t indicate which ordering is "best" — just that it\'s valid',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=dis_c84ejhQ',
    directed: true,
  },
  {
    name: 'Cycle Detection', slug: 'cycle-detection', endpoint: 'cycle-detection', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to;...)', defaultInput: '4;0,1;1,2;2,3;3,1',
    description: 'Imagine you\'re following a trail of arrows between cities and you want to know if any trail loops back on itself — if you could end up going in circles forever. Cycle Detection walks the graph using DFS and color-codes every city as it goes: white means unvisited, grey means currently being explored, black means fully done. If you ever follow an arrow and land on a grey city — one you\'re already in the middle of exploring — you\'ve found a cycle. A black city is safe, only grey means trouble.',
    usecase: 'Deadlock detection in operating systems, validating dependency graphs, preventing infinite loops in state machines, and checking if topological sort is possible.',
    pros: [
      'O(V + E) time — standard DFS traversal',
      'O(V) space for the color/visited array',
      'Gives a definitive yes/no answer',
    ],
    cons: [
      'DFS coloring is specific to directed graphs — undirected graphs need a different approach',
      'Only detects the existence of a cycle — doesn\'t enumerate all cycles',
      'Finding all distinct cycles is a much harder problem (exponential in general)',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=rKQaZuoUR4M',
    directed: true,
  },
  {
    name: 'Union-Find', slug: 'union-find', endpoint: 'union-find', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to;...)', defaultInput: '5;0,1;1,2;3,4',
    description: 'Imagine a group of people at a party and you want to track who is connected to whom — directly or through mutual friends. Union-Find maintains a collection of groups. It supports two actions: Find — which group does this person belong to? And Union — merge the groups of these two people together. Internally, each group is represented like a tree, and every person points toward the root of their group. Checking if two people are connected is as simple as checking if they share the same root.',
    usecase: 'Kruskal\'s MST implementation, network connectivity queries, image segmentation (connected pixel regions), and equivalence class tracking.',
    pros: [
      'Nearly O(1) amortized per operation — with path compression and union by rank, operations take O(α(n)) which is effectively constant',
      'O(n) space — one parent entry per element',
      'Extremely fast for connectivity queries',
    ],
    cons: [
      'Only tracks connectivity — doesn\'t store the actual path or edges between nodes',
      'Not suitable for directed graphs — only works with undirected connectivity',
      'Doesn\'t support disconnecting (splitting) groups efficiently',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=ibjEGG7ylHk',
  },
  {
    name: "Kruskal's MST", slug: 'kruskal', endpoint: 'kruskal', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to,weight;...)', defaultInput: '4;0,1,10;0,2,6;0,3,5;1,3,15;2,3,4',
    description: 'Imagine you\'re building a road network to connect a group of cities as cheaply as possible — every city must be reachable but you want to spend the least on road construction. Kruskal\'s algorithm sorts all possible roads by cost, cheapest first, and goes through them one by one. It adds a road to the network only if it connects two cities that aren\'t already connected — skipping any road that would create a redundant loop. It keeps adding roads until every city is connected. The result is a Minimum Spanning Tree — the cheapest possible network that reaches everywhere.',
    usecase: 'Network design (laying cable/fiber), clustering (removing the most expensive edges), circuit design, and approximation algorithms for NP-hard problems.',
    pros: [
      'O(E log E) time — dominated by sorting the edges',
      'Works on disconnected graphs — produces a minimum spanning forest',
      'Simple implementation when paired with Union-Find',
    ],
    cons: [
      'Requires sorting all edges upfront — O(E log E) even if MST is found early',
      'O(E) space to store all edges',
      'Slower than Prim\'s on dense graphs where E ≈ V²',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=JZBQLXgSGfs',
  },
  {
    name: "Prim's MST", slug: 'prim', endpoint: 'prim', category: 'graph', inputType: 'graph', inputLabel: 'Graph (nodes;from,to,weight;...)', defaultInput: '4;0,1,10;0,2,6;0,3,5;1,3,15;2,3,4', needsTarget: true, targetLabel: 'Start node', defaultTarget: 0,
    description: 'Prim\'s algorithm solves the exact same problem as Kruskal\'s — building the cheapest road network to connect all cities — but it grows the network differently. Instead of sorting all roads upfront, it starts at one city and at each step looks at all roads leading out of the already-connected network. It always picks the cheapest one that reaches a new city not yet in the network, adds that city, and repeats. It grows outward like a spreading tree, one cheapest branch at a time, until every city is connected.',
    usecase: 'Same as Kruskal\'s — network design, cable laying, circuit design — but preferred when the graph is dense (many edges).',
    pros: [
      'O((V + E) log V) time with a min-heap — faster than Kruskal on dense graphs',
      'Doesn\'t require sorting all edges — grows incrementally',
      'O(V) space for the priority queue and visited set',
    ],
    cons: [
      'Requires a connected graph — can\'t produce a spanning forest like Kruskal\'s',
      'Needs a priority queue — more complex implementation',
      'Slower than Kruskal\'s on sparse graphs where E ≈ V',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=jsmMtJpPnhU',
  },
];

export const dpConfig: AlgorithmConfig[] = [
  {
    name: 'Fibonacci', slug: 'fibonacci', endpoint: 'fibonacci', category: 'dp', inputType: 'number', inputLabel: 'n', defaultInput: '10',
    description: 'Imagine you\'re counting how many rabbits you have each month, where every month each pair of adult rabbits produces a new pair. The sequence goes 1, 1, 2, 3, 5, 8, 13... — each number is just the sum of the two before it. Simple enough, but if you calculate it naively, you end up solving the same smaller problems over and over. Dynamic programming fixes this by writing down every answer as you go — a technique called memoization. The moment you\'ve calculated a number, you remember it, so if you ever need it again you just look it up instead of recalculating it from scratch.',
    usecase: 'Teaching memoization and dynamic programming, mathematical modeling, and as a building block in algorithms involving Fibonacci-like recurrences.',
    pros: [
      'O(n) time with DP — linear, compared to O(2^n) for naive recursion',
      'O(1) space with bottom-up optimization — only store the last two values',
      'Perfect introduction to the power of memoization',
    ],
    cons: [
      'O(n) space if using memoization (top-down) — one entry per subproblem',
      'Integer overflow for large n — Fibonacci grows exponentially',
      'The problem itself is trivially simple — DP is overkill but illustrative',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=JXUOMsFBDXQ',
  },
  {
    name: '0/1 Knapsack', slug: 'knapsack', endpoint: 'knapsack', category: 'dp', inputLabel: 'Weights', defaultInput: '1,3,4,5', needsTarget: true, targetLabel: 'Capacity', defaultTarget: 7, needsPattern: true, patternLabel: 'Values', defaultPattern: '1,4,5,7',
    description: 'Imagine you\'re a hiker packing a bag that can hold a maximum weight of 7kg. You have several items, each with a weight and a value. You want to pack the most valuable combination without exceeding the weight limit — and each item is either fully packed or left behind, no splitting allowed. The algorithm builds a grid where each row represents adding one more item to consider, and each column represents a different weight capacity. Each cell answers: what\'s the best value I can pack given these items and this capacity? It fills the grid step by step, each cell borrowing the best answer from above.',
    usecase: 'Resource allocation, budget optimization, cargo loading, portfolio selection, and any problem where you choose items with weight/value tradeoffs.',
    pros: [
      'O(n × W) time — pseudo-polynomial, fills the entire grid once',
      'Guarantees the optimal solution',
      'O(W) space with 1D array optimization',
    ],
    cons: [
      'O(n × W) space for the full grid (needed to reconstruct the selected items)',
      'Pseudo-polynomial — W depends on input magnitude, not input size (NP-hard problem)',
      'Not efficient when capacity W is very large',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=8LusJS5-AGo',
  },
  {
    name: 'Longest Common Subsequence', slug: 'lcs', endpoint: 'lcs', category: 'dp', inputType: 'text', inputLabel: 'Text 1', defaultInput: 'ABCBDAB', needsPattern: true, patternLabel: 'Text 2', defaultPattern: 'BDCAB',
    description: 'Imagine two words — "ABCBDAB" and "BDCAB" — and you want to find the longest sequence of letters that appears in both, in the same order, but not necessarily side by side. For example "BCAB" appears in both if you skip around. LCS builds a grid comparing every letter of one word against every letter of the other. When two letters match, it extends the best subsequence found so far. When they don\'t, it borrows the best answer from either ignoring the current letter of word one or word two — whichever is longer.',
    usecase: 'Diff tools (git diff, file comparison), DNA sequence alignment, version control merge strategies, and plagiarism detection.',
    pros: [
      'O(n × m) time — fills the grid exactly once',
      'Guarantees the optimal (longest) answer',
      'Can reconstruct the actual subsequence by backtracking through the grid',
    ],
    cons: [
      'O(n × m) space for the full grid',
      'Slow on very long sequences — quadratic growth',
      'O(min(n, m)) space optimization exists but loses the ability to reconstruct the subsequence',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=Ua0GhsJSlWM',
  },
  {
    name: 'Longest Increasing Subsequence', slug: 'lis', endpoint: 'lis', category: 'dp', inputLabel: 'Array', defaultInput: '10,22,9,33,21,50,41,60',
    description: 'Imagine a row of numbers: 10, 22, 9, 33, 21, 50, 41, 60. You want to find the longest chain of numbers that are strictly going up — but they don\'t have to be next to each other, you can skip numbers as long as the ones you pick keep climbing. For example 10, 22, 33, 50, 60 is one valid chain. The algorithm goes through each number and asks: what\'s the longest increasing chain that ends at this number? It looks back at all previous numbers smaller than the current one and builds on the best chain among them.',
    usecase: 'Patience sorting, longest chain problems, stock price analysis, and any problem involving finding monotonically increasing sequences.',
    pros: [
      'O(n²) time for the basic DP approach — simple and correct',
      'O(n) space — one DP value per element',
      'Can be optimized to O(n log n) with binary search and a patience-sorting trick',
    ],
    cons: [
      'O(n²) time for the basic version — slow on large arrays',
      'Basic DP only finds the length — reconstructing the actual subsequence needs extra bookkeeping',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=cjWnW0hdF1Y',
  },
  {
    name: 'Coin Change', slug: 'coin-change', endpoint: 'coin-change', category: 'dp', inputLabel: 'Coins', defaultInput: '1,5,10,25', needsTarget: true, targetLabel: 'Amount', defaultTarget: 22,
    description: 'Imagine you want to make exactly 22 cents using coins of 1, 5, 10, and 25 cents, using as few coins as possible. Instead of trying every combination, the algorithm builds up from zero. It asks: what\'s the fewest coins needed to make 1 cent? 2 cents? 3 cents? All the way up to 22. For each amount, it tries every coin and asks: if I use this coin, what\'s the fewest coins needed for the remaining amount? It picks whichever coin leaves the smallest total, building the answer step by step from the ground up.',
    usecase: 'Vending machines, currency exchange, making change, and any resource-minimization problem with reusable denominations.',
    pros: [
      'O(n × amount) time — where n is the number of coin types',
      'O(amount) space — single 1D array',
      'Guarantees the minimum number of coins',
    ],
    cons: [
      'Pseudo-polynomial — runtime depends on the amount value, not just input size',
      'Returns -1 (impossible) if no combination reaches the exact amount',
      'Greedy doesn\'t work for arbitrary coin sets — DP is necessary',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=H9bfqozjoqs',
  },
  {
    name: 'Levenshtein Distance', slug: 'levenshtein', endpoint: 'levenshtein', category: 'dp', inputType: 'text', inputLabel: 'Text 1', defaultInput: 'kitten', needsPattern: true, patternLabel: 'Text 2', defaultPattern: 'sitting',
    description: 'Imagine you mistyped "kitten" and meant to write "sitting". How many fixes does it take to get from one to the other? You can insert a letter, delete a letter, or swap a letter for a different one — each counts as one edit. Levenshtein Distance finds the minimum number of such edits needed. It builds a grid comparing every prefix of one word against every prefix of the other. Each cell answers: what\'s the cheapest way to match these two prefixes? It checks three neighbors — the cell above (delete), to the left (insert), and diagonal (replace or free if the characters match) — and picks the cheapest option, building the full answer bottom-up.',
    usecase: 'Spell checkers, autocorrect, DNA/protein sequence alignment, diff tools, and fuzzy string matching.',
    pros: [
      'O(n × m) time — optimal for this problem',
      'Handles all three edit operations uniformly',
      'O(min(n, m)) space with rolling-row optimization',
    ],
    cons: [
      'O(n × m) space for the full grid if you need to reconstruct the edits',
      'Quadratic — slow on very long strings (thousands of characters)',
      'Doesn\'t handle transpositions — Damerau-Levenshtein adds that',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=MiqoA-yF-0M',
  },
  {
    name: 'Subset Sum', slug: 'subset-sum', endpoint: 'subset-sum', category: 'dp', inputLabel: 'Set', defaultInput: '3,34,4,12,5,2', needsTarget: true, targetLabel: 'Target', defaultTarget: 9,
    description: 'Imagine you have a bag of numbered stones — 3, 34, 4, 12, 5, 2 — and you want to know if any combination of them adds up to exactly 9. You don\'t need to find the combination, just whether it\'s possible. The algorithm builds a grid where each row adds one more stone to consider, and each column represents a target sum from 0 up to 9. Each cell answers: is it possible to reach this sum using only the stones considered so far? It fills the grid by checking: can I reach this sum without the current stone, or can I reach what\'s left after using it?',
    usecase: 'Scheduling, resource partitioning, cryptographic knapsack problems, and decision problems involving subset selection.',
    pros: [
      'O(n × target) time — pseudo-polynomial',
      'O(target) space with 1D array optimization',
      'Simple boolean DP — each cell is just true/false',
    ],
    cons: [
      'NP-hard in general — pseudo-polynomial time depends on the target value, not just the number of elements',
      'Not efficient when the target is astronomically large',
      'Only answers yes/no — finding the actual subset requires backtracking through the grid',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=s6FhG--P7z0',
  },
  {
    name: 'Climbing Stairs', slug: 'climbing-stairs', endpoint: 'climbing-stairs', category: 'dp', inputType: 'number', inputLabel: 'Stairs (n)', defaultInput: '8',
    description: 'Imagine a staircase with 8 steps. You can take either 1 step or 2 steps at a time. How many different ways can you reach the top? It turns out the answer follows the same pattern as Fibonacci. To reach step 8, you either came from step 7 (took 1 step) or from step 6 (took 2 steps). So the number of ways to reach step 8 is just the number of ways to reach step 7 plus the number of ways to reach step 6. The algorithm builds this up from the bottom — starting from step 1 and step 2 — remembering each answer until it reaches the top.',
    usecase: 'Counting path problems, combinatorics, probability calculations, and as a gateway to understanding DP recurrences.',
    pros: [
      'O(n) time — single pass from bottom to top',
      'O(1) space — only store the last two values (same as Fibonacci)',
      'Extremely simple to implement',
    ],
    cons: [
      'Integer overflow for large n — the count grows exponentially',
      'Trivially simple — mostly used as a teaching tool',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=Y0lT9Fck7qI',
  },
];

export const backtrackingConfig: AlgorithmConfig[] = [
  {
    name: 'N-Queens', slug: 'n-queens', endpoint: 'n-queens', category: 'backtracking', inputType: 'number', inputLabel: 'Board size (n)', defaultInput: '4',
    description: 'Imagine placing 4 queens on a 4×4 chessboard so that no queen can attack another — no two queens can share the same row, column, or diagonal. The algorithm places one queen at a time, row by row. For each row it tries every column, and before placing a queen it checks: is this square safe from all queens already placed? If yes, it places it and moves to the next row. If it gets stuck — no safe square exists in the current row — it backtracks to the previous row, moves that queen to the next available safe square, and tries again. It keeps placing and backtracking until all queens are safely placed.',
    usecase: 'Constraint satisfaction problems, VLSI chip testing, parallel memory storage schemes, and as a classic example of backtracking.',
    pros: [
      'Prunes invalid branches early — avoids exploring configurations that already violate constraints',
      'O(n) space for storing queen positions',
      'Finds all valid solutions, not just one',
    ],
    cons: [
      'O(n!) time worst case — the number of configurations grows factorially',
      'Exponential — impractical for very large n (though solutions exist for all n >= 4)',
      'Brute-force at its core — just smarter than trying every possible placement',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=Ph95IHmRp5M',
  },
  {
    name: 'Sudoku Solver', slug: 'sudoku', endpoint: 'sudoku', category: 'backtracking', inputLabel: 'Grid (81 values, 0=empty)', defaultInput: '5,3,0,0,7,0,0,0,0,6,0,0,1,9,5,0,0,0,0,9,8,0,0,0,0,6,0,8,0,0,0,6,0,0,0,3,4,0,0,8,0,3,0,0,1,7,0,0,0,2,0,0,0,6,0,6,0,0,0,0,2,8,0,0,0,0,4,1,9,0,0,5,0,0,0,0,8,0,0,7,9',
    description: 'Imagine a 9×9 Sudoku grid with some numbers already filled in. The rule is every row, column, and 3×3 box must contain the digits 1 through 9 with no repeats. The algorithm scans for the first empty cell and tries placing every digit from 1 to 9. For each digit it asks: does this break any rule? If not, it fills the cell and moves to the next empty one. If it ever reaches a cell where no digit fits, it backtracks — erases the last placed digit, tries the next one, and continues. It keeps filling and backtracking until every cell is filled correctly.',
    usecase: 'Puzzle solving, constraint propagation problems, SAT solvers (Sudoku reduces to SAT), and teaching recursive backtracking.',
    pros: [
      'Guarantees a solution if one exists',
      'O(1) space — the grid is always 9×9 (81 cells, fixed size)',
      'Can be enhanced with constraint propagation for massive speedups',
    ],
    cons: [
      'O(9^(empty cells)) worst case — exponential in the number of blanks',
      'Slow on puzzles with many empty cells without optimizations',
      'Brute force — doesn\'t use human-like solving logic without added heuristics',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=mcXc8Mva2bA',
  },
  {
    name: 'Generate Permutations', slug: 'permutations', endpoint: 'permutations', category: 'backtracking', inputLabel: 'Array', defaultInput: '1,2,3',
    description: 'Imagine you have three numbered tiles — 1, 2, 3 — and you want to list every possible order they can be arranged in. The algorithm builds each arrangement one position at a time. It picks a tile for the first position, then picks from the remaining tiles for the second position, then the last tile fills the third. Every time it completes a full arrangement, it records it, then backtracks and swaps in a different tile to explore the next arrangement. It keeps doing this until every possible ordering has been found.',
    usecase: 'Brute-force optimization (try all orderings), generating test cases, combinatorial analysis, and scheduling all possible sequences.',
    pros: [
      'Generates all n! permutations — complete enumeration',
      'O(n) extra space — just the recursion stack and current permutation',
    ],
    cons: [
      'O(n * n!) time — n! permutations, each of length n',
      'Factorial growth — impractical beyond ~10-12 elements',
      'Output itself is O(n * n!) — storing all results requires massive memory',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=s7AvT7cGdSo',
  },
  {
    name: 'Generate Subsets', slug: 'subsets', endpoint: 'subsets', category: 'backtracking', inputLabel: 'Array', defaultInput: '1,2,3',
    description: 'Imagine the same three tiles — 1, 2, 3 — and you want every possible group you can form from them, including the empty group and the full group. That\'s 8 subsets in total. For each tile, the algorithm makes a simple yes or no decision: include it or leave it out. It builds subsets like a branching tree of decisions — at each step it branches into two paths, one with the current tile and one without. Every time it reaches the end of the tile list, it records whatever combination it has built so far.',
    usecase: 'Feature selection in machine learning, power set enumeration, combinatorial testing, and generating all possible configurations.',
    pros: [
      'Complete — generates all 2^n subsets',
      'O(n) recursion depth — space-efficient during generation',
    ],
    cons: [
      'O(n * 2^n) time — 2^n subsets, each up to length n',
      'Exponential growth — impractical beyond ~20-25 elements',
      'Output itself is O(n * 2^n) — storing all subsets requires exponential memory',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=REOH22Xwdkk',
  },
  {
    name: 'Rat in a Maze', slug: 'rat-in-maze', endpoint: 'rat-in-maze', category: 'backtracking', inputLabel: 'Grid (1=open, 0=blocked)', defaultInput: '1,0,0,0,1,1,0,0,0,1,1,0,0,0,0,1', needsTarget: true, targetLabel: 'Grid size (n)', defaultTarget: 4,
    description: 'Imagine a rat sitting in the top-left corner of a grid. Some cells are open and some are blocked. The rat wants to reach the bottom-right corner. The algorithm tries moving right first, then down. Before each move it checks: is this cell open and not yet visited? If yes, it steps in and tries again from there. If it gets completely stuck with no valid moves, it backtracks — steps back to the previous cell and tries a different direction. It keeps exploring and backtracking until it finds a path to the exit or confirms no path exists.',
    usecase: 'Robot navigation, pathfinding in constrained environments, maze solving, and grid-based puzzle games.',
    pros: [
      'Finds a valid path if one exists',
      'O(n²) space for the visited/solution grid',
      'Simple and intuitive recursive approach',
    ],
    cons: [
      'O(2^(n²)) worst case — exponential in grid size',
      'Does not find the shortest path — just any valid path',
      'Much slower than BFS for shortest-path problems',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=bLGZhJlt98Y',
  },
  {
    name: 'Word Search in Grid', slug: 'word-search', endpoint: 'word-search', category: 'backtracking', inputType: 'text', inputLabel: 'Grid (rows separated by ;)', defaultInput: 'ABCE;SFCS;ADEE', needsPattern: true, patternLabel: 'Word', defaultPattern: 'ABCCED',
    description: 'Imagine a grid of random letters and you\'re looking for the word "ABCCED" hidden somewhere in it, where each next letter must be directly above, below, left, or right of the previous one. The algorithm scans every cell in the grid looking for the first letter of the word. When it finds one, it tries to extend the match in all four directions, letter by letter. If it ever hits a wrong letter or a dead end, it backtracks — unmarks the path and tries a different direction. It keeps searching until it either spells out the full word or exhausts every possibility.',
    usecase: 'Word games (Boggle), puzzle solving, pattern detection in grids, and DNA sequence searching in 2D structures.',
    pros: [
      'Early termination — abandons a path the moment a letter doesn\'t match',
      'O(L) space where L is the word length — only stores the current path',
    ],
    cons: [
      'O(n * m * 4^L) worst case — exponential in word length L',
      'Slow for long words in large grids',
      'Each cell can be a starting point — n * m DFS attempts in the worst case',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=pfiQ_PS1g8E',
  },
  {
    name: 'Combination Sum', slug: 'combination-sum', endpoint: 'combination-sum', category: 'backtracking', inputLabel: 'Candidates', defaultInput: '2,3,6,7', needsTarget: true, targetLabel: 'Target', defaultTarget: 7,
    description: 'Imagine you have the numbers 2, 3, 6, and 7, and you want to find every combination that adds up to exactly 7 — you can reuse numbers as many times as you want. The algorithm builds combinations one number at a time, keeping a running total. Each time it adds a number it checks: did I hit the target? If yes, record the combination. Did I go over? Backtrack and try the next candidate. It explores every possible combination by diving deep into one path, then stepping back and branching into the next, until all valid combinations are found.',
    usecase: 'Financial planning (exact change), recipe ingredient combinations, and any problem requiring all ways to reach a target sum with reusable elements.',
    pros: [
      'Finds all valid combinations — complete enumeration',
      'Prunes branches that exceed the target — avoids wasted exploration',
      'O(target) recursion stack space',
    ],
    cons: [
      'Exponential time — number of valid combinations can grow very fast',
      'Reusable elements mean deeper recursion trees compared to without-replacement variants',
      'Output size itself can be exponential',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=GBKI9VSKdGg',
  },
  {
    name: 'Palindrome Partitioning', slug: 'palindrome-partitioning', endpoint: 'palindrome-partitioning', category: 'backtracking', inputType: 'text', inputLabel: 'Text', defaultInput: 'aab',
    description: 'Imagine the word "aab" and you want to find every way to split it into parts where each part reads the same forwards and backwards. For example "a", "a", "b" works because each piece is a single letter — and single letters are always palindromes. The algorithm tries every possible cut point from left to right. At each step it asks: is the current piece a palindrome? If yes, it records it and keeps cutting the remaining string. If no valid cut exists, it backtracks and tries a different split point, until every valid partitioning has been found.',
    usecase: 'Text processing, string decomposition, DNA palindrome analysis, and problems requiring all ways to split a string into valid segments.',
    pros: [
      'Complete — finds every valid partitioning',
      'O(n) recursion depth',
      'Palindrome checks can be precomputed in O(n²) for speedup',
    ],
    cons: [
      'O(n * 2^n) worst case — exponential number of partitions',
      'Slow for long strings with many palindromic substrings',
      'Output size itself is exponential',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=3jvWodd7ht0',
  },
];

export const nrTheoryConfig: AlgorithmConfig[] = [
  {
    name: 'Sieve of Eratosthenes', slug: 'sieve', endpoint: 'sieve', category: 'number-theory', inputType: 'number', inputLabel: 'Upper bound (n)', defaultInput: '30',
    description: 'Imagine you have a list of every number from 2 to 30 and you want to cross out all the non-primes, leaving only the primes behind. Start at 2 — the first prime. Cross out every multiple of 2: 4, 6, 8, and so on. Move to the next uncrossed number — 3. Cross out every multiple of 3. Keep going: the next uncrossed number is always a prime, and you cross out all its multiples. By the time you reach the end of the list, every number still standing is a prime. You never had to test any of them individually — you just kept eliminating their multiples.',
    usecase: 'Generating primes for cryptography (RSA key generation), competitive programming, prime-based number theory problems, and primality lookups.',
    pros: [
      'O(n log log n) time — extremely fast for generating all primes up to n',
      'Simple to implement — just mark multiples',
      'One pass gives you every prime up to n — no per-number testing needed',
    ],
    cons: [
      'O(n) space — must store a boolean for every number up to n',
      'Not suitable for very large n (billions+) — memory becomes the bottleneck',
      'Only generates primes in a range starting from 2 — use segmented sieve for arbitrary ranges',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=pKvGYOnO9Ao',
  },
  {
    name: 'Euclidean Algorithm (GCD)', slug: 'gcd', endpoint: 'gcd', category: 'number-theory', inputType: 'numbers', inputLabel: 'a, b', defaultInput: '48,18',
    description: 'Imagine you have two pieces of rope — one 48cm and one 18cm — and you want to find the longest ruler that measures both perfectly with no leftover. The Euclidean algorithm works like this: divide the longer rope by the shorter one and keep only the remainder. Now repeat with the shorter rope and the remainder. Keep going until the remainder is zero — the last non-zero remainder is your answer. For 48 and 18: 48 / 18 leaves 12, then 18 / 12 leaves 6, then 12 / 6 leaves 0. The answer is 6 — the greatest common divisor.',
    usecase: 'Simplifying fractions, RSA cryptography (computing modular inverses), least common multiple (LCM) via GCD, and modular arithmetic.',
    pros: [
      'O(log(min(a, b))) time — converges extremely fast',
      'O(1) space with iterative implementation',
      'One of the oldest known algorithms — proven correct for millennia',
    ],
    cons: [
      'Recursive version uses O(log(min(a, b))) stack space',
      'Only finds GCD — LCM requires an additional multiplication step',
      'Extended Euclidean algorithm needed for modular inverses — slightly more complex',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=p5gn2hj51hs',
  },
  {
    name: 'Prime Factorization', slug: 'prime-factorization', endpoint: 'prime-factorization', category: 'number-theory', inputType: 'number', inputLabel: 'n', defaultInput: '84',
    description: 'Imagine breaking a number like 84 apart into the smallest possible building blocks — numbers that can\'t be divided any further. These building blocks are called prime numbers. The algorithm starts by trying to divide 84 by the smallest prime, 2. It divides as many times as it can: 84 / 2 = 42, 42 / 2 = 21. Now 21 isn\'t divisible by 2, so it tries 3: 21 / 3 = 7. And 7 is itself a prime, so it stops. The result is 2 * 2 * 3 * 7 — the unique set of prime building blocks that multiply together to make 84.',
    usecase: 'Cryptography (RSA security relies on factorization being hard for large numbers), number theory, divisor counting, and simplifying mathematical expressions.',
    pros: [
      'O(sqrt(n)) time — only needs to check divisors up to the square root',
      'O(log n) space — the number of prime factors is at most log2(n)',
      'Simple trial division — easy to implement',
    ],
    cons: [
      'O(sqrt(n)) is slow for very large numbers (hundreds of digits) — basis of RSA security',
      'Trial division is not the fastest method — Pollard\'s rho and number field sieve are faster for large inputs',
      'No known polynomial-time algorithm for general factorization',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=6PDtgHhpCHo',
  },
  {
    name: 'Bit Manipulation', slug: 'bit-manipulation', endpoint: 'bit-manipulation', category: 'number-theory', inputType: 'number', inputLabel: 'n', defaultInput: '42',
    description: 'Every number in a computer is stored as a sequence of 1s and 0s — called bits. For example, the number 42 is stored as 101010 in binary. Bit manipulation is a set of tricks that work directly on these 1s and 0s instead of the number as a whole, making certain operations extremely fast. AND keeps a bit only if both numbers have a 1 there. OR keeps a bit if either number has a 1 there. XOR keeps a bit only if exactly one of the two numbers has a 1 there. These simple rules unlock surprisingly powerful shortcuts — like checking if a number is even, counting how many 1s it has, or swapping two values without using a third variable.',
    usecase: 'Low-level systems programming, embedded systems, permission flags, hash functions, competitive programming tricks, and performance-critical code.',
    pros: [
      'O(1) time per operation — single CPU instruction',
      'O(1) space — no extra memory',
      'Extremely fast — operates at the hardware level, faster than arithmetic',
    ],
    cons: [
      'Hard to read and debug — bitwise code is not human-friendly',
      'Limited to integer types — doesn\'t work on floats or strings',
      'Platform-dependent behavior for signed integers (arithmetic vs. logical shift)',
    ],
    ytTutorial: 'https://www.youtube.com/watch?v=7jkIUgLC29I',
  },
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
