import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./DPVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
  inputControls?: React.ReactNode;
  speed: number;
  isPaused?: boolean;
  onComplete?: () => void;
}

/**
 * Shows the 1-D DP table as a grid of cells.
 * Highlighted cells are the ones just updated; sorted cells are finalized.
 */
export default function DPVisualizer({
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
        const COLS = 7;

        if (step.labels && step.labels.length > 0) {
          const labels = step.labels;
          const isAllDone = (step.sortedIndices?.length ?? 0) === arr.length;
          const activeIdx = isAllDone || hl.size !== 1 ? -1 : [...hl][0];
          const lookupIdx = isAllDone ? -1 : (step.patternOffset ?? -1);
          const pathSet = new Set<number>(isAllDone ? [...hl] : []);

          const chunks: number[][] = [];
          for (let i = 0; i < arr.length; i += COLS)
            chunks.push(arr.slice(i, i + COLS).map((_, j) => i + j));

          return (
            <div className="dp-vis cc-vis">
              <div className="dp-matrix-center">
              <div className="dp-matrix">
                {chunks.map((chunk, rowIdx) => (
                  <div key={rowIdx} className="dp-matrix-row">
                    <div className="dp-header">
                      {chunk.map((i) => (
                        <span key={i} className="dp-idx">{i}</span>
                      ))}
                    </div>
                    <div className="dp-row">
                      {chunk.map((i) => {
                        let cls = "dp-cell";
                        const isImpossible = labels[i] === "✗";
                        if (!isAllDone && i === activeIdx) cls += " active";
                        else if (!isAllDone && lookupIdx >= 0 && i === lookupIdx) cls += " lookup";
                        else if (isAllDone && i > 0 && pathSet.has(i)) cls += " path";
                        else if (isImpossible) cls += " impossible";
                        else if (done.has(i)) cls += " done";
                        const badgeNum = isAllDone && i > 0 && pathSet.has(i) && labels[i]?.startsWith("+")
                          ? labels[i].slice(1)
                          : null;
                        return (
                          <div key={i} className={cls} style={{ position: "relative" }}>
                            {notes?.[i] ?? String(arr[i])}
                            {badgeNum && (
                              <span className="cc-badge">+{badgeNum}</span>
                            )}
                          </div>
                        );
                      })}
                    </div>
                  </div>
                ))}
              </div>
              </div>
              <div className={`step-info${isFinalStep ? " final" : ""}`}>
                {step.description}
              </div>
            </div>
          );
        }

        const chunks: number[][] = [];
        for (let i = 0; i < arr.length; i += COLS)
          chunks.push(arr.slice(i, i + COLS).map((_, j) => i + j));

        return (
          <div className="dp-vis">
            <div className="dp-matrix-center">
            <div className="dp-matrix">
              {chunks.map((chunk, rowIdx) => (
                <div key={rowIdx} className="dp-matrix-row">
                  <div className="dp-header">
                    {chunk.map((i) => (
                      <span key={i} className="dp-idx">{i}</span>
                    ))}
                  </div>
                  <div className="dp-row">
                    {chunk.map((i) => {
                      let cls = "dp-cell";
                      if (done.has(i)) cls += " done";
                      else if (hl.has(i)) cls += " active";
                      return (
                        <div key={i} className={cls}>
                          {notes?.[i] ?? String(arr[i])}
                        </div>
                      );
                    })}
                  </div>
                </div>
              ))}
            </div>
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
