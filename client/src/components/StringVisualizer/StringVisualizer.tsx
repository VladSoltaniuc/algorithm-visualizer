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

        const hasHash =
          step.textHash !== undefined &&
          step.textHash !== null &&
          step.patternHash !== undefined &&
          step.patternHash !== null;
        const hashesEqual = hasHash && step.textHash === step.patternHash;

        const isManacher =
          step.pArray !== undefined && step.pArray !== null && step.pArray.length > 0;
        const mCenter = step.manacherCenter ?? -1;
        const mRight = step.manacherRight ?? -1;

        const hasDpMatrix =
          step.dpMatrix !== undefined &&
          step.dpMatrix !== null &&
          step.dpMatrix.length > 0;

        if (hasDpMatrix) {
          const matrix = step.dpMatrix!;
          const rowHdr = step.rowHeaders ?? "";
          const colHdr = step.colHeaders ?? "";
          const hlRow = step.highlightRow ?? -1;
          const hlCol = step.highlightCol ?? -1;
          const bp = step.backtrackPath ?? [];
          const bpSet = new Set<string>();
          for (let k = 0; k < bp.length; k += 2) bpSet.add(`${bp[k]},${bp[k + 1]}`);

          return (
            <div className="string-vis">
              <div className="lcs-matrix-wrap">
                <table className="lcs-matrix">
                  <thead>
                    <tr>
                      <th className="lcs-corner"></th>
                      {[...colHdr].map((ch, j) => (
                        <th key={j} className="lcs-col-hdr">{ch === " " ? "\u2205" : ch}</th>
                      ))}
                    </tr>
                  </thead>
                  <tbody>
                    {matrix.map((row, i) => (
                      <tr key={i}>
                        <th className="lcs-row-hdr">
                          {i < rowHdr.length ? (rowHdr[i] === " " ? "\u2205" : rowHdr[i]) : i}
                        </th>
                        {row.map((val, j) => {
                          let cls = "lcs-cell";
                          if (bpSet.has(`${i},${j}`)) cls += " backtrack";
                          else if (i === hlRow && j === hlCol) cls += " active";
                          return <td key={j} className={cls}>{val}</td>;
                        })}
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          );
        }

        return (
          <div className="string-vis">
            {hasHash && (
              <div className={`hash-comparison ${hashesEqual ? "hash-match" : "hash-mismatch"}`}>
                <span className="hash-item">
                  <span className="hash-label">Pattern Hash</span>
                  <span className="hash-value">{step.patternHash}</span>
                </span>
                <span className={`hash-indicator ${hashesEqual ? "match" : "mismatch"}`}>
                  {hashesEqual ? "=" : "≠"}
                </span>
                <span className="hash-item">
                  <span className="hash-label">Window Hash</span>
                  <span className="hash-value">{step.textHash}</span>
                </span>
              </div>
            )}
            {isManacher && mCenter >= 0 && (
              <div className="manacher-legend">
                <span className="legend-item"><span className="legend-swatch center-swatch" />C = {mCenter}</span>
                <span className="legend-item"><span className="legend-swatch right-swatch" />R = {mRight}</span>
              </div>
            )}
            {patternChars && patternOffset >= 0 && (
              <span className="row-label">Text</span>
            )}
            {isManacher && <span className="row-label">Transformed</span>}
            <div className={`string-chars${patternChars && patternOffset >= 0 ? " has-pattern" : ""}`}>
              {chars.map((ch, i) => {
                let cls = "str-cell";
                if (matched.has(i)) cls += " matched";
                else if (hl.has(i)) cls += " active";
                if (isManacher && i === mCenter) cls += " manacher-center";
                if (isManacher && i === mRight) cls += " manacher-right";
                return (
                  <div key={i} className={cls}>
                    <span className="str-char">{ch}</span>
                    <span className="str-idx">{i}</span>
                  </div>
                );
              })}
            </div>
            {isManacher && (
              <>
                <span className="row-label">P (radius)</span>
                <div className="string-chars">
                  {step.pArray!.map((val, i) => {
                    let cls = "str-cell p-cell";
                    if (hl.has(i)) cls += " active";
                    return (
                      <div key={`p-${i}`} className={cls}>
                        <span className="str-char">{val}</span>
                        <span className="str-idx">{i}</span>
                      </div>
                    );
                  })}
                </div>
              </>            
            )}
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
