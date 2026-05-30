import type { AlgorithmStep } from "../../types";
import "./BitManipulatorVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  inputControls?: React.ReactNode;
}

function toBits(val: number): number[] {
  return ((val >>> 0) & 0xff)
    .toString(2)
    .padStart(8, "0")
    .split("")
    .map(Number);
}

function PosRow() {
  return (
    <div className="bmv-row bmv-pos-row">
      <span className="bmv-label" />
      <div className="bmv-bits">
        {Array.from({ length: 8 }, (_, i) => (
          <div key={i} className="bmv-pos">
            {7 - i}
          </div>
        ))}
      </div>
      <span className="bmv-decimal" />
    </div>
  );
}

function BitRow({
  label,
  val,
  highlightSet,
  rowCls,
}: {
  label: string;
  val: number;
  highlightSet?: Set<number>;
  rowCls?: string;
}) {
  const bits = toBits(val);
  return (
    <div className="bmv-row">
      <span className="bmv-label">{label}</span>
      <div className="bmv-bits">
        {bits.map((bit, i) => {
          let cls = "bmv-bit ";
          if (rowCls) cls += `bmv-${rowCls}`;
          else if (highlightSet?.has(i)) cls += "bmv-active";
          else cls += bit === 1 ? "bmv-one" : "bmv-zero";
          return (
            <div key={i} className={cls}>
              {bit}
            </div>
          );
        })}
      </div>
      <span className="bmv-decimal">= {val}</span>
    </div>
  );
}

function OpRow({ symbol }: { symbol: string }) {
  return (
    <div className="bmv-op-row">
      <span className="bmv-label">{symbol}</span>
    </div>
  );
}

function Divider() {
  return <div className="bmv-divider" />;
}

function Dashboard({ n }: { n: number }) {
  const nm1 = Math.max(n - 1, 0);
  const andResult = n & nm1;
  const isPow2 = n > 0 && andResult === 0;
  const notN = ~n & 0xff;
  const lShift1 = (n << 1) & 0xff;
  const lShift2 = (n << 2) & 0xff;
  const rShift1 = n >>> 1;
  const rShift2 = n >>> 2;
  const nBits = toBits(n);
  const popcount = nBits.filter((b) => b === 1).length;
  const setBitIndices = new Set(
    nBits.map((b, i) => (b === 1 ? i : -1)).filter((i) => i >= 0),
  );

  return (
    <>
      <section className="bmv-section bmv-section--wide">
        <p className="bmv-section-title">Binary Representation</p>
        <div className="bmv-vis">
          <PosRow />
          <BitRow label="n" val={n} />
        </div>
      </section>

      <div className="bmv-grid">
        <section className="bmv-section">
          <p className="bmv-section-title">Count Set Bits</p>
          <div className="bmv-vis">
            <PosRow />
            <BitRow label="n" val={n} highlightSet={setBitIndices} />
          </div>
          <div className="bmv-badge">
            {popcount} set bit{popcount !== 1 ? "s" : ""}
          </div>
        </section>

        <section className="bmv-section">
          <p className="bmv-section-title">Power of 2 - n &amp; (n-1)</p>
          <div className="bmv-vis">
            <PosRow />
            <BitRow label="n" val={n} />
            <OpRow symbol="&" />
            <BitRow label="n-1" val={nm1} />
            <Divider />
            <BitRow
              label="="
              val={andResult}
              rowCls={isPow2 ? "done" : "active"}
            />
          </div>
          <div className={`bmv-verdict ${isPow2 ? "bmv-yes" : "bmv-no"}`}>
            {isPow2 ? `✓ ${n} is a power of 2` : `✗ ${n} is not a power of 2`}
          </div>
        </section>

        <section className="bmv-section">
          <p className="bmv-section-title">Bitwise NOT ~n</p>
          <div className="bmv-vis">
            <PosRow />
            <BitRow label="n" val={n} />
            <OpRow symbol="~" />
            <BitRow label="~n" val={notN} rowCls="done" />
          </div>
          <p className="bmv-hint">Every 0 → 1, every 1 → 0</p>
        </section>

        <section className="bmv-section">
          <p className="bmv-section-title">Left Shift - n &lt;&lt; k</p>
          <div className="bmv-vis">
            <PosRow />
            <BitRow label="n" val={n} />
            <BitRow label="&lt;&lt; 1" val={lShift1} rowCls="done" />
            <BitRow label="&lt;&lt; 2" val={lShift2} rowCls="done" />
          </div>
          <p className="bmv-hint">Each left shift multiplies by 2</p>
        </section>

        <section className="bmv-section">
          <p className="bmv-section-title">Right Shift - n &gt;&gt; k</p>
          <div className="bmv-vis">
            <PosRow />
            <BitRow label="n" val={n} />
            <BitRow label="&gt;&gt; 1" val={rShift1} rowCls="done" />
            <BitRow label="&gt;&gt; 2" val={rShift2} rowCls="done" />
          </div>
          <p className="bmv-hint">Each right shift divides by 2 (floor)</p>
        </section>
      </div>
    </>
  );
}

export default function BitManipulatorVisualizer({
  steps,
  inputControls,
}: Props) {
  const rawBits = steps[0]?.array;
  const n = rawBits ? parseInt(rawBits.join("") || "0", 2) : null;

  return (
    <div className="bmv-page">
      {inputControls && <div className="bmv-top-controls">{inputControls}</div>}
      {n !== null ? (
        <Dashboard n={n} />
      ) : (
        <p className="bmv-empty">
          Enter a value (0–255) and press Run to explore all bitwise operations.
        </p>
      )}
    </div>
  );
}
