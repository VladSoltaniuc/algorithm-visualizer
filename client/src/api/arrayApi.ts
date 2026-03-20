import type { AlgorithmStep } from '../types';

const BASE = '/api/array';

async function post(url: string, body: number[]): Promise<AlgorithmStep[]> {
  const res = await fetch(url, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  });
  if (!res.ok) {
    const text = await res.text();
    throw new Error(text || `Server error: ${res.status}`);
  }
  return res.json();
}

export const arrayApi = {
  bubbleSort: (arr: number[]) => post(`${BASE}/bubble-sort`, arr),
  quickSort: (arr: number[]) => post(`${BASE}/quick-sort`, arr),
  mergeSort: (arr: number[]) => post(`${BASE}/merge-sort`, arr),
  binarySearch: (arr: number[], target: number) => post(`${BASE}/binary-search/${target}`, arr),
  linearSearch: (arr: number[], target: number) => post(`${BASE}/linear-search/${target}`, arr),
  insertionSort: (arr: number[]) => post(`${BASE}/insertion-sort`, arr),
  selectionSort: (arr: number[]) => post(`${BASE}/selection-sort`, arr),
  twoPointers: (arr: number[], target: number) => post(`${BASE}/two-pointers/${target}`, arr),
  slidingWindow: (arr: number[], windowSize: number) => post(`${BASE}/sliding-window/${windowSize}`, arr),
  kadane: (arr: number[]) => post(`${BASE}/kadane`, arr),
};
