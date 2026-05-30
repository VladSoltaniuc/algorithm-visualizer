import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./HuffmanVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
  inputControls?: React.ReactNode;
  speed: number;
  isPaused?: boolean;
  onComplete?: () => void;
}

/* ── Step-type detection ── */
function isStatsStep(step: AlgorithmStep) {
  return step.labels?.length === 2 && step.labels[0] === "Original (ASCII)";
}
function isCodesStep(step: AlgorithmStep) {
  return (
    !isStatsStep(step) &&
    (step.notes?.length ?? 0) > 0 &&
    /^[01]+$/.test(step.notes?.[0] ?? "")
  );
}

/* ── Queue / Frequency view ── */
function QueueView({ step }: { readonly step: AlgorithmStep }) {
  const hl = new Set(step.highlightIndices ?? []);
  const isFreqStep = step.highlightIndices?.length === step.array.length;

  return (
    <div className="huff-section">
      <p className="huff-section-title">
        {isFreqStep
          ? "Character Frequencies"
          : "Priority Queue  (lowest weight = next to merge)"}
      </p>
      <div className="huff-queue">
        {step.array.map((weight, i) => {
          const label = step.labels?.[i] ?? "?";
          const isNew = !isFreqStep && hl.has(i);
          const isMerged = label.includes("+");
          const charCount = isMerged ? label.split("+").length : 1;
          return (
            <div
              key={`${label}-${weight}`}
              className={`huff-node${isNew ? " huff-node--new" : ""}${isMerged ? " huff-node--merged" : ""}`}
            >
              {isMerged ? (
                <>
                  <span className="huff-node-icon">⊕</span>
                  <span className="huff-node-merged-count">
                    {charCount} chars
                  </span>
                </>
              ) : (
                <span className="huff-node-char">{label}</span>
              )}
              <span className="huff-node-weight">{weight}</span>
            </div>
          );
        })}
      </div>
      {!isFreqStep && (
        <p className="huff-queue-hint">
          Each box is a node in the tree. ⊕ = merged subtree. Blue = just
          combined. The two leftmost nodes merge next.
        </p>
      )}
    </div>
  );
}

/* ── Codes table view ── */
function CodesView({ step }: { readonly step: AlgorithmStep }) {
  const maxLen = Math.max(...step.array, 1);
  return (
    <div className="huff-section">
      <p className="huff-section-title">
        Huffman Codes &nbsp;-&nbsp; frequent characters get shorter codes
      </p>
      <div className="huff-codes-table">
        <div className="huff-codes-head">
          <span>Char</span>
          <span>Code</span>
          <span>Bits</span>
          <span></span>
        </div>
        {step.array.map((len, i) => {
          const code = step.notes?.[i] ?? "";
          const char = step.labels?.[i] ?? "?";
          const ratio = len / maxLen;
          return (
            <div key={i} className="huff-codes-row">
              <span className="huff-code-char">{char}</span>
              <span className="huff-code-string">{code}</span>
              <span className="huff-code-len">{len}</span>
              <div className="huff-code-bar-track">
                <div
                  className="huff-code-bar-fill"
                  style={{ width: `${ratio * 100}%` }}
                />
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
}

/* ── Encoded bit-stream view ── */
function EncodedView({
  statsStep,
  codesStep,
}: {
  readonly statsStep: AlgorithmStep;
  readonly codesStep: AlgorithmStep;
}) {
  const originalText = statsStep.notes?.[2];
  if (!originalText) return null;

  const codeMap = new Map<string, string>();
  codesStep.labels?.forEach((char, i) => {
    const bits = codesStep.notes?.[i] ?? "";
    codeMap.set(char, bits);
    if (char === "⎵") codeMap.set(" ", bits);
  });

  return (
    <div className="huff-section huff-encoded-section">
      <p className="huff-section-title">Encoded Bit Stream</p>
      <div className="huff-encoded-chars">
        {[...originalText].map((ch, i) => {
          const isSpace = ch === " ";
          const displayChar = isSpace ? "⎵" : ch;
          const bits = codeMap.get(ch) ?? "";
          return (
            <div
              key={`${ch}-${i}`}
              className={`huff-encoded-cell${isSpace ? " huff-encoded-cell--space" : ""}`}
            >
              <span className="huff-encoded-char">{displayChar}</span>
              <span className="huff-encoded-bits">{bits}</span>
            </div>
          );
        })}
      </div>
    </div>
  );
}

/* ── Compression stats view ── */
function StatsView({ step }: { readonly step: AlgorithmStep }) {
  const [original, huffman] = step.array;
  const saved = original - huffman;
  const ratio = huffman / original;
  const pct = ((1 - ratio) * 100).toFixed(1);

  return (
    <div className="huff-section huff-stats">
      <p className="huff-section-title">Compression Result</p>
      <div className="huff-stat-bars">
        <div className="huff-stat-row">
          <span className="huff-stat-label">Original (ASCII)</span>
          <div className="huff-stat-track">
            <div
              className="huff-stat-fill huff-stat-original"
              style={{ width: "100%" }}
            >
              {original} bits
            </div>
          </div>
        </div>
        <div className="huff-stat-row">
          <span className="huff-stat-label">Huffman</span>
          <div className="huff-stat-track">
            <div
              className="huff-stat-fill huff-stat-huffman"
              style={{ width: `${ratio * 100}%` }}
            >
              {huffman} bits
            </div>
          </div>
        </div>
      </div>
      <div className="huff-savings">
        <span className="huff-savings-num">-{saved} bits</span>
        <span className="huff-savings-pct">{pct}% smaller</span>
      </div>
      <p className="huff-notes">{step.notes?.[1]}</p>
    </div>
  );
}

/* ── Main component ── */
export default function HuffmanVisualizer({
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
      inputControls={inputControls}
      speed={speed}
      isPaused={isPaused}
      onComplete={onComplete}
    >
      {(step: AlgorithmStep) => {
        const codesStep = steps.find(isCodesStep);
        if (isStatsStep(step))
          return (
            <>
              <StatsView step={step} />
              {codesStep && <CodesView step={codesStep} />}
              {codesStep && (
                <EncodedView statsStep={step} codesStep={codesStep} />
              )}
            </>
          );
        if (isCodesStep(step)) return <CodesView step={step} />;
        return <QueueView step={step} />;
      }}
    </VisControls>
  );
}
