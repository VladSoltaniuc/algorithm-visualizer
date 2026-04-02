import { useState, useEffect } from "react";
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

  useEffect(() => {
    if (!config) return;
    setInput(config.defaultInput ?? "");
    setTarget(config.defaultTarget?.toString() ?? "");
    setWindowSize(config.defaultWindowSize?.toString() ?? "");
    setPattern(config.defaultPattern ?? "");
    setSteps([]);
    setError(null);
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
      <div className="algo-header">
        <h2>
          {isLearned(learnedKey) ? "✅ " : ""}
          {config.name}
        </h2>
        <label className="learned-toggle">
          <input
            type="checkbox"
            checked={isLearned(learnedKey)}
            onChange={() => toggle(learnedKey)}
          />
          Mark as learned
        </label>
      </div>
      <p className="algo-description">{config.description}</p>

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
        <ArrayVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
        />
      )}
    </div>
  );
}
