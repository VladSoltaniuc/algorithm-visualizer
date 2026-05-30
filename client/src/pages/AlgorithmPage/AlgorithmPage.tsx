import { useState, useEffect } from "react";
import { useParams, Navigate } from "react-router-dom";
import { useLearned } from "../../context/LearnedContext";
import { allConfigs } from "../../config/algorithms";
import { sortApi } from "../../api/sortApi";
import { findApi } from "../../api/findApi";
import { patternApi } from "../../api/patternApi";
import { miscApi } from "../../api/miscApi";
import { graphApi } from "../../api/graphApi";
import { dpApi } from "../../api/dpApi";
import { backtrackingApi } from "../../api/backtrackingApi";
import { treeApi } from "../../api/treeApi";
import ArrayVisualizer from "../../components/ArrayVisualizer/ArrayVisualizer";
import StringVisualizer from "../../components/StringVisualizer/StringVisualizer";
import GraphVisualizer from "../../components/GraphVisualizer/GraphVisualizer";
import DPVisualizer from "../../components/DPVisualizer/DPVisualizer";
import BacktrackingVisualizer from "../../components/BacktrackingVisualizer/BacktrackingVisualizer";
import NrTheoryVisualizer from "../../components/NrTheoryVisualizer/NrTheoryVisualizer";
import TreeVisualizer from "../../components/TreeVisualizer/TreeVisualizer";
import { algorithmRatings } from "../../config/ratings";
import {
  algorithmPseudocode,
  algorithmShortcuts,
} from "../../config/CodeSnippets";
import type { AlgorithmStep } from "../../types";
import "./AlgorithmPage.css";

const apiMap: Record<
  string,
  Record<string, (...args: unknown[]) => Promise<AlgorithmStep[]>>
> = {
  sort: sortApi,
  find: findApi,
  pattern: patternApi,
  misc: miscApi,
  graph: graphApi,
  dp: dpApi,
  backtracking: backtrackingApi,
  tree: treeApi,
};

const slugToApiKey = (slug: string): string =>
  slug.replace(/-([a-z])/g, (_, c: string) => c.toUpperCase());

const ratingLabels: Record<number, string> = {
  1: "Rarely applicable",
  2: "Niche use",
  3: "Situational",
  4: "Broadly useful",
  5: "Reach for it constantly",
};

export default function AlgorithmPage() {
  const { category, slug } = useParams<{ category: string; slug: string }>();
  const { isLearned, toggle } = useLearned();

  const configs = category ? allConfigs[category] : null;
  const config = configs?.find((c) => c.slug === slug) ?? null;

  const [input, setInput] = useState(config?.defaultInput ?? "");
  const [target, setTarget] = useState(config?.defaultTarget?.toString() ?? "");
  const [windowSize, setWindowSize] = useState(
    config?.defaultWindowSize?.toString() ?? "",
  );
  const [pattern, setPattern] = useState(config?.defaultPattern ?? "");
  const [steps, setSteps] = useState<AlgorithmStep[]>([]);
  const [loading, setLoading] = useState(false);
  const [spamPrevention, setSpamPrevention] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState<"what" | "proscons">("what");
  const [mobileTabOpen, setMobileTabOpen] = useState(false);

  const [graphData, setGraphData] = useState<{
    nodeCount: number;
    edges: number[][];
  } | null>(null);
  const [treeInputValues, setTreeInputValues] = useState<number[]>([]);

  useEffect(() => {
    if (!config) return;
    setInput(config.defaultInput ?? "");
    setTarget(config.defaultTarget?.toString() ?? "");
    setWindowSize(config.defaultWindowSize?.toString() ?? "");
    setPattern(config.defaultPattern ?? "");
    setSteps([]);
    setError(null);
    setActiveTab("what");
    setGraphData(null);
    setTreeInputValues([]);
  }, [category, slug]);

  if (!category || !slug) return <Navigate to="/sort/bubble-sort" replace />;
  if (!configs) return <Navigate to="/404" replace />;
  if (!config) return <Navigate to="/404" replace />;

  const handleRun = async () => {
    const inputType = config.inputType ?? "numbers";
    let parsedInput: unknown;

    switch (inputType) {
      case "numbers": {
        const numbers = input.split(",").map((n) => parseInt(n.trim(), 10));
        if (numbers.some(isNaN)) {
          setError("Please enter valid comma-separated numbers.");
          return;
        }
        if (slug === "combination-sum" && numbers.some((n) => n <= 0)) {
          setError(
            "This implementation expects an all-positive array. Please enter only positive integers.",
          );
          return;
        }
        parsedInput = numbers;
        break;
      }
      case "text":
        if (!input.trim()) {
          setError("Please enter some text.");
          return;
        }
        parsedInput = input;
        break;
      case "number": {
        const num = parseInt(input.trim(), 10);
        if (isNaN(num)) {
          setError("Please enter a valid number.");
          return;
        }
        parsedInput = num;
        break;
      }
      case "graph":
        if (!input.trim()) {
          setError("Please enter graph data.");
          return;
        }
        parsedInput = input;
        break;
    }

    setLoading(true);
    setError(null);
    setSteps([]);

    try {
      const apiModule = apiMap[category];
      const apiFn = apiModule?.[slugToApiKey(slug)];
      if (!apiFn) throw new Error("Unknown algorithm");

      let data: AlgorithmStep[];
      if (config.needsTarget && config.needsPattern) {
        const t = parseInt(target, 10);
        if (isNaN(t)) {
          setError("Please enter a valid target.");
          setLoading(false);
          return;
        }
        data = await apiFn(parsedInput, t, pattern);
      } else if (config.needsTarget) {
        const t = parseInt(target, 10);
        if (isNaN(t)) {
          setError("Please enter a valid target.");
          setLoading(false);
          return;
        }
        data = await apiFn(parsedInput, t);
      } else if (config.needsPattern) {
        data = await apiFn(parsedInput, pattern);
      } else if (config.needsWindowSize) {
        const w = parseInt(windowSize, 10);
        if (isNaN(w) || w <= 0) {
          setError("Please enter a valid window size.");
          setLoading(false);
          return;
        }
        data = await apiFn(parsedInput, w);
      } else {
        data = await apiFn(parsedInput);
      }
      setSteps(data);
      if (category === "graph") {
        try {
          const parts = input
            .split(";")
            .map((s) => s.trim())
            .filter(Boolean);
          const nodeCount = Number.parseInt(parts[0], 10);
          if (!Number.isNaN(nodeCount)) {
            const edges = parts.slice(1).map((e) => e.split(",").map(Number));
            setGraphData({ nodeCount, edges });
          }
        } catch {
          /* ignore */
        }
      }
      if (
        (category === "tree" || (category === "misc" && slug === "huffman")) &&
        config.inputType !== "text"
      ) {
        const nums = input.split(",").map((n) => Number.parseInt(n.trim(), 10));
        if (!nums.some(Number.isNaN)) setTreeInputValues(nums);
      }
    } catch (err) {
      setError(
        err instanceof Error ? err.message : "An unexpected error occurred.",
      );
    }
    setLoading(false);
    setSpamPrevention(true);
    setTimeout(() => setSpamPrevention(false), 1000);
  };

  const learnedKey = `${category}/${slug}`;

  const inputControls = (
    <div className="input-row">
      <label>
        {config.inputLabel ?? "Array"}
        <input
          type="text"
          value={input}
          onChange={(e) => setInput(e.target.value)}
          placeholder={
            config.inputType === "text"
              ? "Enter text"
              : config.inputType === "number"
                ? "Enter a number"
                : "Comma-separated values"
          }
        />
      </label>

      {config.needsTarget && (
        <label>
          {config.targetLabel ?? "Target"}
          <input
            type="number"
            value={target}
            onChange={(e) => setTarget(e.target.value)}
          />
        </label>
      )}

      {config.needsPattern && (
        <label>
          {config.patternLabel ?? "Pattern"}
          <input
            type="text"
            value={pattern}
            onChange={(e) => setPattern(e.target.value)}
          />
        </label>
      )}

      {config.needsWindowSize && (
        <label>
          Window size
          <input
            type="number"
            value={windowSize}
            onChange={(e) => setWindowSize(e.target.value)}
            min={1}
          />
        </label>
      )}
    </div>
  );

  return (
    <div className="algorithm-page">
      <div className="algo-tabs">
        <div className="algo-tab-bar">
          <button
            className={`algo-tab-btn${activeTab === "what" ? " active" : ""}`}
            onMouseEnter={() => setActiveTab("what")}
          >
            Explanation
          </button>
          <button
            className={`algo-tab-btn${activeTab === "proscons" ? " active" : ""}`}
            onMouseEnter={() => setActiveTab("proscons")}
          >
            Pros &amp; Cons
          </button>
          <div className="algo-tab-mobile-menu">
            <button
              className="algo-tab-hamburger"
              onClick={() => setMobileTabOpen((o) => !o)}
            >
              ☰ {activeTab === "what" ? "Explanation" : "Pros & Cons"}
            </button>
            {mobileTabOpen && (
              <div className="algo-tab-mobile-dropdown">
                <button
                  onClick={() => {
                    setActiveTab("what");
                    setMobileTabOpen(false);
                  }}
                >
                  Explanation
                </button>
                <button
                  onClick={() => {
                    setActiveTab("proscons");
                    setMobileTabOpen(false);
                  }}
                >
                  Pros &amp; Cons
                </button>
              </div>
            )}
          </div>
          <span className="algo-tab-title">
            {algorithmPseudocode[slug] && (
              <>
                <span className="algo-code-badge">{"</Code>"}</span>
                <span className="algo-code-tip">
                  {algorithmShortcuts[slug] && (
                    <span className="algo-code-shortcut">
                      <span className="algo-code-shortcut-label">
                        Memory shortcut:
                      </span>{" "}
                      {algorithmShortcuts[slug]}
                    </span>
                  )}
                  <pre className="algo-code-pre">
                    {algorithmPseudocode[slug]}
                  </pre>
                </span>
              </>
            )}
            <span className="algo-name">{config.name}</span>
            {algorithmRatings[slug] && (
              <>
                <span className="algo-rating-badge">
                  Rating {algorithmRatings[slug].stars}/5
                </span>
                <span className="algo-rating-tip">
                  <span className="algo-code-shortcut">
                    <span className="algo-code-shortcut-label">
                      Usability :
                    </span>
                    {ratingLabels[algorithmRatings[slug].stars]}
                    <span className="rating-stars">
                      {"★".repeat(algorithmRatings[slug].stars)}
                      <span className="rating-stars-empty">
                        {"★".repeat(5 - algorithmRatings[slug].stars)}
                      </span>
                    </span>
                  </span>
                  <span className="algo-rating-text">
                    {algorithmRatings[slug].tooltip}
                  </span>
                </span>
              </>
            )}
          </span>
          <div className="algo-tab-right">
            {config.ytTutorial && (
              <a
                href={config.ytTutorial}
                target="_blank"
                rel="noopener noreferrer"
                className="algo-tab-btn algo-tab-yt"
              >
                <svg
                  className="yt-icon"
                  viewBox="0 0 48 48"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    fill="#FF0000"
                    d="M47.52 13.4a5.97 5.97 0 0 0-4.2-4.22C39.52 8 24 8 24 8s-15.52 0-19.32 1.18a5.97 5.97 0 0 0-4.2 4.22C0 17.18 0 24 0 24s0 6.82.48 10.6a5.97 5.97 0 0 0 4.2 4.22C8.48 40 24 40 24 40s15.52 0 19.32-1.18a5.97 5.97 0 0 0 4.2-4.22C48 30.82 48 24 48 24s0-6.82-.48-10.6Z"
                  />
                  <path fill="#FFF" d="m19.2 31.2 12.96-7.2L19.2 16.8v14.4Z" />
                </svg>
                <span className="yt-label">Tutorial that helped me</span>
              </a>
            )}
            <label className="learned-toggle">
              <input
                type="checkbox"
                checked={isLearned(learnedKey)}
                onChange={() => toggle(learnedKey)}
              />
              <span className="learned-label">Mark as learned</span>
            </label>
          </div>
        </div>
        <div className="algo-tab-content">
          {activeTab === "what" && (
            <p className="algo-tab-text">{config.description}</p>
          )}
          {activeTab === "proscons" && (
            <div className="pros-cons-columns">
              <ul className="pros-list">
                {config.pros.map((pro, i) => (
                  <li key={i} className="pro-item">
                    {pro}
                  </li>
                ))}
              </ul>
              <ul className="cons-list">
                {config.cons.map((con, i) => (
                  <li key={i} className="con-item">
                    {con}
                  </li>
                ))}
              </ul>
            </div>
          )}
        </div>
      </div>

      {error && <p className="error">{error}</p>}

      {category === "pattern" && (
        <StringVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          inputControls={inputControls}
        />
      )}
      {category === "graph" && (
        <GraphVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          edges={graphData?.edges ?? []}
          nodeCount={graphData?.nodeCount ?? 0}
          directed={config?.directed}
          inputControls={inputControls}
        />
      )}
      {category === "dp" && (
        <DPVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          inputControls={inputControls}
        />
      )}
      {category === "backtracking" && (
        <BacktrackingVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          slug={slug}
          inputControls={inputControls}
        />
      )}
      {category === "misc" && slug === "bit-manipulation" && (
        <NrTheoryVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          slug={slug}
          inputControls={inputControls}
        />
      )}
      {category === "misc" && slug === "reversal" && (
        <StringVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          inputControls={inputControls}
        />
      )}
      {category === "misc" && slug === "huffman" && (
        <ArrayVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          inputControls={inputControls}
        />
      )}
      {(category === "sort" || category === "find") && (
        <ArrayVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          inputControls={inputControls}
        />
      )}
      {category === "tree" && (
        <TreeVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          inputValues={treeInputValues}
          inputControls={inputControls}
        />
      )}
    </div>
  );
}
