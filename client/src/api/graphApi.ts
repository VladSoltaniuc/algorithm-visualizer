import type { AlgorithmStep } from '../types';

const BASE = '/api/graph';

async function postJson(url: string, body: unknown): Promise<AlgorithmStep[]> {
  const res = await fetch(url, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  });
  if (!res.ok) throw new Error((await res.text()) || `Server error: ${res.status}`);
  return res.json();
}

function parseGraphInput(input: string): { nodeCount: number; edges: number[][] } {
  const parts = input.split(';').map(s => s.trim()).filter(Boolean);
  const nodeCount = parseInt(parts[0], 10);
  const edges = parts.slice(1).map(e => e.split(',').map(Number));
  return { nodeCount, edges };
}

export const graphApi: Record<string, (...args: unknown[]) => Promise<AlgorithmStep[]>> = {
  bfs: (graphStr: unknown, startNode?: unknown) => {
    const { nodeCount, edges } = parseGraphInput(graphStr as string);
    return postJson(`${BASE}/bfs`, { nodeCount, edges, startNode: Number(startNode ?? 0) });
  },
  dfs: (graphStr: unknown, startNode?: unknown) => {
    const { nodeCount, edges } = parseGraphInput(graphStr as string);
    return postJson(`${BASE}/dfs`, { nodeCount, edges, startNode: Number(startNode ?? 0) });
  },
  dijkstra: (graphStr: unknown, startNode?: unknown) => {
    const { nodeCount, edges } = parseGraphInput(graphStr as string);
    return postJson(`${BASE}/dijkstra`, { nodeCount, edges, startNode: Number(startNode ?? 0) });
  },
  bellmanFord: (graphStr: unknown, startNode?: unknown) => {
    const { nodeCount, edges } = parseGraphInput(graphStr as string);
    return postJson(`${BASE}/bellman-ford`, { nodeCount, edges, startNode: Number(startNode ?? 0) });
  },
  floydWarshall: (graphStr: unknown) => {
    const { nodeCount, edges } = parseGraphInput(graphStr as string);
    return postJson(`${BASE}/floyd-warshall`, { nodeCount, edges, startNode: 0 });
  },
  topologicalSort: (graphStr: unknown) => {
    const { nodeCount, edges } = parseGraphInput(graphStr as string);
    return postJson(`${BASE}/topological-sort`, { nodeCount, edges, startNode: 0 });
  },
  cycleDetection: (graphStr: unknown) => {
    const { nodeCount, edges } = parseGraphInput(graphStr as string);
    return postJson(`${BASE}/cycle-detection`, { nodeCount, edges, startNode: 0 });
  },
  unionFind: (graphStr: unknown) => {
    const { nodeCount, edges } = parseGraphInput(graphStr as string);
    return postJson(`${BASE}/union-find`, { nodeCount, edges, startNode: 0 });
  },
  kruskal: (graphStr: unknown) => {
    const { nodeCount, edges } = parseGraphInput(graphStr as string);
    return postJson(`${BASE}/kruskal`, { nodeCount, edges, startNode: 0 });
  },
  prim: (graphStr: unknown, startNode?: unknown) => {
    const { nodeCount, edges } = parseGraphInput(graphStr as string);
    return postJson(`${BASE}/prim`, { nodeCount, edges, startNode: Number(startNode ?? 0) });
  },
};
