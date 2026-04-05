import { useMemo } from "react";
import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./GraphVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
  edges: number[][];
  nodeCount: number;
}

/**
 * Renders nodes in a circle and draws actual graph edges between them.
 * Highlighted = active, sorted = finalized.
 */
export default function GraphVisualizer({
  steps,
  onRun,
  disabled,
  edges,
  nodeCount,
}: Props) {
  return (
    <VisControls steps={steps} onRun={onRun} disabled={disabled}>
      {(step: AlgorithmStep) => {
        const n = step.array.length || nodeCount;
        const hl = new Set(step.highlightIndices ?? []);
        const done = new Set(step.sortedIndices ?? []);

        return (
          <GraphCanvas
            n={n}
            values={step.array}
            hl={hl}
            done={done}
            edges={edges}
          />
        );
      }}
    </VisControls>
  );
}

function GraphCanvas({
  n,
  values,
  hl,
  done,
  edges,
}: {
  n: number;
  values: number[];
  hl: Set<number>;
  done: Set<number>;
  edges: number[][];
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
        {/* actual graph edges */}
        {edges.map((e, i) => {
          const from = e[0];
          const to = e[1];
          if (from >= n || to >= n) return null;
          const p1 = positions[from];
          const p2 = positions[to];
          const weight = e.length > 2 ? e[2] : null;
          const mx = (p1.x + p2.x) / 2;
          const my = (p1.y + p2.y) / 2;
          return (
            <g key={`e-${i}`}>
              <line
                x1={p1.x}
                y1={p1.y}
                x2={p2.x}
                y2={p2.y}
                className="graph-edge"
              />
              {weight !== null && (
                <text
                  x={mx}
                  y={my}
                  textAnchor="middle"
                  dominantBaseline="central"
                  className="graph-edge-weight"
                >
                  {weight}
                </text>
              )}
            </g>
          );
        })}

        {/* nodes */}
        {positions.map((pos, i) => {
          let fill = "#3a86ff";
          if (done.has(i)) fill = "#06d6a0";
          else if (hl.has(i)) fill = "#e94560";

          const val = values[i];
          const label = val === -1 ? "∞" : String(val ?? "");

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
                y={pos.y + 9}
                textAnchor="middle"
                dominantBaseline="central"
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
