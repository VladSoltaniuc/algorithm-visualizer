import type { AlgorithmStep } from '../types';

const BASE = '/api/nrtheory';

async function get(url: string): Promise<AlgorithmStep[]> {
  const res = await fetch(url);
  if (!res.ok) throw new Error((await res.text()) || `Server error: ${res.status}`);
  return res.json();
}


export const nrTheoryApi: Record<string, (...args: unknown[]) => Promise<AlgorithmStep[]>> = {
  bitManipulation: (n: unknown) => get(`${BASE}/bit-manipulation/${n}`),
};
