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
    <VisControls
      steps={steps}
      onRun={onRun}
      disabled={disabled}
      hideDescription
    >
      {(step: AlgorithmStep) => {
        const n = step.array.length || nodeCount;
        const hl = new Set(step.highlightIndices ?? []);
        const done = new Set(step.sortedIndices ?? []);
        const isFinal = step.stepNumber === steps[steps.length - 1]?.stepNumber;
        const hasCycleTable = !!(
          step.dpMatrix &&
          step.rowLabels &&
          step.colLabels
        );

        return (
          <>
            <GraphCanvas
              n={n}
              values={step.array}
              hl={hl}
              done={done}
              edges={edges}
              labels={step.labels}
              notes={hasCycleTable ? undefined : step.notes}
              directed={directed}
            />
            {hasCycleTable && (
              <>
                <CycleLegend />
                <StateArrayTable
                  matrix={step.dpMatrix!}
                  rowLabels={step.rowLabels!}
                  colLabels={step.colLabels!}
                  highlightIndices={hl}
                  changedCol={step.highlightCol ?? -1}
                />
              </>
            )}
            <div className={`step-info${isFinal ? " final" : ""}`}>
              {step.description}
            </div>
          </>
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

  // DFS state labels use fixed semantic colors — not the component palette
  const DFS_STATE_COLORS: Record<string, string> = {
    unvisited: "#3a86ff",
    stack: "#f4a11d",
    done: "#06d6a0",
  };

  // Build component → color map from labels (distinct color per unique label value)
  // Skip known DFS state labels so they don't consume palette slots
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
    [...new Set(labels)]
      .filter((lbl) => !(lbl in DFS_STATE_COLORS))
      .forEach((lbl) => {
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
          // DFS state labels take fixed semantic colors
          const compLabel = labels?.[i];
          if (compLabel && compLabel in DFS_STATE_COLORS) {
            fill = DFS_STATE_COLORS[compLabel];
          } else if (compLabel && componentColorMap.has(compLabel)) {
            fill = componentColorMap.get(compLabel)!;
          }
          // Active highlights override component/state color
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

function CycleLegend() {
  return (
    <div className="cycle-legend">
      <span className="cycle-legend-item">
        <span className="cycle-legend-dot" style={{ background: "#3a86ff" }} />
        Unvisited
      </span>
      <span className="cycle-legend-item">
        <span className="cycle-legend-dot" style={{ background: "#f4a11d" }} />
        On current path
      </span>
      <span className="cycle-legend-item">
        <span className="cycle-legend-dot" style={{ background: "#06d6a0" }} />
        Fully processed
      </span>
      <span className="cycle-legend-item">
        <span className="cycle-legend-dot" style={{ background: "#e94560" }} />
        Active this step
      </span>
    </div>
  );
}

function StateArrayTable({
  matrix,
  rowLabels,
  colLabels,
  highlightIndices,
  changedCol,
}: {
  matrix: number[][];
  rowLabels: string[];
  colLabels: string[];
  highlightIndices: Set<number>;
  changedCol: number;
}) {
  return (
    <div className="state-array-wrap">
      <table className="state-array-table">
        <thead>
          <tr>
            <th className="state-array-corner" />
            {colLabels.map((col, j) => (
              <th
                key={`col-${col}`}
                className={`state-array-col${highlightIndices.has(j) ? " hl" : ""}${j === changedCol ? " changed" : ""}`}
              >
                {col}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {matrix.map((row, i) => (
            <tr key={`row-${rowLabels[i]}`}>
              <td className="state-array-label">{rowLabels[i]}</td>
              {row.map((val, j) => {
                const isChanged = j === changedCol;
                return (
                  <td
                    key={`cell-${rowLabels[i]}-${j}`}
                    className={`state-array-cell${val ? " on" : " off"}${isChanged ? " changed" : ""}${highlightIndices.has(j) ? " hl" : ""}`}
                  >
                    {val ? "✓" : "✗"}
                  </td>
                );
              })}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
