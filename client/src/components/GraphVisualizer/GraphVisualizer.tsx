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
  directed?: boolean;
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
  directed,
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
            labels={step.labels}
            notes={step.notes}
            directed={directed}
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
  labels,
  notes,
  directed,
}: {
  n: number;
  values: number[];
  hl: Set<number>;
  done: Set<number>;
  edges: number[][];
  labels?: string[];
  notes?: string[];
  directed?: boolean;
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

  // Build component → color map from labels (distinct color per unique label value)
  const componentPalette = [
    "#3a86ff",
    "#a855f7",
    "#f4a11d",
    "#14b8a6",
    "#ec4899",
    "#f97316",
    "#84cc16",
    "#06b6d4",
  ];
  const componentColorMap = new Map<string, string>();
  if (labels?.length) {
    let idx = 0;
    [...new Set(labels)].forEach((lbl) => {
      componentColorMap.set(
        lbl,
        componentPalette[idx++ % componentPalette.length],
      );
    });
  }

  return (
    <div className="graph-vis">
      <svg viewBox={`0 0 ${size} ${size}`} className="graph-svg">
        <defs>
          <marker
            id="arrowhead"
            markerUnits="userSpaceOnUse"
            markerWidth="11"
            markerHeight="8"
            refX="11"
            refY="4"
            orient="auto"
          >
            <path d="M0,0 L0,8 L11,4 z" fill="#8888aa" />
          </marker>
        </defs>
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
          // For directed edges, trim line so it runs between circle edges (not centers)
          const nr = 22;
          const edgeDx = p2.x - p1.x,
            edgeDy = p2.y - p1.y;
          const edgeLen = Math.sqrt(edgeDx * edgeDx + edgeDy * edgeDy) || 1;
          const ux = edgeDx / edgeLen,
            uy = edgeDy / edgeLen;
          const lx1 = directed ? p1.x + ux * nr : p1.x;
          const ly1 = directed ? p1.y + uy * nr : p1.y;
          const lx2 = directed ? p2.x - ux * (nr + 2) : p2.x;
          const ly2 = directed ? p2.y - uy * (nr + 2) : p2.y;
          return (
            <g key={`e-${i}`}>
              <line
                x1={lx1}
                y1={ly1}
                x2={lx2}
                y2={ly2}
                className="graph-edge"
                markerEnd={directed ? "url(#arrowhead)" : undefined}
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
          // Component color from labels (if present)
          const compLabel = labels?.[i];
          if (compLabel && componentColorMap.has(compLabel)) {
            fill = componentColorMap.get(compLabel)!;
          }
          // Active highlights override component color
          if (done.has(i)) fill = "#06d6a0";
          else if (hl.has(i)) fill = "#e94560";

          // Sub-label: prefer notes[i], then fallback to parent/dist value
          const subLabel =
            notes?.[i] ?? (values[i] === -1 ? "∞" : String(values[i] ?? ""));

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
                {subLabel}
              </text>
            </g>
          );
        })}
      </svg>
    </div>
  );
}
