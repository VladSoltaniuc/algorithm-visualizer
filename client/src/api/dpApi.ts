import type { AlgorithmStep } from '../types';

const BASE = '/api/dynamicprog';

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

export const dpApi: Record<string, (...args: unknown[]) => Promise<AlgorithmStep[]>> = {
  fibonacci: (n: unknown) => get(`${BASE}/fibonacci/${n}`),
  knapsack: (weights: unknown, capacity?: unknown, valuesStr?: unknown) => {
    const values = typeof valuesStr === 'string' ? valuesStr.split(',').map(Number) : [];
    return post(`${BASE}/knapsack`, { weights, values, capacity: Number(capacity ?? 0) });
  },
  lcs: (text: unknown, pattern?: unknown) =>
    post(`${BASE}/lcs`, { text, pattern: pattern ?? '' }),
  lis: (arr: unknown) => post(`${BASE}/lis`, arr),
  coinChange: (coins: unknown, amount?: unknown) =>
    post(`${BASE}/coin-change/${amount}`, coins),
  levenshtein: (text: unknown, pattern?: unknown) =>
    post(`${BASE}/levenshtein`, { text, pattern: pattern ?? '' }),
  subsetSum: (arr: unknown, target?: unknown) =>
    post(`${BASE}/subset-sum/${target}`, arr),
  climbingStairs: (n: unknown) => get(`${BASE}/climbing-stairs/${n}`),
};
