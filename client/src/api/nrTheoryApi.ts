import type { AlgorithmStep } from '../types';

const BASE = '/api/nrtheory';

async function get(url: string): Promise<AlgorithmStep[]> {
  const res = await fetch(url);
  if (!res.ok) throw new Error((await res.text()) || `Server error: ${res.status}`);
  return res.json();
}


export const nrTheoryApi: Record<string, (...args: unknown[]) => Promise<AlgorithmStep[]>> = {
  sieve: (n: unknown) => get(`${BASE}/sieve/${n}`),
  gcd: (input: unknown) => {
    const parts = String(input).split(',').map(Number);
    return get(`${BASE}/gcd/${parts[0]}/${parts[1]}`);
  },
  primeFactorization: (n: unknown) => get(`${BASE}/prime-factorization/${n}`),
  bitManipulation: (n: unknown) => get(`${BASE}/bit-manipulation/${n}`),
};
