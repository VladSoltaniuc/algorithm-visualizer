import { useState, useEffect, useMemo } from "react";
import { useParams, Navigate } from "react-router-dom";
import { useLearned } from "../../context/LearnedContext";
import { allConfigs } from "../../config/algorithms";
import { arrayApi } from "../../api/arrayApi";
import { stringApi } from "../../api/stringApi";
import { graphApi } from "../../api/graphApi";
import { dpApi } from "../../api/dpApi";
import { backtrackingApi } from "../../api/backtrackingApi";
import { nrTheoryApi } from "../../api/nrTheoryApi";
import { treeApi } from "../../api/treeApi";
import ArrayVisualizer from "../../components/ArrayVisualizer/ArrayVisualizer";
import StringVisualizer from "../../components/StringVisualizer/StringVisualizer";
import GraphVisualizer from "../../components/GraphVisualizer/GraphVisualizer";
import DPVisualizer from "../../components/DPVisualizer/DPVisualizer";
import BacktrackingVisualizer from "../../components/BacktrackingVisualizer/BacktrackingVisualizer";
import NrTheoryVisualizer from "../../components/NrTheoryVisualizer/NrTheoryVisualizer";
import TreeVisualizer from "../../components/TreeVisualizer/TreeVisualizer";
import type { AlgorithmStep } from "../../types";
import "./AlgorithmPage.css";

const apiMap: Record<
  string,
  Record<string, (...args: unknown[]) => Promise<AlgorithmStep[]>>
> = {
  array: arrayApi as Record<
    string,
    (...args: unknown[]) => Promise<AlgorithmStep[]>
  >,
  string: stringApi,
  graph: graphApi,
  dp: dpApi,
  backtracking: backtrackingApi,
  "number-theory": nrTheoryApi,
  tree: treeApi,
};

const slugToApiKey = (slug: string): string =>
  slug.replace(/-([a-z])/g, (_, c: string) => c.toUpperCase());

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
  const [activeTab, setActiveTab] = useState<"what" | "usecase" | "proscons">(
    "what",
  );

  /* Parse graph edges from input string for the graph visualizer */
  const graphData = useMemo(() => {
    if (category !== "graph") return null;
    try {
      const parts = input.split(";").map((s) => s.trim()).filter(Boolean);
      const nodeCount = parseInt(parts[0], 10);
      if (isNaN(nodeCount)) return null;
      const edges = parts.slice(1).map((e) => e.split(",").map(Number));
      return { nodeCount, edges };
    } catch {
      return null;
    }
  }, [category, input]);

  /* Parse tree input values for the tree visualizer */
  const treeInputValues = useMemo(() => {
    if (category !== "tree") return [];
    if (config?.inputType === "text") return [];
    const nums = input.split(",").map((n) => parseInt(n.trim(), 10));
    if (nums.some(isNaN)) return [];
    return nums;
  }, [category, input, config?.inputType]);

  useEffect(() => {
    if (!config) return;
    setInput(config.defaultInput ?? "");
    setTarget(config.defaultTarget?.toString() ?? "");
    setWindowSize(config.defaultWindowSize?.toString() ?? "");
    setPattern(config.defaultPattern ?? "");
    setSteps([]);
    setError(null);
    setActiveTab("what");
  }, [category, slug]);

  if (!category || !slug) return <Navigate to="/array/bubble-sort" replace />;
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

  return (
    <div className="algorithm-page">
      <div className="algo-tabs">
        <div className="algo-tab-bar">
          <button
            className={`algo-tab-btn${activeTab === "what" ? " active" : ""}`}
            onClick={() => setActiveTab("what")}
          >
            What is it?
          </button>
          <button
            className={`algo-tab-btn${activeTab === "usecase" ? " active" : ""}`}
            onClick={() => setActiveTab("usecase")}
          >
            What for?
          </button>
          <button
            className={`algo-tab-btn${activeTab === "proscons" ? " active" : ""}`}
            onClick={() => setActiveTab("proscons")}
          >
            Pros &amp; Cons
          </button>
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
          {activeTab === "usecase" && (
            <p className="algo-tab-text">{config.usecase}</p>
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

      <div className="algo-controls">
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
      </div>

      {error && <p className="error">{error}</p>}

      {category === "string" && (
        <StringVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
        />
      )}
      {category === "graph" && (
        <GraphVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          edges={graphData?.edges ?? []}
          nodeCount={graphData?.nodeCount ?? 0}
        />
      )}
      {category === "dp" && (
        <DPVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
        />
      )}
      {category === "backtracking" && (
        <BacktrackingVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          slug={slug}
        />
      )}
      {category === "number-theory" && (
        <NrTheoryVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          slug={slug}
        />
      )}
      {category === "array" && (
        <ArrayVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
        />
      )}
      {category === "tree" && (
        <TreeVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          inputValues={treeInputValues}
        />
      )}
    </div>
  );
}
