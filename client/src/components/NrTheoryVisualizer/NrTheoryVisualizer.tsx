import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./NrTheoryVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
  slug?: string;
}

/**
 * Number Theory visualizer.
 *  - Sieve: large bitmap grid (index = number, 1 = prime, 0 = composite)
 *  - Small arrays (GCD): labeled value cards
 *  - Growing lists (PrimeFactorization): sequential cells
 */
export default function NrTheoryVisualizer({ steps, onRun, disabled, slug }: Props) {
  return (
    <VisControls steps={steps} onRun={onRun} disabled={disabled}>
      {(step: AlgorithmStep) => {
        const arr = step.array;
        const hl = new Set(step.highlightIndices ?? []);
        const done = new Set(step.sortedIndices ?? []);

        // Sieve: large array where index = number
        if (slug === "sieve") {
          return <SieveGrid arr={arr} hl={hl} done={done} />;
        }


        // Bit Manipulation: show as bit-cards with binary representation
        if (slug === "bit-manipulation") {
          return (
            <div className="nt-vis">
              <div className="nt-cards">
                {arr.map((val, i) => {
                  let cls = "nt-card";
                  if (done.has(i)) cls += " done";
                  else if (hl.has(i)) cls += " active";
                  return (
                    <div key={i} className={cls}>
                      <span className="nt-card-val">{val}</span>
                      <span className="nt-card-bin">{(val >>> 0).toString(2)}</span>
                    </div>
                  );
                })}
              </div>
            </div>
          );
        }

        // Default: value cards
        return (
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
        );
      }}
    </VisControls>
  );
}

function SieveGrid({ arr, hl, done }: { arr: number[]; hl: Set<number>; done: Set<number> }) {
  return (
    <div className="nt-vis">
      <div className="sieve-grid">
        {arr.map((val, i) => {
          if (i < 2) return <div key={i} className="sieve-cell dim">{i}</div>;
          let cls = "sieve-cell";
          if (done.has(i)) cls += " prime";
          else if (hl.has(i)) cls += " crossing";
          else if (val === 0) cls += " composite";
          return (
            <div key={i} className={cls}>{i}</div>
          );
        })}
      </div>
    </div>
  );
}
