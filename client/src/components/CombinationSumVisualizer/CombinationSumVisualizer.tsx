import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./CombinationSumVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
  inputControls?: React.ReactNode;
  speed: number;
  isPaused?: boolean;
  onComplete?: () => void;
}

export default function CombinationSumVisualizer({
  steps,
  onRun: _onRun,
  disabled: _disabled,
  inputControls,
  speed,
  isPaused,
  onComplete,
}: Props) {
  const target = (() => {
    const m = steps[0]?.description?.match(/target=(\d+)/);
    return m ? parseInt(m[1], 10) : 0;
  })();

  const candidates = steps[0]?.array ?? [];

  return (
    <VisControls
      steps={steps}
      hideDescription
      inputControls={inputControls}
      speed={speed}
      isPaused={isPaused}
      onComplete={onComplete}
    >
      {(step: AlgorithmStep) => {
        const arr = step.array;
        const isInitStep = step.stepNumber === steps[0]?.stepNumber;
        const isFinalStep = step.stepNumber === steps[steps.length - 1]?.stepNumber;
        const isFound = !isInitStep && !isFinalStep && (step.sortedIndices?.length ?? 0) > 0;
        const lastAdded = isFound ? -1 : (step.highlightIndices?.[0] ?? -1);
        const showPath = !isInitStep && !isFinalStep;

        const currentSum = showPath ? arr.reduce((a, b) => a + b, 0) : 0;
        const need = target - currentSum;

        const foundCombos = steps
          .filter(
            (s) =>
              s.stepNumber <= step.stepNumber &&
              (s.sortedIndices?.length ?? 0) > 0,
          )
          .map((s) => s.array);

        let statusLabel: string;
        if (isFinalStep)
          statusLabel = `Done — ${foundCombos.length} solution${foundCombos.length !== 1 ? "s" : ""} found`;
        else if (isInitStep) statusLabel = "Ready";
        else if (isFound) statusLabel = "Solution found!";
        else statusLabel = "Exploring…";

        return (
          <>
            {/* ── Candidates reference ── */}
            <section className="cs-section">
              <p className="cs-section-title">
                Candidates&nbsp;·&nbsp;target&nbsp;=&nbsp;
                <span className="cs-target">{target}</span>
              </p>
              <div className="cs-candidates-row">
                {candidates.map((c, i) => (
                  <div key={i} className="cs-candidate">
                    {c}
                  </div>
                ))}
              </div>
            </section>

            {/* ── Current path ── */}
            <section className="cs-section">
              <p
                className={`cs-section-title${isFound ? " cs-section-title--found" : isFinalStep ? " cs-section-title--final" : ""}`}
              >
                {statusLabel}
              </p>

              <div className="cs-path">
                {!showPath || arr.length === 0 ? (
                  <div className="cs-path-empty">[ ]</div>
                ) : (
                  arr.map((val, i) => (
                    <div
                      key={i}
                      className={`cs-cell${
                        isFound
                          ? " cs-cell--found"
                          : i === lastAdded
                            ? " cs-cell--active"
                            : " cs-cell--used"
                      }`}
                    >
                      {val}
                    </div>
                  ))
                )}
              </div>

              {showPath && (
                <div className="cs-sum-row">
                  <span className="cs-sum-label">sum</span>
                  <span
                    className={`cs-sum-val${isFound ? " cs-sum-val--found" : ""}`}
                  >
                    {currentSum}
                  </span>
                  <span className="cs-sum-sep">/</span>
                  <span className="cs-sum-label">target</span>
                  <span className="cs-sum-val">{target}</span>
                  {!isFound && need > 0 && (
                    <>
                      <span className="cs-sum-sep">·</span>
                      <span className="cs-need">need {need} more</span>
                    </>
                  )}
                </div>
              )}

              <p
                className={`cs-desc${isFound ? " cs-desc--found" : ""}`}
              >
                {step.description}
              </p>
            </section>

            {/* ── Found solutions ── */}
            <section className="cs-section">
              <p className="cs-section-title">
                Solutions found&nbsp;
                <span className="cs-count">{foundCombos.length}</span>
              </p>
              {foundCombos.length > 0 ? (
                <div className="cs-found-grid">
                  {foundCombos.map((combo, ci) => (
                    <div
                      key={ci}
                      className={`cs-found-item${
                        ci === foundCombos.length - 1 && isFound
                          ? " cs-found-item--latest"
                          : ""
                      }`}
                    >
                      <span className="cs-found-numbers">
                        {combo.join(" + ")}
                      </span>
                      <span className="cs-found-eq">=&nbsp;{target}</span>
                    </div>
                  ))}
                </div>
              ) : (
                <p className="cs-found-empty">None yet</p>
              )}
            </section>
          </>
        );
      }}
    </VisControls>
  );
}
