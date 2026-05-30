import type { AlgorithmStep } from '../types';

const BASE = '/api/sort';

async function post(url: string, body: number[]): Promise<AlgorithmStep[]> {
  const res = await fetch(url, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) });
  if (!res.ok) throw new Error((await res.text()) || `Server error: ${res.status}`);
  return res.json();
}

export const sortApi: Record<string, (...args: unknown[]) => Promise<AlgorithmStep[]>> = {
  bubbleSort: (input: unknown) => post(`${BASE}/bubble-sort`, input as number[]),
  quickSort: (input: unknown) => post(`${BASE}/quick-sort`, input as number[]),
  mergeSort: (input: unknown) => post(`${BASE}/merge-sort`, input as number[]),
  insertionSort: (input: unknown) => post(`${BASE}/insertion-sort`, input as number[]),
  selectionSort: (input: unknown) => post(`${BASE}/selection-sort`, input as number[]),
};
