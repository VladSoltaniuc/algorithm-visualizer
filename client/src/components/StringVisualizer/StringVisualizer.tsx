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
        const isNumeric = step.isNumericArray ?? false;
        const chars = isNumeric
          ? step.array.map((v) => String(v))
          : step.array.map((code) => String.fromCharCode(code));
        const hl = new Set(step.highlightIndices ?? []);
        const matched = new Set(step.sortedIndices ?? []);

        const patternChars =
          step.patternArray && step.patternArray.length > 0
            ? step.patternArray.map((code) => String.fromCharCode(code))
            : null;
        const patternOffset = step.patternOffset ?? -1;
        const patternHl = step.patternHighlightIndex ?? -1;

        return (
          <div className="string-vis">
            {patternChars && patternOffset >= 0 && (
              <span className="row-label">Text</span>
            )}
            <div className={`string-chars${patternChars && patternOffset >= 0 ? " has-pattern" : ""}`}>
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
            {patternChars && patternOffset >= 0 && (
              <>
                <span className="row-label">Pattern</span>
                <div className="string-chars pattern-row">
                  {Array.from({ length: patternOffset }, (_, i) => (
                    <div key={`spacer-${i}`} className="str-cell spacer" />
                  ))}
                  {patternChars.map((ch, i) => {
                    let cls = "str-cell pattern-cell";
                    if (matched.size > 0 && matched.has(patternOffset + i))
                      cls += " matched";
                    else if (i === patternHl) cls += " active";
                    return (
                      <div key={`pat-${i}`} className={cls}>
                        <span className="str-char">{ch}</span>
                        <span className="str-idx">{i}</span>
                      </div>
                    );
                  })}
                </div>
              </>
            )}
          </div>
        );
      }}
    </VisControls>
  );
}
