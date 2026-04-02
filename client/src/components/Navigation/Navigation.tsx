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
  const all = [...arrayConfig, ...stringConfig];
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
    label: "Traverse",
    basePath: "/traverse",
    items: [
      ...treeConfig.map((a) => ({ name: a.name, path: `/tree/${a.slug}` })),
      ...graphConfig.map((a) => ({ name: a.name, path: `/graph/${a.slug}` })),
    ],
  },
  {
    label: "Misc",
    basePath: "/misc",
    items: [
      ...dpConfig.map((a) => ({ name: a.name, path: `/dp/${a.slug}` })),
      ...backtrackingConfig.map((a) => ({
        name: a.name,
        path: `/backtracking/${a.slug}`,
      })),
      ...nrTheoryConfig.map((a) => ({
        name: a.name,
        path: `/number-theory/${a.slug}`,
      })),
    ],
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
