import { NavLink } from "react-router-dom";
import {
  arrayConfig,
  stringConfig,
  treeConfig,
  graphConfig,
  dpConfig,
  backtrackingConfig,
  nrTheoryConfig,
} from "../../config/algorithms";
import { useLearned } from "../../context/LearnedContext";
import "./Navigation.css";

interface Tab {
  label: string;
  basePath: string;
  items: { name: string; path: string }[];
}

function fromArray(slugs: string[]) {
  return slugs.map((slug) => {
    const a = arrayConfig.find((c) => c.slug === slug)!;
    return { name: a.name, path: `/array/${a.slug}` };
  });
}

function fromString(slugs: string[]) {
  return slugs.map((slug) => {
    const a = stringConfig.find((c) => c.slug === slug)!;
    return { name: a.name, path: `/string/${a.slug}` };
  });
}

function from(slugs: string[]) {
  const all = [
    ...arrayConfig,
    ...stringConfig,
    ...treeConfig,
    ...graphConfig,
    ...dpConfig,
    ...backtrackingConfig,
    ...nrTheoryConfig,
  ];
  return slugs.map((slug) => {
    const a = all.find((c) => c.slug === slug)!;
    return { name: a.name, path: `/${a.category}/${a.slug}` };
  });
}

const tabs: Tab[] = [
  {
    label: "Sort",
    basePath: "/sort",
    items: from([
      "bubble-sort",
      "quick-sort",
      "merge-sort",
      "insertion-sort",
      "selection-sort",
    ]),
  },
  {
    label: "Find",
    basePath: "/find",
    items: from([
      "binary-search",
      "longest-palindrome",
      "sliding-window",
      "two-pointers",
      "kadane",
    ]),
  },
  {
    label: "Patterns",
    basePath: "/patterns",
    items: from(["kmp", "boyer-moore", "rabin-karp", "anagram-detection"]),
  },
  {
    label: "Transform",
    basePath: "/transform",
    items: fromString(["run-length-encoding", "levenshtein", "reversal"]),
  },
  {
    label: "Trees",
    basePath: "/trees",
    items: from([
      "bst-insert-search",
      "inorder",
      "level-order",
      "lca",
      "diameter",
      "validate-bst",
      "invert",
      "huffman",
    ]),
  },
  {
    label: "Graphs",
    basePath: "/graphs",
    items: from([
      "bfs",
      "dfs",
      "dijkstra",
      "union-find",
      "topological-sort",
      "cycle-detection",
      "bellman-ford",
    ]),
  },
  {
    label: "Dynamic Prog.",
    basePath: "/dp",
    items: from([
      "fibonacci",
      "coin-change",
      "lcs",
      "knapsack",
      "edit-distance",
      "lis",
      "subset-sum",
    ]),
  },
  {
    label: "Backtracking",
    basePath: "/backtracking",
    items: from([
      "n-queens",
      "combination-sum",
      "permutations",
      "sudoku",
      "word-search",
      "rat-in-maze",
    ]),
  },
  {
    label: "Nr. Theory",
    basePath: "/number-theory",
    items: from(["sieve", "gcd", "prime-factorization", "bit-manipulation"]),
  },
];

export default function Navigation() {
  const { isLearned } = useLearned();

  return (
    <nav className="nav">
      <div className="nav-brand">Algorithm Visualizer</div>
      <div className="nav-tabs">
        {tabs.map((tab) => (
          <div key={tab.label} className="nav-tab-group">
            <span className="nav-tab-label">{tab.label}</span>
            <div className="nav-subtabs">
              {tab.items.map((item) => {
                const key = item.path.slice(1);
                const learned = isLearned(key);
                return (
                  <NavLink
                    key={item.path}
                    to={item.path}
                    className={({ isActive }) =>
                      `nav-subtab${isActive ? " active" : ""}${learned ? " learned" : ""}`
                    }
                  >
                    {learned ? "✅ " : ""}
                    {item.name}
                  </NavLink>
                );
              })}
            </div>
          </div>
        ))}
      </div>
    </nav>
  );
}
