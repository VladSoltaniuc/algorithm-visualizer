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

const tabs: Tab[] = [
  {
    label: "Array",
    basePath: "/array",
    items: arrayConfig.map((a) => ({ name: a.name, path: `/array/${a.slug}` })),
  },
  {
    label: "String",
    basePath: "/string",
    items: stringConfig.map((a) => ({
      name: a.name,
      path: `/string/${a.slug}`,
    })),
  },
  {
    label: "Graph",
    basePath: "/graph",
    items: graphConfig.map((a) => ({ name: a.name, path: `/graph/${a.slug}` })),
  },
  {
    label: "Tree",
    basePath: "/tree",
    items: treeConfig.map((a) => ({ name: a.name, path: `/tree/${a.slug}` })),
  },
  {
    label: "Dynamic Prog.",
    basePath: "/dp",
    items: dpConfig.map((a) => ({ name: a.name, path: `/dp/${a.slug}` })),
  },
  {
    label: "Backtracking",
    basePath: "/backtracking",
    items: backtrackingConfig.map((a) => ({
      name: a.name,
      path: `/backtracking/${a.slug}`,
    })),
  },
  {
    label: "Number Theory",
    basePath: "/number-theory",
    items: nrTheoryConfig.map((a) => ({
      name: a.name,
      path: `/number-theory/${a.slug}`,
    })),
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
            <span className="nav-tab-label">
              {tab.basePath === "/array" ? tab.label : `${tab.label} (WIP)`}
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
                      `nav-subtab${isActive ? " active" : ""}${
                        learned ? " learned" : ""
                      }`
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
