import { useMemo } from "react";
import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./GraphVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
}

/**
 * Shows nodes in a circle layout. Array[i] = per-node value (visited/dist/color/parent).
 * Highlighted nodes are active; sorted nodes are finalized.
 */
export default function GraphVisualizer({ steps, onRun, disabled }: Props) {
  return (
    <VisControls steps={steps} onRun={onRun} disabled={disabled}>
      {(step: AlgorithmStep) => {
        const n = step.array.length;
        const hl = new Set(step.highlightIndices ?? []);
        const done = new Set(step.sortedIndices ?? []);

        return <GraphCanvas n={n} values={step.array} hl={hl} done={done} />;
      }}
    </VisControls>
  );
}

function GraphCanvas({
  n,
  values,
  hl,
  done,
}: {
  n: number;
  values: number[];
  hl: Set<number>;
  done: Set<number>;
}) {
  const size = 380;
  const cx = size / 2;
  const cy = size / 2;
  const r = size / 2 - 36;

  const positions = useMemo(() => {
    return Array.from({ length: n }, (_, i) => {
      const angle = (2 * Math.PI * i) / n - Math.PI / 2;
      return { x: cx + r * Math.cos(angle), y: cy + r * Math.sin(angle) };
    });
  }, [n, cx, cy, r]);

  return (
    <div className="graph-vis">
      <svg viewBox={`0 0 ${size} ${size}`} className="graph-svg">
        {/* edges between consecutive nodes (simple chain for visual context) */}
        {positions.map((pos, i) => {
          if (i === n - 1) return null;
          const next = positions[i + 1];
          return (
            <line
              key={`e-${i}`}
              x1={pos.x}
              y1={pos.y}
              x2={next.x}
              y2={next.y}
              stroke="#ddd"
              strokeWidth="1.5"
            />
          );
        })}

        {/* nodes */}
        {positions.map((pos, i) => {
          let fill = "#3a86ff";
          if (done.has(i)) fill = "#06d6a0";
          else if (hl.has(i)) fill = "#e94560";

          const val = values[i];
          const label = val === -1 ? "∞" : String(val);

          return (
            <g key={i}>
              <circle
                cx={pos.x}
                cy={pos.y}
                r={22}
                fill={fill}
                className={`graph-node${hl.has(i) ? " pulse" : ""}`}
              />
              <text
                x={pos.x}
                y={pos.y - 2}
                textAnchor="middle"
                dominantBaseline="central"
                className="graph-node-id"
              >
                {i}
              </text>
              <text
                x={pos.x}
                y={pos.y + 13}
                textAnchor="middle"
                className="graph-node-val"
              >
                {label}
              </text>
            </g>
          );
        })}
      </svg>
    </div>
  );
}
