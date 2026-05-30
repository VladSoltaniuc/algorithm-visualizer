import { useState, useEffect, useLayoutEffect, useRef } from "react";
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
import HuffmanVisualizer from "../../components/HuffmanVisualizer/HuffmanVisualizer";
import StringVisualizer from "../../components/StringVisualizer/StringVisualizer";
import GraphVisualizer from "../../components/GraphVisualizer/GraphVisualizer";
import DPVisualizer from "../../components/DPVisualizer/DPVisualizer";
import BacktrackingVisualizer from "../../components/BacktrackingVisualizer/BacktrackingVisualizer";
import PermutationsVisualizer from "../../components/PermutationsVisualizer/PermutationsVisualizer";
import CombinationSumVisualizer from "../../components/CombinationSumVisualizer/CombinationSumVisualizer";
import BitManipulatorVisualizer from "../../components/BitManipulatorVisualizer/BitManipulatorVisualizer";
import TreeVisualizer from "../../components/TreeVisualizer/TreeVisualizer";
import { algorithmRatings } from "../../config/ratings";
import { algorithmPseudocode } from "../../config/CodeSnippets";
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
  const [speed, setSpeed] = useState(500);
  const [isRunning, setIsRunning] = useState(false);
  const [isPaused, setIsPaused] = useState(false);
  const [infoOpen, setInfoOpen] = useState(false);
  const [showSecret, setShowSecret] = useState(false);
  const secretCanvasRef = useRef<HTMLCanvasElement>(null);

  const allAlgosForSecret = Object.values(allConfigs).flat();
  const learnedCountForSecret = allAlgosForSecret.filter((a) =>
    isLearned(`${a.category}/${a.slug}`),
  ).length;
  const prevLearnedRef = useRef(learnedCountForSecret);
  useEffect(() => {
    if (learnedCountForSecret >= 36 && prevLearnedRef.current < 36) {
      setShowSecret(true);
    }
    prevLearnedRef.current = learnedCountForSecret;
  }, [learnedCountForSecret]);

  useEffect(() => {
    if (!showSecret) return;
    const canvas = secretCanvasRef.current;
    if (!canvas) return;

    const dpr = window.devicePixelRatio || 1;
    const w = window.innerWidth;
    const h = window.innerHeight;
    canvas.width = w * dpr;
    canvas.height = h * dpr;
    canvas.style.width = `${w}px`;
    canvas.style.height = `${h}px`;

    const ctx = canvas.getContext("2d")!;
    ctx.scale(dpr, dpr);

    const FONT_SIZE = 18;
    const LINE_HEIGHT = 29;
    const FONT = `bold ${FONT_SIZE}px 'Segoe UI', Arial, sans-serif`;
    const TEXT = "ヾ(⌐■_■)ノ♪  ONE OF US!  ";
    ctx.font = FONT;
    const unitWidth = ctx.measureText(TEXT).width;
    const ROWS = Math.ceil(h / LINE_HEIGHT) + 1;

    const rows = Array.from({ length: ROWS }, (_, i) => ({
      x: -((i * unitWidth * 0.37) % unitWidth),
      speed: i % 2 === 0 ? 0.9 : -0.9,
      scanSpeed: 0.7 + (i % 5) * 0.18,
      phase: (i / ROWS) * (w + 400),
      row: i,
    }));

    let t = 0;
    let animId: number;

    const draw = () => {
      ctx.clearRect(0, 0, w, h);
      ctx.font = FONT;

      rows.forEach((row) => {
        row.x += row.speed;
        if (row.x > 0) row.x -= unitWidth;
        if (row.x < -unitWidth) row.x += unitWidth;

        const scanX = ((t * row.scanSpeed + row.phase) % (w + 500)) - 250;
        const grad = ctx.createLinearGradient(scanX - 300, 0, scanX + 300, 0);
        grad.addColorStop(0, "rgba(15, 50, 28, 0.7)");
        grad.addColorStop(0.5, "#4ade80");
        grad.addColorStop(1, "rgba(15, 50, 28, 0.7)");
        ctx.fillStyle = grad;

        const y = row.row * LINE_HEIGHT + FONT_SIZE;
        let x = row.x;
        while (x < w + unitWidth) {
          ctx.fillText(TEXT, x, y);
          x += unitWidth;
        }
      });

      t += 0.6;
      animId = requestAnimationFrame(draw);
    };

    draw();
    return () => cancelAnimationFrame(animId);
  }, [showSecret]);

  const [graphData, setGraphData] = useState<{
    nodeCount: number;
    edges: number[][];
  } | null>(null);
  const [treeInputValues, setTreeInputValues] = useState<number[]>([]);

  useLayoutEffect(() => {
    document.title = config
      ? `${config.name} - Algorithm Visualizer`
      : "Algorithm Visualizer";
  }, [config]);

  useEffect(() => {
    if (!config) return;
    setInput(config.defaultInput ?? "");
    setTarget(config.defaultTarget?.toString() ?? "");
    setWindowSize(config.defaultWindowSize?.toString() ?? "");
    setPattern(config.defaultPattern ?? "");
    setSteps([]);
    setError(null);
    setGraphData(null);
    setTreeInputValues([]);
  }, [category, slug]);

  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === "Enter") {
        if (isPaused) {
          setIsPaused(false);
          setIsRunning(true);
        } else if (!loading && !spamPrevention && !isRunning) {
          handleRun();
        }
      }
    };
    window.addEventListener("keydown", handleKeyDown);
    return () => window.removeEventListener("keydown", handleKeyDown);
  }, [loading, spamPrevention, isRunning, isPaused]);

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
        if (slug === "permutations" && numbers.length > 4) {
          setError(
            "Permutations are limited to 4 numbers - with 4 elements there are already 24 permutations to visualize.",
          );
          return;
        }
        if (slug === "subsets" && numbers.length > 20) {
          setError(
            "Subsets are limited to 12 elements - 2^12 produce alkready 4,096 subsets.",
          );
          return;
        }
        if (slug === "lis" && numbers.length > 30) {
          setError("LIS visualization is limited to 30 elements.");
          return;
        }
        if (slug === "knapsack" && numbers.length > 20) {
          setError("Knapsack is limited to 20 items.");
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
        if (slug === "palindrome-partitioning" && input.trim().length > 15) {
          setError(
            "Palindrome partitioning is limited to 15 characters - the number of partitions grows exponentially.",
          );
          return;
        }
        if (slug === "lcs" && input.trim().length > 25) {
          setError("LCS strings are each limited to 25 characters.");
          return;
        }
        if (slug === "lcs" && pattern.length > 25) {
          setError("LCS strings are each limited to 25 characters.");
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
        if (slug === "fibonacci" && num > 100) {
          setError(
            "Fibonacci is limited to n ≤ 100 to keep the visualization manageable.",
          );
          return;
        }
        if (slug === "climbing-stairs" && num > 50) {
          setError("Climbing stairs is limited to n ≤ 50.");
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

    if (config.needsTarget) {
      const t = parseInt(target, 10);
      if (!isNaN(t)) {
        if (slug === "coin-change" && t > 500) {
          setError("Coin change amount is limited to 500.");
          return;
        }
        if (slug === "knapsack" && t < 0) {
          setError("Knapsack capacity must be non-negative.");
          return;
        }
        if (slug === "knapsack" && t > 500) {
          setError("Knapsack capacity is limited to 500.");
          return;
        }
      }
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
      setIsRunning(true);
      setIsPaused(false);
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
      const msg =
        err instanceof Error ? err.message : "An unexpected error occurred.";
      setError(
        msg.includes("502")
          ? "Server is under maintenance, will be up shortly!"
          : msg,
      );
    }
    setLoading(false);
    setSpamPrevention(true);
    setTimeout(() => setSpamPrevention(false), 1000);
  };

  const learnedKey = `${category}/${slug}`;
  const allAlgos = Object.values(allConfigs).flat();
  const learnedCount = allAlgos.filter((a) =>
    isLearned(`${a.category}/${a.slug}`),
  ).length;
  const learnedTooltip =
    learnedCount == 36
      ? "(⌐■_■)   (▀̿Ĺ̯▀̿ ̿) I'm proud of you, came here a young Padawan and now you are a Jedi master! Use your knowledge wisely and spread it to yarning souls. May the algorithms be with you my friend "
      : learnedCount == 35
        ? "(╯°□°）╯︵ ┻━┻ Do you know what happends when you press this? Seriosuly, I GOT NO IDEA!!"
        : learnedCount == 34
          ? "(ﾉ◕ヮ◕)ﾉ*:･ﾟ✧The last few, you can do it! I BELEIVE IN YOU!"
          : learnedCount == 29
            ? "C'mon don't finish all of them, think of that series you liked watching, then it ended and it felt bad."
            : learnedCount == 24
              ? "I'm 100% convinced the user is a bot, activating safety measures!"
              : learnedCount == 19
                ? "What is this?! Are you even human?!?"
                : learnedCount == 14
                  ? "(⚆_⚆)You know you don't have to finish all of them right?"
                  : learnedCount == 9
                    ? "Ok, this one is showing some potential!"
                    : learnedCount == 4
                      ? "You really like those algorithms, don't you? :D"
                      : null;

  const inputControls = (
    <div className="input-row">
      <label>
        <span className="input-badge">{config.inputLabel ?? "Array"}</span>
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
          <span className="input-badge">{config.targetLabel ?? "Target"}</span>
          <input
            type="number"
            value={target}
            onChange={(e) => setTarget(e.target.value)}
          />
        </label>
      )}

      {config.needsPattern && (
        <label>
          <span className="input-badge">
            {config.patternLabel ?? "Pattern"}
          </span>
          <input
            type="text"
            value={pattern}
            onChange={(e) => setPattern(e.target.value)}
          />
        </label>
      )}

      {config.needsWindowSize && (
        <label>
          <span className="input-badge">Window size</span>
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
        <div className="algo-bar">
          <div className="algo-segment">
            <button
              className={`algo-seg-btn algo-info-burger${infoOpen ? " open" : ""}`}
              onClick={() => setInfoOpen((o) => !o)}
              aria-label="Toggle info"
            >
              ☰
            </button>

            <div className={`algo-info-pills${infoOpen ? " open" : ""}`}>
              <span className="algo-seg-wrap">
                <button className="algo-seg-btn">Explanation</button>
                <span className="algo-seg-tip algo-seg-tip--wide">
                  <p className="algo-tip-text">{config.description}</p>
                </span>
              </span>

              {algorithmPseudocode[slug] && (
                <span className="algo-seg-wrap">
                  <button className="algo-seg-btn">&lt;/Code&gt;</button>
                  <span className="algo-seg-tip algo-seg-tip--code">
                    <pre className="algo-code-pre">
                      {algorithmPseudocode[slug]}
                    </pre>
                  </span>
                </span>
              )}

              <span className="algo-seg-wrap">
                <button className="algo-seg-btn">Pros &amp; Cons</button>
                <span className="algo-seg-tip algo-seg-tip--proscons">
                  <div className="algo-tip-proscons">
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
                </span>
              </span>

              {algorithmRatings[slug] && (
                <span className="algo-seg-wrap">
                  <button className="algo-seg-btn">Rating</button>
                  <span className="algo-seg-tip algo-seg-tip--rating">
                    <span className="algo-code-shortcut">
                      <span className="algo-code-shortcut-label">
                        Usability:
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
                </span>
              )}
            </div>

            {config.ytTutorial && (
              <span className="algo-seg-wrap">
                <a
                  href={config.ytTutorial}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="algo-seg-btn algo-seg-yt"
                >
                  Youtube
                </a>
                <span className="algo-seg-tip algo-seg-tip--wide">
                  <p className="algo-tip-text">
                    This is the tutorial that helped me understand this
                    algorithm, if you need more details I highly encourage
                    watching and supporting the teacher with a like and a
                    subscribe &lt;3
                  </p>
                </span>
              </span>
            )}

            <span className="algo-seg-wrap">
              <label
                className={`algo-seg-btn algo-seg-learned${isLearned(learnedKey) ? " learned" : ""}`}
              >
                <input
                  type="checkbox"
                  checked={isLearned(learnedKey)}
                  onChange={() => toggle(learnedKey)}
                />
                {isLearned(learnedKey)
                  ? "Marked as learned"
                  : "Marked as learned"}
              </label>
              {learnedTooltip && !isLearned(learnedKey) && (
                <span className="algo-seg-tip algo-seg-tip--learned">
                  <p className="algo-tip-text">{learnedTooltip}</p>
                </span>
              )}
            </span>

            <label className="algo-seg-btn algo-speed-pill">
              <input
                type="range"
                min={50}
                max={1500}
                step={50}
                value={1550 - speed}
                onChange={(e) => setSpeed(1550 - Number(e.target.value))}
              />
            </label>

            <button
              className="algo-seg-btn algo-run-btn"
              onClick={
                isPaused
                  ? () => {
                      setIsPaused(false);
                      setIsRunning(true);
                    }
                  : handleRun
              }
              disabled={isRunning && !isPaused}
            >
              ▶
            </button>

            <button
              className="algo-seg-btn algo-pause-btn"
              onClick={() => {
                setIsPaused(true);
                setIsRunning(false);
              }}
              disabled={!isRunning}
            >
              ⏸
            </button>

            <button
              className="algo-seg-btn algo-clear-btn"
              onClick={() => {
                setSteps([]);
                setIsRunning(false);
                setIsPaused(false);
              }}
            >
              ✕
            </button>
          </div>
        </div>
      </div>

      {error && <p className="error">{error}</p>}

      {category === "pattern" && (
        <StringVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          inputControls={inputControls}
          speed={speed}
          isPaused={isPaused}
          onComplete={() => setIsRunning(false)}
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
          speed={speed}
          isPaused={isPaused}
          onComplete={() => setIsRunning(false)}
        />
      )}
      {category === "dp" && (
        <DPVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          inputControls={inputControls}
          speed={speed}
          isPaused={isPaused}
          onComplete={() => setIsRunning(false)}
        />
      )}
      {category === "backtracking" && slug === "permutations" && (
        <PermutationsVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          inputControls={inputControls}
          speed={speed}
          isPaused={isPaused}
          onComplete={() => setIsRunning(false)}
        />
      )}
      {category === "backtracking" && slug === "combination-sum" && (
        <CombinationSumVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          inputControls={inputControls}
          speed={speed}
          isPaused={isPaused}
          onComplete={() => setIsRunning(false)}
        />
      )}
      {category === "backtracking" &&
        slug !== "permutations" &&
        slug !== "combination-sum" && (
          <BacktrackingVisualizer
            steps={steps}
            onRun={handleRun}
            disabled={loading || spamPrevention}
            slug={slug}
            inputControls={inputControls}
            speed={speed}
            isPaused={isPaused}
            onComplete={() => setIsRunning(false)}
          />
        )}
      {category === "misc" && slug === "bit-manipulation" && (
        <BitManipulatorVisualizer steps={steps} inputControls={inputControls} />
      )}
      {category === "misc" && slug === "reversal" && (
        <StringVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          inputControls={inputControls}
          speed={speed}
          isPaused={isPaused}
          onComplete={() => setIsRunning(false)}
        />
      )}
      {category === "misc" && slug === "huffman" && (
        <HuffmanVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          inputControls={inputControls}
          speed={speed}
          isPaused={isPaused}
          onComplete={() => setIsRunning(false)}
        />
      )}
      {(category === "sort" || category === "find") && (
        <ArrayVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          inputControls={inputControls}
          speed={speed}
          isPaused={isPaused}
          onComplete={() => setIsRunning(false)}
        />
      )}
      {category === "tree" && (
        <TreeVisualizer
          steps={steps}
          onRun={handleRun}
          disabled={loading || spamPrevention}
          inputValues={treeInputValues}
          inputControls={inputControls}
          speed={speed}
          isPaused={isPaused}
          onComplete={() => setIsRunning(false)}
        />
      )}

      {showSecret && (
        <div className="secret-overlay" onClick={() => setShowSecret(false)}>
          <canvas ref={secretCanvasRef} className="secret-canvas" aria-hidden />
          <div className="secret-card" onClick={(e) => e.stopPropagation()}>
            <div className="secret-top-emoji">ヾ(⌐■_■)ノ♪</div>
            <h1 className="secret-title">ONE OF US!</h1>
            <p className="secret-chant">
              ONE OF US! &nbsp; ONE OF US! &nbsp; ONE OF US!
            </p>
            <p className="secret-body">
              You have completed all <strong>36 algorithms</strong> in the
              visualizer.
            </p>
            <p className="secret-body">
              You are now officially one of us, a true algorithm connoisseur, a
              Big O whisperer, a recursion survivor.
            </p>
            <p className="secret-body">
              No seriously. Are you okay? Have you slept? Have you been outside
              recently? I'm both
              <strong> impressed</strong> and <strong>concerned</strong>.
            </p>
            <p className="secret-body secret-love">
              Go touch some grass. You've more than earned it. ❤️
            </p>
            <button
              className="secret-close"
              onClick={() => setShowSecret(false)}
            >
              Return to reality
            </button>
          </div>
        </div>
      )}
    </div>
  );
}
