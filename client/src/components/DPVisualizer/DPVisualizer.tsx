import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./DPVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
  inputControls?: React.ReactNode;
}

/**
 * Shows the 1-D DP table as a grid of cells.
 * Highlighted cells are the ones just updated; sorted cells are finalized.
 */
export default function DPVisualizer({
  steps,
  onRun,
  disabled,
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
        const notes = step.notes;
        const isFinalStep =
          steps.length > 0 &&
          step.stepNumber === steps[steps.length - 1].stepNumber;

        const hasDpMatrix =
          step.dpMatrix !== undefined &&
          step.dpMatrix !== null &&
          step.dpMatrix.length > 0;

        if (hasDpMatrix) {
          const matrix = step.dpMatrix!;
          const rowHdr = step.rowHeaders ?? "";
          const colHdr = step.colHeaders ?? "";
          const rowLbls = step.rowLabels;
          const colLbls = step.colLabels;
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
                      {colLbls
                        ? colLbls.map((lbl, j) => (
                            <th key={j} className="lcs-col-hdr">
                              {lbl}
                            </th>
                          ))
                        : [...colHdr].map((ch, j) => (
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
                          {rowLbls
                            ? (rowLbls[i] ?? i)
                            : i < rowHdr.length
                              ? rowHdr[i] === " "
                                ? "\u2205"
                                : rowHdr[i]
                              : i}
                        </th>
                        {row.map((val, j) => {
                          let cls = "lcs-cell";
                          if (bpSet.has(`${i},${j}`)) cls += " backtrack";
                          else if (i === hlRow && j === hlCol) cls += " active";
                          else if (val === -1) cls += " na";
                          return (
                            <td key={j} className={cls}>
                              {val === -1 ? "\u2013" : val}
                            </td>
                          );
                        })}
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
              <div className={`step-info${isFinalStep ? " final" : ""}`}>
                {step.description}
              </div>
            </div>
          );
        }

        // ── Coin Change ─────────────────────────────────────────────────────
        // Detected by the presence of the "coin used" labels array.
        // patternOffset = lookup cell (dp[i - coin]); highlighted in blue.
        // On the final step sortedIndices covers all cells and highlightIndices
        // contains the backtrack path.
        if (step.labels && step.labels.length > 0) {
          const labels = step.labels;
          // During "try coin" steps hl has exactly one element (the current amount).
          // On the final step hl contains the backtrack path.
          const isAllDone = (step.sortedIndices?.length ?? 0) === arr.length;
          const activeIdx = isAllDone || hl.size !== 1 ? -1 : [...hl][0];
          const lookupIdx = isAllDone ? -1 : (step.patternOffset ?? -1);
          const pathSet = new Set<number>(isAllDone ? [...hl] : []);

          return (
            <div className="dp-vis cc-vis">
              {/* Scrollable table area */}
              <div className="cc-table-scroll">
                {/* Index header */}
                <div className="dp-header">
                  {arr.map((_, i) => (
                    <span key={i} className="dp-idx">
                      {i}
                    </span>
                  ))}
                </div>

                {/* dp values row */}
                <div className="dp-row">
                  {arr.map((_, i) => {
                    let cls = "dp-cell";
                    if (!isAllDone && i === activeIdx) cls += " active";
                    else if (!isAllDone && lookupIdx >= 0 && i === lookupIdx)
                      cls += " lookup";
                    else if (isAllDone && pathSet.has(i)) cls += " path";
                    else if (done.has(i)) cls += " done";
                    return (
                      <div key={i} className={cls}>
                        {notes?.[i] ?? String(arr[i])}
                      </div>
                    );
                  })}
                </div>

                {/* Coin-used row */}
                <div className="cc-row-title">coin used:</div>
                <div className="dp-row cc-coin-row">
                  {labels.map((lbl, i) => {
                    let cls = "dp-cell cc-coin-cell";
                    if (isAllDone && pathSet.has(i)) cls += " path";
                    else if (done.has(i) && lbl !== "") cls += " done";
                    return (
                      <div key={i} className={cls}>
                        {lbl}
                      </div>
                    );
                  })}
                </div>
              </div>
              <div className={`step-info${isFinalStep ? " final" : ""}`}>
                {step.description}
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
            <div className={`step-info${isFinalStep ? " final" : ""}`}>
              {step.description}
            </div>
          </div>
        );
      }}
    </VisControls>
  );
}
