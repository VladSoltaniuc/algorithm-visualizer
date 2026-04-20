import { useMemo } from "react";
import VisControls from "../VisControls/VisControls";
import type { AlgorithmStep } from "../../types";
import "./TreeVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
  inputValues: number[];
  inputControls?: React.ReactNode;
}

/* ── BST helpers ── */
interface TNode {
  value: number;
  left: TNode | null;
  right: TNode | null;
}

function bstInsert(root: TNode | null, val: number): TNode {
  if (!root) return { value: val, left: null, right: null };
  if (val < root.value) root.left = bstInsert(root.left, val);
  else root.right = bstInsert(root.right, val);
  return root;
}

/* ── Layout: inorder position → x, depth → y ── */
interface LayoutNode {
  value: number;
  x: number;
  y: number;
}

function layoutTree(root: TNode) {
  const nodes: LayoutNode[] = [];
  const edges: [number, number][] = [];
  let pos = 0;

  function walk(node: TNode, depth: number): number {
    let leftIdx = -1;
    if (node.left) leftIdx = walk(node.left, depth + 1);

    const idx = nodes.length;
    nodes.push({ value: node.value, x: pos++, y: depth });

    if (leftIdx >= 0) edges.push([idx, leftIdx]);
    if (node.right) {
      const rightIdx = walk(node.right, depth + 1);
      edges.push([idx, rightIdx]);
    }
    return idx;
  }

  walk(root, 0);

  return {
    nodes,
    edges,
    maxX: Math.max(...nodes.map((n) => n.x), 0),
    maxY: Math.max(...nodes.map((n) => n.y), 0),
  };
}

// Build a layout directly from a null-inclusive BFS level-order array.
// Index i has left child at 2i+1, right child at 2i+2.
function layoutFromLevelOrder(arr: (number | null)[]) {
  const nodeMap = new Map<number, number>(); // arr-index → nodes[] index
  const nodes: LayoutNode[] = [];
  const edges: [number, number][] = [];
  let pos = 0;

  function inorder(i: number) {
    if (i >= arr.length || arr[i] === null) return;
    inorder(2 * i + 1);
    const nodeIdx = nodes.length;
    const depth = Math.floor(Math.log2(i + 1));
    nodes.push({ value: arr[i]!, x: pos++, y: depth });
    nodeMap.set(i, nodeIdx);
    inorder(2 * i + 2);
  }

  inorder(0);

  for (let i = 0; i < arr.length; i++) {
    if (arr[i] === null) continue;
    const parentIdx = nodeMap.get(i);
    if (parentIdx === undefined) continue;
    for (const ci of [2 * i + 1, 2 * i + 2]) {
      if (ci < arr.length && arr[ci] !== null) {
        const childIdx = nodeMap.get(ci);
        if (childIdx !== undefined) edges.push([parentIdx, childIdx]);
      }
    }
  }

  return {
    nodes,
    edges,
    maxX: nodes.length > 0 ? Math.max(...nodes.map((n) => n.x)) : 0,
    maxY: nodes.length > 0 ? Math.max(...nodes.map((n) => n.y)) : 0,
  };
}

/* ── Component ── */
export default function TreeVisualizer({
  steps,
  onRun,
  disabled,
  inputValues,
  inputControls,
}: Props) {
  const layout = useMemo(() => {
    if (!inputValues.length) return null;
    let root: TNode | null = null;
    for (const v of inputValues) root = bstInsert(root, v);
    if (!root) return null;
    return layoutTree(root);
  }, [inputValues]);

  return (
    <VisControls
      steps={steps}
      onRun={onRun}
      disabled={disabled}
      hideDescription
      inputControls={inputControls}
    >
      {(step: AlgorithmStep) => {
        const isFinal = step.stepNumber === steps[steps.length - 1]?.stepNumber;
        /* Fallback for Huffman / non-numeric input */
        if (!layout && !step.treeLevelOrder?.length) {
          const labels = step.labels ?? [];
          const notes = step.notes ?? [];
          const hasLabels = labels.length > 0;
          const hasNotes = notes.length > 0;
          return (
            <>
              <div className="tree-fallback">
                {step.array.map((v, i) => {
                  const hl = step.highlightIndices?.includes(i);
                  const done = step.sortedIndices?.includes(i);
                  return (
                    <div
                      key={i}
                      className={`tree-fb-cell${hl ? " hl" : ""}${done ? " done" : ""}`}
                    >
                      {hasLabels && (
                        <span className="tree-fb-label">{labels[i]}</span>
                      )}
                      <span className="tree-fb-value">{v}</span>
                      {hasNotes && notes[i] && (
                        <span className="tree-fb-note">{notes[i]}</span>
                      )}
                    </div>
                  );
                })}
              </div>
              <div className={`step-info${isFinal ? " final" : ""}`}>
                {step.description}
              </div>
            </>
          );
        }

        // Use per-step dynamic layout (e.g. Invert) if available, otherwise static
        const activeLayout =
          step.treeLevelOrder && step.treeLevelOrder.length > 0
            ? layoutFromLevelOrder(step.treeLevelOrder)
            : layout!;

        const { nodes, edges, maxX, maxY } = activeLayout;
        const xSp = Math.min(48, maxX > 0 ? 320 / maxX : 48);
        const ySp = 52;
        const pad = 30;
        const nR = Math.min(18, xSp * 0.42);
        const svgW = maxX * xSp + pad * 2;
        const svgH = maxY * ySp + pad * 2;

        /* Determine highlighted / finalized values from step data */
        const hlVals = new Set(
          (step.highlightIndices ?? [])
            .map((i) => step.array[i])
            .filter((v) => v !== undefined),
        );
        const doneVals = new Set(
          (step.sortedIndices ?? [])
            .map((i) => step.array[i])
            .filter((v) => v !== undefined),
        );

        return (
          <>
            <div className="tree-vis">
              <svg viewBox={`0 0 ${svgW} ${svgH}`} className="tree-svg">
                {edges.map(([pi, ci], i) => {
                  const p = nodes[pi];
                  const c = nodes[ci];
                  return (
                    <line
                      key={`e${i}`}
                      x1={p.x * xSp + pad}
                      y1={p.y * ySp + pad}
                      x2={c.x * xSp + pad}
                      y2={c.y * ySp + pad}
                      className="tree-edge"
                    />
                  );
                })}
                {nodes.map((n, i) => {
                  let fill = "#3a86ff";
                  if (doneVals.has(n.value)) fill = "#06d6a0";
                  if (hlVals.has(n.value)) fill = "#e94560";
                  const cx = n.x * xSp + pad;
                  const cy = n.y * ySp + pad;
                  return (
                    <g key={i}>
                      <circle
                        cx={cx}
                        cy={cy}
                        r={nR}
                        fill={fill}
                        className="tree-node"
                      />
                      <text
                        x={cx}
                        y={cy}
                        textAnchor="middle"
                        dominantBaseline="central"
                        className="tree-node-label"
                      >
                        {n.value}
                      </text>
                    </g>
                  );
                })}
              </svg>
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
