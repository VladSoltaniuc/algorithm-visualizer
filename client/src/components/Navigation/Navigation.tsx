import { useState, useEffect, useRef } from "react";
import { NavLink, useLocation } from "react-router-dom";
import {
  sortConfig,
  findConfig,
  patternConfig,
  miscConfig,
  treeConfig,
  graphConfig,
  dpConfig,
  backtrackingConfig,
} from "../../config/algorithms";
import { useLearned } from "../../context/LearnedContext";
import { algorithmRatings } from "../../config/ratings";
import "./Navigation.css";

function Stars({ n }: Readonly<{ n: number }>) {
  return (
    <span className="rating-stars">
      {"★".repeat(n)}
      <span className="rating-stars-empty">{"★".repeat(5 - n)}</span>
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
    ...sortConfig,
    ...findConfig,
    ...patternConfig,
    ...miscConfig,
    ...treeConfig,
    ...graphConfig,
    ...dpConfig,
    ...backtrackingConfig,
  ];
  return slugs
    .map((slug) => {
      const a = all.find((c) => c.slug === slug)!;
      return {
        name: a.name,
        path: `/${a.category}/${a.slug}`,
        rating: algorithmRatings[slug]?.stars ?? 0,
      };
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
    items: from(["binary-search", "sliding-window", "two-pointers", "kadane"]),
  },
  {
    label: "Patterns",
    basePath: "/patterns",
    items: from([
      "kmp",
      "boyer-moore",
      "rabin-karp",
      "anagram-detection",
      "longest-palindrome",
    ]),
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
    items: from(["fibonacci", "coin-change", "lcs", "knapsack", "lis"]),
  },
  {
    label: "Backtracking",
    basePath: "/backtracking",
    items: from(["combination-sum", "permutations"]),
  },
  {
    label: "Misc",
    basePath: "/misc",
    items: from(["reversal", "huffman", "bit-manipulation"]),
  },
];

export default function Navigation() {
  const { isLearned } = useLearned();
  const [menuOpen, setMenuOpen] = useState(false);
  const [openCategory, setOpenCategory] = useState<string | null>(null);
  const { pathname } = useLocation();
  const pathParts = pathname.split("/").filter(Boolean);
  const categoryFromPath = pathParts[0] ?? null;
  const slugFromPath = pathParts[1] ?? null;
  const currentAlgo = slugFromPath
    ? ([
        ...sortConfig,
        ...findConfig,
        ...patternConfig,
        ...miscConfig,
        ...treeConfig,
        ...graphConfig,
        ...dpConfig,
        ...backtrackingConfig,
      ].find((a) => a.slug === slugFromPath)?.name ?? null)
    : null;
  const currentAlgoLearned =
    categoryFromPath && slugFromPath
      ? isLearned(`${categoryFromPath}/${slugFromPath}`)
      : false;

  const navItems = tabs.flatMap((tab) => tab.items);
  const totalCount = navItems.length; // 36 - intentionally excludes hidden algorithms
  const learnedCount = navItems.filter((item) =>
    isLearned(item.path.slice(1)),
  ).length;

  const prevLearnedCount = useRef(learnedCount);
  const [shimmer, setShimmer] = useState(false);

  useEffect(() => {
    if (learnedCount > prevLearnedCount.current) {
      setShimmer(false);
      requestAnimationFrame(() =>
        requestAnimationFrame(() => setShimmer(true)),
      );
      const t = setTimeout(() => setShimmer(false), 800);
      return () => clearTimeout(t);
    }
    prevLearnedCount.current = learnedCount;
  }, [learnedCount]);

  const allLearnedKey = tabs
    .map((tab) =>
      tab.items.every((item) => isLearned(item.path.slice(1))) ? "1" : "0",
    )
    .join("");
  const prevAllLearnedKey = useRef(allLearnedKey);
  const [shimmerTabs, setShimmerTabs] = useState<Set<string>>(new Set());

  useEffect(() => {
    const newShimmers: string[] = [];
    tabs.forEach((tab, i) => {
      if (allLearnedKey[i] === "1" && prevAllLearnedKey.current[i] === "0") {
        newShimmers.push(tab.label);
      }
    });
    prevAllLearnedKey.current = allLearnedKey;
    if (newShimmers.length === 0) return;
    setShimmerTabs(new Set(newShimmers));
    const t = setTimeout(() => setShimmerTabs(new Set()), 800);
    return () => clearTimeout(t);
  }, [allLearnedKey]); // eslint-disable-line react-hooks/exhaustive-deps

  const closeMenu = () => {
    setMenuOpen(false);
    setOpenCategory(null);
  };

  return (
    <nav className="nav">
      <div
        className={`nav-brand${currentAlgoLearned ? " nav-brand--learned" : ""}`}
      >
        {currentAlgo ?? "Algorithm Visualizer"}
      </div>
      <div className="nav-tabs">
        {tabs.map((tab) => (
          <div key={tab.label} className="nav-tab-group">
            <span
              className={`nav-tab-label${tab.items.every((item) => isLearned(item.path.slice(1))) ? " all-learned" : ""}${shimmerTabs.has(tab.label) ? " shimmer" : ""}`}
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
                    <span>
                      {learned ? "✅ " : ""}
                      {item.name}
                    </span>
                    <Stars n={item.rating} />
                  </NavLink>
                );
              })}
            </div>
          </div>
        ))}
      </div>

      <div className={`nav-learned-counter${shimmer ? " shimmer" : ""}`}>
        <span
          className={
            learnedCount > 0 ? "nav-learned-count" : "nav-learned-total"
          }
        >
          {learnedCount}
        </span>
        <span className="nav-learned-total">/{totalCount}</span>
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
                        <span>
                          {learned ? "✅ " : ""}
                          {item.name}
                        </span>
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
