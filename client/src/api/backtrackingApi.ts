import type { AlgorithmStep } from '../types';

const BASE = '/api/backtracking';

async function post(url: string, body: unknown): Promise<AlgorithmStep[]> {
  const res = await fetch(url, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  });
  if (!res.ok) throw new Error((await res.text()) || `Server error: ${res.status}`);
  return res.json();
}

async function get(url: string): Promise<AlgorithmStep[]> {
  const res = await fetch(url);
  if (!res.ok) throw new Error((await res.text()) || `Server error: ${res.status}`);
  return res.json();
}

function parseGraphInput(input: string): { nodeCount: number; edges: number[][] } {
  const parts = input.split(';').map(s => s.trim()).filter(Boolean);
  const nodeCount = parseInt(parts[0], 10);
  const edges = parts.slice(1).map(e => e.split(',').map(Number));
  return { nodeCount, edges };
}

export const backtrackingApi: Record<string, (...args: unknown[]) => Promise<AlgorithmStep[]>> = {
  nQueens: (n: unknown) => get(`${BASE}/n-queens/${n}`),
  sudoku: (grid: unknown) => post(`${BASE}/sudoku`, grid),
  permutations: (arr: unknown) => post(`${BASE}/permutations`, arr),
  subsets: (arr: unknown) => post(`${BASE}/subsets`, arr),
  ratInMaze: (grid: unknown, n?: unknown) =>
    post(`${BASE}/rat-in-maze/${n}`, grid),
  knightsTour: (n: unknown) => get(`${BASE}/knights-tour/${n}`),
  wordSearch: (gridStr: unknown, pattern?: unknown) =>
    post(`${BASE}/word-search`, { text: gridStr, pattern: pattern ?? '' }),
  combinationSum: (arr: unknown, target?: unknown) =>
    post(`${BASE}/combination-sum/${target}`, arr),
  palindromePartitioning: (text: unknown) =>
    post(`${BASE}/palindrome-partitioning`, { text, pattern: '' }),
  graphColoring: (graphStr: unknown, numColors?: unknown) => {
    const { nodeCount, edges } = parseGraphInput(graphStr as string);
    return post(`${BASE}/graph-coloring/${numColors}`, { nodeCount, edges, startNode: 0 });
  },
};
