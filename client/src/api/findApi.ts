import type { AlgorithmStep } from '../types';

const BASE = '/api/find';

async function post(url: string, body: number[]): Promise<AlgorithmStep[]> {
  const res = await fetch(url, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) });
  if (!res.ok) throw new Error((await res.text()) || `Server error: ${res.status}`);
  return res.json();
}

export const findApi: Record<string, (...args: unknown[]) => Promise<AlgorithmStep[]>> = {
  binarySearch: (input: unknown, target: unknown) => post(`${BASE}/binary-search/${target}`, input as number[]),
  linearSearch: (input: unknown, target: unknown) => post(`${BASE}/linear-search/${target}`, input as number[]),
  twoPointers: (input: unknown, target: unknown) => post(`${BASE}/two-pointers/${target}`, input as number[]),
  slidingWindow: (input: unknown, windowSize: unknown) => post(`${BASE}/sliding-window/${windowSize}`, input as number[]),
  kadane: (input: unknown) => post(`${BASE}/kadane`, input as number[]),
};
