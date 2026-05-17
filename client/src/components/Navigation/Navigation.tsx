import { useState } from "react";
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
import { algorithmRatings } from "../../config/ratings";
import "./Navigation.css";

function Stars({ n }: Readonly<{ n: number }>) {
  return (
    <span className="nav-stars">
      {"★".repeat(n)}
      <span className="nav-stars-empty">{"★".repeat(5 - n)}</span>
    </span>
  );
}

interface Tab {
  label: string;
  basePath: string;
  items: { name: string; path: string; rating: number }[];
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
  return slugs
    .map((slug) => {
      const a = all.find((c) => c.slug === slug)!;
      return { name: a.name, path: `/${a.category}/${a.slug}`, rating: algorithmRatings[slug]?.stars ?? 0 };
    })
    .sort((a, b) => b.rating - a.rating);
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
    items: from(["reversal", "huffman"]),
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
    ]),
  },
  {
    label: "Graphs",
    basePath: "/graphs",
    items: from([
      "bfs",
      "dfs",
      "dijkstra",
      "topological-sort",
      "cycle-detection",
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
      "lis",
    ]),
  },
  {
    label: "Backtracking",
    basePath: "/backtracking",
    items: from(["combination-sum", "permutations"]),
  },
  {
    label: "Nr. Theory",
    basePath: "/number-theory",
    items: from(["bit-manipulation"]),
  },
];

export default function Navigation() {
  const { isLearned } = useLearned();
  const [menuOpen, setMenuOpen] = useState(false);
  const [openCategory, setOpenCategory] = useState<string | null>(null);

  const closeMenu = () => {
    setMenuOpen(false);
    setOpenCategory(null);
  };

  return (
    <nav className="nav">
      <div className="nav-brand">Algorithm Visualizer</div>
      <div className="nav-tabs">
        {tabs.map((tab) => (
          <div key={tab.label} className="nav-tab-group">
            <span
              className={`nav-tab-label${tab.items.every((item) => isLearned(item.path.slice(1))) ? " all-learned" : ""}`}
            >
              {tab.label}
            </span>
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
                    <span>{learned ? "✅ " : ""}{item.name}</span>
                    <Stars n={item.rating} />
                  </NavLink>
                );
              })}
            </div>
          </div>
        ))}
      </div>

      <button
        className={`nav-burger${menuOpen ? " open" : ""}`}
        onClick={() => setMenuOpen((o) => !o)}
        aria-label="Toggle menu"
      >
        <span />
        <span />
        <span />
      </button>

      {menuOpen && (
        <div className="nav-mobile-menu">
          {tabs.map((tab) => (
            <div key={tab.label} className="nav-mobile-group">
              <button
                className={`nav-mobile-category${openCategory === tab.label ? " open" : ""}`}
                onClick={() =>
                  setOpenCategory((c) => (c === tab.label ? null : tab.label))
                }
              >
                {tab.label}
                <span className="nav-mobile-arrow">▾</span>
              </button>
              {openCategory === tab.label && (
                <div className="nav-mobile-items">
                  {tab.items.map((item) => {
                    const key = item.path.slice(1);
                    const learned = isLearned(key);
                    return (
                      <NavLink
                        key={item.path}
                        to={item.path}
                        onClick={closeMenu}
                        className={({ isActive }) =>
                          `nav-mobile-item${isActive ? " active" : ""}${learned ? " learned" : ""}`
                        }
                      >
                        <span>{learned ? "✅ " : ""}{item.name}</span>
                        <Stars n={item.rating} />
                      </NavLink>
                    );
                  })}
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </nav>
  );
}
