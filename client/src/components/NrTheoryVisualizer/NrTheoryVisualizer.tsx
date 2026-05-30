import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./NrTheoryVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
  slug?: string;
  inputControls?: React.ReactNode;
  speed: number;
  isPaused?: boolean;
  onComplete?: () => void;
}

export default function NrTheoryVisualizer({
  steps,
  onRun: _onRun,
  disabled: _disabled,
  slug,
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
        const isFinal = step.stepNumber === steps[steps.length - 1]?.stepNumber;

        if (slug === "bit-manipulation") {
          const desc = step.description;

          const step0Bits: number[] = steps[0]?.array ?? arr;
          const bitWidth = Math.max(step0Bits.length, 1);
          const n = parseInt(step0Bits.join("") || "0", 2);

          const toBits = (val: number): number[] => {
            const safe = val < 0 ? (val >>> 0) & ((1 << bitWidth) - 1) : val;
            return safe
              .toString(2)
              .padStart(bitWidth, "0")
              .split("")
              .map(Number);
          };

          const bitCls = (
            bit: number,
            i: number,
            hlSet?: Set<number>,
            rowCls?: string,
          ): string => {
            if (rowCls) return `bm-bit bm-${rowCls}`;
            if (hlSet?.has(i)) return "bm-bit bm-active";
            return bit === 1 ? "bm-bit bm-one" : "bm-bit bm-zero";
          };

          const renderBitRow = (
            label: string,
            bits: number[],
            decimal: number,
            hlSet?: Set<number>,
            rowCls?: string,
          ) => (
            <div className="bm-row">
              <span className="bm-label">{label}</span>
              <div className="bm-bits">
                {bits.map((bit, i) => (
                  <div key={i} className={bitCls(bit, i, hlSet, rowCls)}>
                    {bit}
                  </div>
                ))}
              </div>
              <span className="bm-decimal">= {decimal}</span>
            </div>
          );

          const renderPosRow = () => (
            <div className="bm-row bm-pos-row">
              <span className="bm-label" />
              <div className="bm-bits">
                {Array.from({ length: bitWidth }, (_, i) => (
                  <div key={i} className="bm-pos">
                    {bitWidth - 1 - i}
                  </div>
                ))}
              </div>
              <span className="bm-decimal" />
            </div>
          );

          const renderOp = (symbol: string) => (
            <div className="bm-op-row">
              <span className="bm-label">{symbol}</span>
            </div>
          );

          const isAnd = desc.includes("& (n-1)");
          const isXor = desc.includes("XOR");
          const isNot = desc.includes("NOT n");
          const isCount = desc.includes("Set bits");

          if (isAnd) {
            const valN = arr[0],
              valNm1 = arr[1],
              valResult = arr[2];
            const isPow2 = valResult === 0 && valN > 0;
            return (
              <>
                <div className="bm-vis">
                  {renderPosRow()}
                  {renderBitRow("n", toBits(valN), valN)}
                  {renderOp("&")}
                  {renderBitRow("n - 1", toBits(valNm1), valNm1)}
                  <div className="bm-divider" />
                  {renderBitRow(
                    "result",
                    toBits(valResult),
                    valResult,
                    undefined,
                    isPow2 ? "done" : "active",
                  )}
                  <div className={`bm-verdict ${isPow2 ? "bm-yes" : "bm-no"}`}>
                    {isPow2
                      ? `✓ ${n} is a power of 2`
                      : `✗ ${n} is not a power of 2`}
                  </div>
                </div>
                <div className={`step-info${isFinal ? " final" : ""}`}>
                  {desc}
                </div>
              </>
            );
          }

          if (isXor) {
            const valA = arr[0],
              valResult = arr[2];
            return (
              <>
                <div className="bm-vis">
                  {renderPosRow()}
                  {renderBitRow("n", toBits(valA), valA)}
                  {renderOp("⊕")}
                  {renderBitRow("n", toBits(valA), valA)}
                  <div className="bm-divider" />
                  {renderBitRow(
                    "result",
                    toBits(valResult),
                    valResult,
                    undefined,
                    "done",
                  )}
                </div>
                <div className={`step-info${isFinal ? " final" : ""}`}>
                  Any value XOR'd with itself always cancels to 0
                </div>
              </>
            );
          }

          if (isNot) {
            const flippedVal = parseInt(arr.join("") || "0", 2);
            return (
              <>
                <div className="bm-vis">
                  {renderPosRow()}
                  {renderBitRow("n", step0Bits, n)}
                  {renderOp("~")}
                  {renderBitRow("~n", arr, flippedVal, undefined, "done")}
                </div>
                <div className={`step-info${isFinal ? " final" : ""}`}>
                  {desc}
                </div>
              </>
            );
          }

          // Single-row: initial binary display or set-bits count
          const decimal = parseInt(arr.join("") || "0", 2);
          return (
            <>
              <div className="bm-vis">
                {renderPosRow()}
                {renderBitRow("n", arr, decimal, isCount ? hl : undefined)}
                {isCount && (
                  <div className="bm-badge">
                    {arr.filter((b) => b === 1).length} set bit
                    {arr.filter((b) => b === 1).length !== 1 ? "s" : ""}
                  </div>
                )}
              </div>
              <div className={`step-info${isFinal ? " final" : ""}`}>
                {desc}
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
