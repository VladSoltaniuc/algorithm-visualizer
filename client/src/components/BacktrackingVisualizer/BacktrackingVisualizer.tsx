import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./BacktrackingVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
  slug?: string;
  inputControls?: React.ReactNode;
}

export default function BacktrackingVisualizer({
  steps,
  onRun,
  disabled,
  slug,
  inputControls,
}: Props) {
  return (
    <VisControls
      steps={steps}
      onRun={onRun}
      disabled={disabled}
      hideDescription
      inputControls={inputControls}
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

