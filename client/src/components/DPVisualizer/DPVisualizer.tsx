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
        const notes = step.notes;

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
          for (let k = 0; k < bp.length; k += 2)
            bpSet.add(`${bp[k]},${bp[k + 1]}`);

          return (
            <div className="dp-vis">
              <div className="lcs-matrix-wrap">
                <table className="lcs-matrix">
                  <thead>
                    <tr>
                      <th className="lcs-corner"></th>
                      {[...colHdr].map((ch, j) => (
                        <th key={j} className="lcs-col-hdr">
                          {ch === " " ? "\u2205" : ch}
                        </th>
                      ))}
                    </tr>
                  </thead>
                  <tbody>
                    {matrix.map((row, i) => (
                      <tr key={i}>
                        <th className="lcs-row-hdr">
                          {i < rowHdr.length
                            ? rowHdr[i] === " "
                              ? "\u2205"
                              : rowHdr[i]
                            : i}
                        </th>
                        {row.map((val, j) => {
                          let cls = "lcs-cell";
                          if (bpSet.has(`${i},${j}`)) cls += " backtrack";
                          else if (i === hlRow && j === hlCol) cls += " active";
                          return (
                            <td key={j} className={cls}>
                              {val}
                            </td>
                          );
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
                    {notes?.[i] ?? String(val)}
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
