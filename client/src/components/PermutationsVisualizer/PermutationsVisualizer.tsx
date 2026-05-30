import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./PermutationsVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
  inputControls?: React.ReactNode;
  speed: number;
  isPaused?: boolean;
  onComplete?: () => void;
}

function factorial(n: number): number {
  let r = 1;
  for (let i = 2; i <= n; i++) r *= i;
  return r;
}

export default function PermutationsVisualizer({
  steps,
  onRun: _onRun,
  disabled: _disabled,
  inputControls,
  speed,
  isPaused,
  onComplete,
}: Props) {
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
        const n = steps[0]?.array?.length ?? arr.length;
        const hl = step.highlightIndices ?? [];
        const isPermStep = (step.sortedIndices?.length ?? 0) > 0;
        const isBacktrack = step.notes?.[0] === "backtrack";
        const isPlaceStep = hl.length > 0 && !isPermStep;
        const depth: number = step.backtrackPath?.[0] ?? 0;

        const foundPerms = steps
          .filter(
            (s) =>
              s.stepNumber <= step.stepNumber &&
              (s.sortedIndices?.length ?? 0) > 0,
          )
          .map((s) => s.array);

        const total = factorial(n);

        // ── cell classification ──────────────────────────────
        type CellState = "locked" | "placing" | "swap" | "backtrack" | "done" | "open";
        function cellState(i: number): CellState {
          if (isPermStep) return "done";
          if (isPlaceStep) {
            if (i === hl[0]) return isBacktrack ? "backtrack" : "placing";
            if (hl[0] !== hl[1] && i === hl[1]) return isBacktrack ? "backtrack" : "swap";
            if (i < depth) return "locked";
            return "open";
          }
          return "open";
        }

        const stateLabel: Record<CellState, string> = {
          locked: "locked",
          placing: "placing",
          swap: "source",
          backtrack: "undo",
          done: "fixed",
          open: "open",
        };

        return (
          <>
            {/* ── current array ── */}
            <section className="perm-section">
              <p className="perm-section-title">Current State</p>
              <div className="perm-array-wrap">
                {/* position row */}
                <div className="perm-row perm-pos-row">
                  {arr.map((_, i) => (
                    <div key={i} className="perm-pos">
                      pos {i}
                    </div>
                  ))}
                </div>
                {/* value cells */}
                <div className="perm-row">
                  {arr.map((val, i) => {
                    const s = cellState(i);
                    return (
                      <div key={i} className={`perm-cell perm-cell--${s}`}>
                        {val}
                      </div>
                    );
                  })}
                </div>
                {/* state labels */}
                <div className="perm-row perm-state-row">
                  {arr.map((_, i) => {
                    const s = cellState(i);
                    return (
                      <div key={i} className={`perm-state-label perm-state--${s}`}>
                        {stateLabel[s]}
                      </div>
                    );
                  })}
                </div>
              </div>

              {/* depth progress */}
              {isPlaceStep && (
                <div className="perm-depth">
                  <span className="perm-depth-label">
                    Filling position {depth} of {n - 1}
                  </span>
                  <div className="perm-depth-track">
                    {Array.from({ length: n }, (_, i) => (
                      <div
                        key={i}
                        className={`perm-depth-pip ${
                          i < depth
                            ? "perm-depth-pip--done"
                            : i === depth
                              ? "perm-depth-pip--active"
                              : "perm-depth-pip--open"
                        }`}
                      />
                    ))}
                  </div>
                </div>
              )}

              <p className={`perm-desc${isPermStep ? " perm-desc--found" : isBacktrack ? " perm-desc--back" : ""}`}>
                {step.description}
              </p>
            </section>

            {/* ── found permutations ── */}
            <section className="perm-section">
              <p className="perm-section-title">
                Found&nbsp;
                <span className="perm-count">{foundPerms.length}</span>
                &nbsp;of&nbsp;{total}
              </p>
              {foundPerms.length > 0 ? (
                <div className="perm-found-grid">
                  {foundPerms.map((perm, pi) => (
                    <div
                      key={pi}
                      className={`perm-found-item${pi === foundPerms.length - 1 ? " perm-found-item--latest" : ""}`}
                    >
                      {perm.map((v, vi) => (
                        <span key={vi} className="perm-found-val">
                          {v}
                        </span>
                      ))}
                    </div>
                  ))}
                </div>
              ) : (
                <p className="perm-found-empty">None yet</p>
              )}
            </section>
          </>
        );
      }}
    </VisControls>
  );
}
