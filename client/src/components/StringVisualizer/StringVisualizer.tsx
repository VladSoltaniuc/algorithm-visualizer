import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./StringVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
}

export default function StringVisualizer({ steps, onRun, disabled }: Props) {
  return (
    <VisControls steps={steps} onRun={onRun} disabled={disabled}>
      {(step: AlgorithmStep) => {
        const chars = step.array.map((code) => String.fromCharCode(code));
        const hl = new Set(step.highlightIndices ?? []);
        const matched = new Set(step.sortedIndices ?? []);

        return (
          <div className="string-vis">
            <div className="string-chars">
              {chars.map((ch, i) => {
                let cls = "str-cell";
                if (matched.has(i)) cls += " matched";
                else if (hl.has(i)) cls += " active";
                return (
                  <div key={i} className={cls}>
                    <span className="str-char">{ch}</span>
                    <span className="str-idx">{i}</span>
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
