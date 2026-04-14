import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./BacktrackingVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
  slug?: string;
}

/**
 * Renders board/grid state for backtracking algorithms.
 * Detects the grid shape from the array length and slug to choose the right layout:
 *  - N-Queens: arr[row] = col of queen  →  N×N chessboard
 *  - Sudoku: 81 cells  →  9×9 grid
 *  - Rat: n² cells  →  n×n grid
 *  - Others (permutations, subsets, combos): linear cell row
 */
export default function BacktrackingVisualizer({
  steps,
  onRun,
  disabled,
  slug,
}: Props) {
  return (
    <VisControls
      steps={steps}
      onRun={onRun}
      disabled={disabled}
      hideDescription
    >
      {(step: AlgorithmStep) => {
        const arr = step.array;
        const hl = new Set(step.highlightIndices ?? []);
        const done = new Set(step.sortedIndices ?? []);
        const isFinal = step.stepNumber === steps[steps.length - 1]?.stepNumber;

        // N-Queens: array length = n, values are column positions (-1 = empty)
        if (slug === "n-queens") {
          return (
            <>
              <NQueensBoard queens={arr} hl={hl} done={done} />
              <div className={`step-info${isFinal ? " final" : ""}`}>
                {step.description}
              </div>
            </>
          );
        }

        // Sudoku: exactly 81 cells  →  9×9
        if (arr.length === 81 && slug === "sudoku") {
          return (
            <>
              <GridBoard arr={arr} cols={9} hl={hl} done={done} />
              <div className={`step-info${isFinal ? " final" : ""}`}>
                {step.description}
              </div>
            </>
          );
        }

        // Square grids (rat-in-maze)
        const sqrt = Math.round(Math.sqrt(arr.length));
        if (
          sqrt * sqrt === arr.length &&
          arr.length > 1 &&
          slug === "rat-in-maze"
        ) {
          return (
            <>
              <GridBoard arr={arr} cols={sqrt} hl={hl} done={done} />
              <div className={`step-info${isFinal ? " final" : ""}`}>
                {step.description}
              </div>
            </>
          );
        }

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

function NQueensBoard({
  queens,
  hl,
  done,
}: {
  queens: number[];
  hl: Set<number>;
  done: Set<number>;
}) {
  const n = queens.length;
  return (
    <div className="bt-vis">
      <div
        className="bt-board"
        style={{ gridTemplateColumns: `repeat(${n}, 1fr)` }}
      >
        {Array.from({ length: n * n }, (_, idx) => {
          const row = Math.floor(idx / n);
          const col = idx % n;
          const isQueen = queens[row] === col;
          const isDark = (row + col) % 2 === 1;
          let cls = `bt-square${isDark ? " dark" : ""}`;
          if (isQueen && done.has(row)) cls += " done";
          else if (isQueen && hl.has(row)) cls += " active";
          else if (isQueen) cls += " queen";
          return (
            <div key={idx} className={cls}>
              {isQueen ? "♛" : ""}
            </div>
          );
        })}
      </div>
    </div>
  );
}

function GridBoard({
  arr,
  cols,
  hl,
  done,
}: {
  arr: number[];
  cols: number;
  hl: Set<number>;
  done: Set<number>;
}) {
  return (
    <div className="bt-vis">
      <div
        className="bt-board"
        style={{ gridTemplateColumns: `repeat(${cols}, 1fr)` }}
      >
        {arr.map((val, i) => {
          let cls = "bt-grid-cell";
          if (done.has(i)) cls += " done";
          else if (hl.has(i)) cls += " active";
          else if (val !== 0) cls += " filled";
          // Sudoku 3×3 box borders
          if (cols === 9) {
            if (i % 9 !== 0 && i % 3 === 0) cls += " box-left";
            const row = Math.floor(i / 9);
            if (row !== 0 && row % 3 === 0 && i % 9 < 9) cls += " box-top";
          }
          return (
            <div key={i} className={cls}>
              {val !== 0 ? val : ""}
            </div>
          );
        })}
      </div>
    </div>
  );
}
