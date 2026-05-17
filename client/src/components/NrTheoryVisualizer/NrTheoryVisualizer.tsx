import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./NrTheoryVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
  slug?: string;
  inputControls?: React.ReactNode;
}

export default function NrTheoryVisualizer({
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

        // Bit Manipulation: show as bit-cards with binary representation
        if (slug === "bit-manipulation") {
          return (
            <>
              <div className="nt-vis">
                <div className="nt-cards">
                  {arr.map((val, i) => {
                    let cls = "nt-card";
                    if (done.has(i)) cls += " done";
                    else if (hl.has(i)) cls += " active";
                    return (
                      <div key={i} className={cls}>
                        <span className="nt-card-val">{val}</span>
                        <span className="nt-card-bin">
                          {(val >>> 0).toString(2)}
                        </span>
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
        }

        // Default: value cards
        return (
          <>
            <div className="nt-vis">
              <div className="nt-cards">
                {arr.map((val, i) => {
                  let cls = "nt-card";
                  if (done.has(i)) cls += " done";
                  else if (hl.has(i)) cls += " active";
                  return (
                    <div key={i} className={cls}>
                      <span className="nt-card-val">{val}</span>
                      <span className="nt-card-idx">{i}</span>
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

