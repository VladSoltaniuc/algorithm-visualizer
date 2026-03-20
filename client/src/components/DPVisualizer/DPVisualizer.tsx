import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./DPVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
}

/**
 * Shows the 1-D DP table as a grid of cells.
 * Highlighted cells are the ones just updated; sorted cells are finalized.
 */
export default function DPVisualizer({ steps, onRun, disabled }: Props) {
  return (
    <VisControls steps={steps} onRun={onRun} disabled={disabled}>
      {(step: AlgorithmStep) => {
        const arr = step.array;
        const hl = new Set(step.highlightIndices ?? []);
        const done = new Set(step.sortedIndices ?? []);

        return (
          <div className="dp-vis">
            <div className="dp-header">
              {arr.map((_, i) => (
                <span key={i} className="dp-idx">
                  {i}
                </span>
              ))}
            </div>
            <div className="dp-row">
              {arr.map((val, i) => {
                let cls = "dp-cell";
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
        );
      }}
    </VisControls>
  );
}
