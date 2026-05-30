import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./BacktrackingVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
  slug?: string;
  inputControls?: React.ReactNode;
  speed: number;
  isPaused?: boolean;
  onComplete?: () => void;
}

export default function BacktrackingVisualizer({
  steps,
  onRun: _onRun,
  disabled: _disabled,
  slug: _slug,
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
        const hl = new Set(step.highlightIndices ?? []);
        const done = new Set(step.sortedIndices ?? []);
        const isFinal = step.stepNumber === steps[steps.length - 1]?.stepNumber;

        // Default linear display for permutations, subsets, combos, etc.
        return (
          <>
            <div className="bt-vis">
              <div className="bt-linear">
                {arr.map((val, i) => {
                  let cls = "bt-cell";
                  if (done.has(i)) cls += " done";
                  else if (hl.has(i)) cls += " active";
                  return (
                    <div key={i} className={cls}>
                      {val}
                    </div>
                  );
                })}
              </div>
            </div>
            <div className={`step-info${isFinal ? " final" : ""}`}>
              {step.description}
            </div>
          </>
        );
      }}
    </VisControls>
  );
}

